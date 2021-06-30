using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hangfire.HangfireHelper
{
    public class HangfireStorage
    {
        public enum Storage
        {
            Mysql,
            SqlServer,
            Redis
        }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public Storage DbType { get; set; } = HangfireStorage.Storage.SqlServer;

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        #region redis所使用

        /// <summary>
        /// redis所使用的db
        /// </summary>
        public int RedisDbNum { get; set; }

        /// <summary>
        /// redis所有
        /// 任务转移间隔, 在这段间隔内，后台任务任为同一个worker处理；超时后将转移到另一个worker处理
        /// 单位分钟
        /// </summary>
        public int InvisibilityTimeout { get; set; } = 180;

        /// <summary>
        /// redis 前缀key
        /// </summary>
        public string KeyPrefix { get; set; } = "hangfire:";

        #endregion


        /// <summary>
        /// 作业队列轮询间隔。 默认值为 15 秒。
        /// 单位秒
        /// </summary>
        public int QueuePollInterval { get; set; } = 15;

        /// <summary>
        /// 作业过期检查间隔（管理过期记录）。 默认为 1 小时。
        /// 单位小时
        /// </summary>
        public int JobExpirationCheckInterval { get; set; } = 1;

        /// <summary>
        /// 聚合计数器的间隔。 默认为 5 分钟。
        /// 单位分钟
        /// </summary>
        public int CountersAggregateInterval { get; set; } = 5;

        /// <summary>
        /// 如果是true，会创建数据库
        /// </summary>
        public bool PrepareSchemaIfNecessary { get; set; } = true;

        /// <summary>
        /// 仪表板作业列表限制。 默认值为 50000。
        /// </summary>
        public int DashboardJobListLimit { get; set; } = 50000;

        /// <summary>
        /// 事务超时时间，默认1分钟
        /// </summary>
        public int TransactionTimeout { get; set; } = 1;

        /// <summary>
        /// 数据库表前缀，默认空
        /// 只在mysql下支持
        /// </summary>
        public string TablesPrefix { get; set; } = "hangfire";

        /// <summary>
        /// sql server 下支持
        /// </summary>
        public string SchemaName { get; set; } = "hangfire";
    }

    public class HangfireConfig
    {

        public enum LogType
        {
            NLog,
            SerilogLog
        }

        public LogType? Log { get; set; }

        /// <summary>
        /// 定义队列
        /// 顺序很重要，worker将首先从 "alpha", "beta", "default" 队列中顺序获取任务，然后从default 队列中获取任务。
        /// [Queue("alpha")]
        /// public void SomeMethod(){}
        /// </summary>
        public string[] Queues { get; set; }

        /// <summary>
        /// 自动重试次数
        /// [AutomaticRetry(Attempts = 0)]
        /// </summary>
        public int AutomaticRetryCount { get; set; } = 3;

        /// <summary>
        /// 自动重试延迟
        /// </summary>
        public int[] AutomaticRetryDelaysInSeconds { get; set; }

        /// <summary>
        /// 删除任务（仅Succeeded和Deleted内置状态，但不是唯一状态Failed）,默认是24小时失效
        /// 单位小时h
        /// </summary>
        public int WithJobExpirationTimeout { get; set; } = 24;

        /// <summary>
        /// 检查计划任务并将其入队,并允许worker执行（知道是enqueue，15秒检查才会真正的任务enqueue）。默认情况下，检查的间隔时间是 15秒,
        /// 单位秒
        /// </summary>
        public int SchedulePollingInterval { get; set; } = 15;

        /// <summary>
        /// 数据库存储
        /// </summary>
        public HangfireStorage Storage { get; set; }
    }
}
