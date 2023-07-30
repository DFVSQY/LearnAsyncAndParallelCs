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
            延续任务可以查询前导任务的Exception属性来确认前导任务是否已经失败。
            当然，也可以调用Result/Wait并捕获AggregateException。
            如果前导任务失败，则延续任务既不确认，也不获得前导任务的结果，则前导任务的异常就成为未观测异常。
            之后，当垃圾回收器回收前导任务时就会触发TaskScheduler.Unobserved-TaskException事件。
            重新抛出前导任务中的异常是一种安全的处理方式。只要有程序调用延续任务的Wait方法，则该异常就会继续传播，并在Wait方法中重新抛出。

            重新抛出前导任务中的异常是一种安全的处理方式。
            只要有程序调用延续任务的Wait方法，则该异常就会继续传播，并在Wait方法中重新抛出。

            另一种处理异常的方式是为异常和正常的结果指定不同的延续任务。
            指定TaskContinuationOptions就可以做到这一点。
            */
            Task task = Task.Factory.StartNew(() =>
            {
                throw null;
            });

            Task error = task.ContinueWith((t1) =>
            {
                Console.WriteLine("exp msg:{0}", t1.Exception?.Message);
            }, TaskContinuationOptions.OnlyOnFaulted);

            Task ok = task.ContinueWith((t1) =>
            {
                Console.WriteLine("task finish");
            }, TaskContinuationOptions.NotOnFaulted);

            try
            {
                ok.Wait();
                error.Wait();
            }
            catch (Exception ex)
            {
                Exception? e = ex.InnerException;
                while (e != null && e is AggregateException)
                {
                    e = e.InnerException;
                }

                if (e != null)
                {
                    // 任务取消异常，System.Threading.Tasks.TaskCanceledException
                    Console.WriteLine("inner ex:{0}", e.GetType());
                }
            }

            Console.WriteLine("all finish");
        }
    }
}