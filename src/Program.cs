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
            TaskCreationOptions options = TaskCreationOptions.AttachedToParent;

            Task task = Task.Factory.StartNew(() =>
            {
                Task.Factory.StartNew(() =>
                {
                    Console.WriteLine("child task1");
                    Thread.Sleep(1000);
                    throw null;
                }, options);

                Task.Factory.StartNew(() =>
                {
                    Console.WriteLine("child task2");
                    Thread.Sleep(3000);
                    throw null;
                }, options);

                Task.Factory.StartNew(() =>
                {
                    Console.WriteLine("child task3");
                    Thread.Sleep(5000);
                    throw null;
                }, options);
            });

            /*
            延续任务有一个非常重要的特性：它在所有的子任务完成之后才会开始执行。
            这时，子任务抛出的所有异常都会封送到延续任务中。
            */
            Task error = task.ContinueWith((t) =>
            {
                if (t.Exception is AggregateException)
                {
                    AggregateException aggregate = t.Exception as AggregateException;
                    foreach (Exception ex in aggregate.InnerExceptions)
                    {
                        Console.WriteLine("type:{0} msg:{1}", ex.InnerException?.GetType(), ex.InnerException?.Message);
                    }
                }
            }, TaskContinuationOptions.OnlyOnFaulted);

            error.Wait();

            Console.WriteLine("all finish");
        }
    }
}