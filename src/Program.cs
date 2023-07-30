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
            在启动任务时，我们可以传入一个取消令牌。若通过该令牌执行取消操作，则任务本身就会进入“已取消”状态。
            */
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;
            cancellationTokenSource.CancelAfter(1000);

            Task task = Task.Factory.StartNew(() =>
            {
                Thread.Sleep(1500);
                token.ThrowIfCancellationRequested();
            }, token);

            try
            {
                task.Wait();
            }
            catch (AggregateException ex)
            {
                Console.WriteLine(ex.InnerException is TaskCanceledException);

                Console.WriteLine(task.IsCanceled);

                Console.WriteLine(task.Status);
            }

            /*
            TaskCanceledException是OperationCanceledException的子类。
            如果希望显式抛出一个OperationCanceledException（而不是调用token.ThrowIfCancellation-Requested方法），
            则必须用取消令牌作为OperationCanceledException的构造器的参数。
            如果不这样做，那么任务就不会进入TaskStatus.Canceled状态，也不会触发标记为OnlyOnCanceled的延续任务。
            
            如果一个任务还没有开始就取消了，则它就不会被调度。并且该任务会立即抛出OperationCanceledException。

            我们还可以将取消令牌作为其他支持取消令牌的API的参数，这样，就可以将取消操作无缝地传播出去。

            Wait和CancelAndWait方法中的取消令牌参数用于取消等待操作，而不是取消任务本身。
            */

            Console.WriteLine("all finish");
        }

        static void Run(object? msg)
        {
            Console.WriteLine(msg);
        }
    }
}