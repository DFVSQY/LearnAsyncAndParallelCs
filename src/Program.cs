using System;
using System.Net;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("start, now:{0} processorID:{1}", DateTime.Now, Thread.GetCurrentProcessorId());

            try
            {
                Task<int> task1 = Task.Run<int>(async () =>
                {
                    await Task.Delay(5000);
                    return 5;
                });

                Task<int> task2 = Task.Run<int>(async () =>
                {
                    await Task.Delay(1000);
                    throw new Exception("throw exception!");
                });

                int[] results = await TaskExt.WhenAllOrError<int>(task1, task2);

                Console.WriteLine("result's num:{0} now:{1} processorID:{2}", results.Length, DateTime.Now, Thread.GetCurrentProcessorId());
            }
            catch (AggregateException e)
            {
                Console.WriteLine("start print all exceptions");
                foreach (var ie in e.InnerExceptions)
                {
                    Console.WriteLine(ie.Message);
                }
                Console.WriteLine("finish print all exceptions");
            }
            catch (Exception e)
            {
                Console.WriteLine("exception msg:{0}, type:{1}", e.Message, e.GetType());
            }
        }
    }

    internal static class TaskExt
    {
        public async static Task<TResult> WithTimeout<TResult>(this Task<TResult> task, int timeout)
        {
            Task winner = await (Task.WhenAny(task, Task.Delay(timeout)));
            if (winner != task) throw new TimeoutException();
            return await task;    // Unwrap result/re-throw
        }

        public static Task<TResult> WithCancellation<TResult>(this Task<TResult> task, CancellationToken cancelToken)
        {
            var tcs = new TaskCompletionSource<TResult>();
            var reg = cancelToken.Register(() => tcs.TrySetCanceled());
            task.ContinueWith(ant =>
            {
                reg.Dispose();
                if (ant.IsCanceled)
                    tcs.TrySetCanceled();
                else if (ant.IsFaulted)
                    tcs.TrySetException(ant.Exception.InnerException);
                else
                    tcs.TrySetResult(ant.Result);
            });
            return tcs.Task;
        }

        /*
        以下的组合器作用与WhenAll类似，不同点在于只要有一个任务出现错误，那么最终任务也会立即出错。
        */
        public static async Task<TResult[]> WhenAllOrError<TResult>(params Task<TResult>[] tasks)
        {
            var killJoy = new TaskCompletionSource<TResult[]>();
            foreach (var task in tasks)
            {
                task.ContinueWith(ant =>
                {
                    if (ant.IsCanceled)
                        killJoy.TrySetCanceled();
                    else if (ant.IsFaulted)
                        killJoy.TrySetException(ant.Exception.InnerException);
                });
            }
            return await await Task.WhenAny(killJoy.Task, Task.WhenAll(tasks));
        }
    }
}