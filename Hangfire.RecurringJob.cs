using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hangfire.HangfireHelper
{
    public static partial class HangfireHepler
    {
        /// <summary>
        /// 新增或更新循环任务
        /// </summary>
        /// <param name="action">执行的内容</param>
        /// <param name="cronExpression">cron 表达式，例如:Cron.Daily</param>
        /// <param name="timeZoneInfo">国际化时需要用到的时区</param>
        /// <param name="queueName">进入那个循环调度队列</param>
        /// <param name="recurringJobId">唯一的job名</param>
        public static void AddOrUpdateJob(Action action
            , string cronExpression
            , TimeZoneInfo timeZoneInfo = null
            , string queueName = "default"
            , string recurringJobId = null)
        {
            if (string.IsNullOrWhiteSpace(recurringJobId))
                RecurringJob.AddOrUpdate(() => action(), cronExpression, timeZoneInfo, queueName);
            else
                RecurringJob.AddOrUpdate(recurringJobId, () => action(), cronExpression, timeZoneInfo, queueName);
        }

        /// <summary>
        /// 新增或更新循环任务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="cronExpression"></param>
        /// <param name="timeZoneInfo"></param>
        /// <param name="queueName"></param>
        /// <param name="recurringJobId"></param>
        public static void AddOrUpdateJob<T>(System.Linq.Expressions.Expression< Action<T>> action
            , string cronExpression
            , TimeZoneInfo timeZoneInfo = null, string queueName = "default"
            , string recurringJobId = null)
        {
            if (string.IsNullOrWhiteSpace(recurringJobId))
                RecurringJob.AddOrUpdate<T>(action, cronExpression, timeZoneInfo, queueName);
            else
                RecurringJob.AddOrUpdate<T>(recurringJobId, action, cronExpression, timeZoneInfo, queueName);
        }

        /// <summary>
        /// 新增或更新循环任务
        /// </summary>
        /// <param name="action"></param>
        /// <param name="cronExpression"></param>
        /// <param name="timeZoneInfo"></param>
        /// <param name="queueName"></param>
        /// <param name="recurringJobId"></param>
        public static void AddOrUpdateJob(Func<Task> action
          , string cronExpression
          , TimeZoneInfo timeZoneInfo = null
          , string queueName = "default"
          , string recurringJobId = null)
        {
            if (string.IsNullOrWhiteSpace(recurringJobId))
                RecurringJob.AddOrUpdate(()=>action(), cronExpression, timeZoneInfo, queueName);
            else
                RecurringJob.AddOrUpdate(recurringJobId, () => action(), cronExpression, timeZoneInfo, queueName);
        }


        /// <summary>
        /// 新增或更新循环任务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="cronExpression"></param>
        /// <param name="timeZoneInfo"></param>
        /// <param name="queueName"></param>
        /// <param name="recurringJobId"></param>
        public static void AddOrUpdateJob<T>(System.Linq.Expressions.Expression< Func<T,Task>> action
            , string cronExpression
            , T obj
            , TimeZoneInfo timeZoneInfo = null, string queueName = "default"
            , string recurringJobId = null)
        {
            if (string.IsNullOrWhiteSpace(recurringJobId))
                RecurringJob.AddOrUpdate<T>(action, cronExpression, timeZoneInfo, queueName);
            else
                RecurringJob.AddOrUpdate<T>(recurringJobId,action, cronExpression, timeZoneInfo, queueName);
        }

        
        /// <summary>
        /// 移除存在的循环任务job
        /// </summary>
        /// <param name="jobName"></param>
        public static void RemoveIfExistsJob(string jobName)
        {
            RecurringJob.RemoveIfExists(jobName);
        }

        /// <summary>
        /// 立即触发循环任务job执行
        /// 比如job周五执行，你在周四触发，下一次执行是下一周五，本轮不会再执行
        /// 假如是本周5执行，你再周四触发多次呢，待测试
        /// </summary>
        /// <param name="jobName"></param>
        public static void TriggerJob(string jobName)
        {
            RecurringJob.Trigger(jobName);
        }
    }
}
