using Hangfire;
using Hangfire.MySql;
using Hangfire.Redis;
using Hangfire.SqlServer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace hangfire.HangfireHelper
{
    public static class HangfireAddServiceExtention
    {
        public static void AddHangfireServiceConfig(this IServiceCollection services, HangfireConfig hangfireConfig)
        {
            if (hangfireConfig == null || hangfireConfig.Storage == null)
                throw new ArgumentNullException("storage 配置不存在");

            services.AddHangfire((provider, configuration) =>
            {
                configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                             .UseSimpleAssemblyNameTypeSerializer()
                             .UseRecommendedSerializerSettings()
                             ;

                //设置日志
                if (hangfireConfig.Log.HasValue)
                    switch (hangfireConfig.Log.Value)
                    {
                        case HangfireConfig.LogType.NLog:
                            configuration.UseNLogLogProvider();
                            break;
                        case HangfireConfig.LogType.SerilogLog:
                            configuration.UseSerilogLogProvider();
                            break;
                        default:
                            break;
                    }

                //配置错误重试次数
                if (hangfireConfig.AutomaticRetryCount > 0)
                    configuration.UseFilter(new AutomaticRetryAttribute()
                    {
                        Attempts = hangfireConfig.AutomaticRetryCount,
                        DelaysInSeconds = hangfireConfig.AutomaticRetryDelaysInSeconds
                    });

                //配置存储
                if (hangfireConfig.Storage.DbType == HangfireStorage.Storage.Mysql)
                {
                    configuration.UseStorage(
                      new MySqlStorage(
                        hangfireConfig.Storage.ConnectionString,
                             new MySqlStorageOptions
                             {
                                 //TransactionIsolationLevel = IsolationLevel.ReadCommitted, // Default is read committed.
                                 QueuePollInterval = TimeSpan.FromSeconds(hangfireConfig.Storage.QueuePollInterval),
                                 JobExpirationCheckInterval = TimeSpan.FromHours(hangfireConfig.Storage.JobExpirationCheckInterval),
                                 CountersAggregateInterval = TimeSpan.FromMinutes(hangfireConfig.Storage.CountersAggregateInterval),
                                 PrepareSchemaIfNecessary = hangfireConfig.Storage.PrepareSchemaIfNecessary,
                                 DashboardJobListLimit = hangfireConfig.Storage.DashboardJobListLimit,
                                 TransactionTimeout = TimeSpan.FromMinutes(hangfireConfig.Storage.TransactionTimeout),
                                 TablesPrefix = hangfireConfig.Storage.TablesPrefix
                             }
                      )
                   );
                }
                else if (hangfireConfig.Storage.DbType == HangfireStorage.Storage.SqlServer)
                {
                    configuration.UseStorage(
                     new SqlServerStorage(
                       hangfireConfig.Storage.ConnectionString,
                            new SqlServerStorageOptions
                            {
                                //TransactionIsolationLevel = IsolationLevel.ReadCommitted, // Default is read committed.
                                QueuePollInterval = TimeSpan.FromSeconds(hangfireConfig.Storage.QueuePollInterval),
                                JobExpirationCheckInterval = TimeSpan.FromHours(hangfireConfig.Storage.JobExpirationCheckInterval),
                                CountersAggregateInterval = TimeSpan.FromMinutes(hangfireConfig.Storage.CountersAggregateInterval),
                                PrepareSchemaIfNecessary = hangfireConfig.Storage.PrepareSchemaIfNecessary,
                                DashboardJobListLimit = hangfireConfig.Storage.DashboardJobListLimit,
                                TransactionTimeout = TimeSpan.FromMinutes(hangfireConfig.Storage.TransactionTimeout),
                                SchemaName = hangfireConfig.Storage.SchemaName,
                                //CommandBatchMaxTimeout =null, 
                                //CommandTimeout=null,
                                //DeleteExpiredBatchSize=1000,
                                //DisableGlobalLocks
                                //EnableHeavyMigrations
                                //ImpersonationFunc
                                //SlidingInvisibilityTimeout
                                //UseFineGrainedLocks
                                //UseIgnoreDupKeyOption
                                //UsePageLocksOnDequeue
                                //UseRecommendedIsolationLevel
                            }
                     )
                  );
                }
                else if (hangfireConfig.Storage.DbType == HangfireStorage.Storage.Redis)
                {
                    var options = new RedisStorageOptions
                    {
                        Prefix = hangfireConfig.Storage.KeyPrefix,
                        InvisibilityTimeout = TimeSpan.FromHours(3),  //任务转移间隔, 在这段间隔内，后台任务任为同一个worker处理；超时后将转移到另一个worker处理
                        Db = 1,

                        //DeletedListSize = 1000,   //	删除列表中的最大可见后台作业，以防止其无限期增长。
                        //SucceededListSize = 10000, //	成功列表中的最大可见后台任务，以防止其无限期增长。
                        //UseTransactions = false,
                        //FetchTimeout,
                        //LifoQueues
                        //ExpiryCheckInterval
                    };

                    configuration.UseRedisStorage(hangfireConfig.Storage.ConnectionString, options);
                }
                else
                    throw new ArgumentException($"并不支持storage 类型为 {hangfireConfig.Storage.DbType }");
            });


            // Add the processing server as IHostedService
            services.AddHangfireServer((provider, option) => {
                //添加队列
                if (hangfireConfig.Queues != null && hangfireConfig.Queues.Count() >= 1)
                {
                    string defaultQueue = "default";
                    List<string> queues = new List<string>();
                    foreach (var item in hangfireConfig.Queues)
                    {
                        if (queues.IndexOf(item) < 0 && item != defaultQueue)
                            queues.Add(item);
                    }
                    queues.Add(defaultQueue);
                    option.Queues = queues.ToArray();
                }
                //option.CancellationCheckInterval
                //option.HeartbeatInterval
                //option.SchedulePollingInterval    //检查计划任务并将其入队,并允许worker执行。默认情况下，检查的间隔时间是 15秒,但您可以更改它，只需将相应的选项传递给 BackgroundJobServer 的构造器。
                //option.ServerCheckInterval
                //option.ServerTimeout
                //option.ShutdownTimeout
                //option.TaskScheduler
                //option.WorkerCount   // Environment.ProcessorCount * 5 配置工作数量

            });

        }
    }
}
