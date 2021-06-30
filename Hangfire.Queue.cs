using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hangfire.HangfireHelper
{
    /// <summary>
    /// 队列
    /// </summary>
    public static partial class HangfireHepler
    {
        public static string Enqueue(Action action)
            => BackgroundJob.Enqueue(() => action());


        public static string Enqueue<T>(System.Linq.Expressions.Expression<Action<T>> action)
        => BackgroundJob.Enqueue<T>(action);

        public static string Enqueue(Func<Task> action)
            => BackgroundJob.Enqueue(() => action());

        public static string Enqueue<T>(System.Linq.Expressions.Expression<Func<T, Task>> action)
            => BackgroundJob.Enqueue<T>(action);
    }
}
