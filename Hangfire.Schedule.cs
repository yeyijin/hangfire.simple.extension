using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hangfire.HangfireHelper
{
    /// <summary>
    /// 延迟执行
    /// </summary>
    public static partial class HangfireHepler
    {
        public static void Schedule(Action action, TimeSpan delay)
        => BackgroundJob.Schedule(() => action(), delay);

        public static void Schedule<T>(System.Linq.Expressions.Expression<Action<T>> action, TimeSpan delay)
        => BackgroundJob.Schedule<T>(action, delay);

        public static void Schedule(Func<Task> action, TimeSpan delay)
        => BackgroundJob.Schedule(() => action(), delay);

        public static void Schedule<T>(System.Linq.Expressions.Expression<Func<T, Task>> action, TimeSpan delay)
        => BackgroundJob.Schedule<T>(action, delay);
    }
}
