using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using System.Timers;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main()
        {
            /*
            在默认情况下，延续任务的调度是无条件的，即无论前导任务是否完成，是否抛出了异常抑或被取消，延续任务都会执行。
            若想更改这个行为，可以为延续任务指定一组TaskContinuationOptions枚举值的组合。

            需要特别指出的是当延续任务指定了上述标志而无法执行时，它并不会被遗忘或者丢弃，而会取消。
            这意味着延续的任何一个任务都将执行，但指定了NotOnCanceled的延续任务除外。
            */
            TaskContinuationOptions OnlyOnRanToCompletion = TaskContinuationOptions.NotOnFaulted | TaskContinuationOptions.NotOnCanceled;

            Task task = Task.Factory.StartNew(() =>
            {
                throw null;
            });

            Task t = task.ContinueWith((t) =>
            {
                Console.WriteLine("t, faulted:{0} canceled:{1}", t.IsFaulted, t.IsCanceled);
            }, OnlyOnRanToCompletion);

            try
            {
                t.Wait();
            }
            catch (AggregateException ex)
            {
                // System.Threading.Tasks.TaskCanceledException
                Console.WriteLine("t exception:{0}", ex.InnerException?.GetType());
            }

            Console.WriteLine("all finish");
        }
    }
}