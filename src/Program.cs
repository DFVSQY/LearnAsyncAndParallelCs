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
                Task<int> task = Task.Run<int>(async () =>
                {
                    await Task.Delay(5000);
                    return 5;
                });

                int result = await task.WithTimeout<int>(1000);

                Console.WriteLine("result:{0} now:{1} processorID:{2}", result, DateTime.Now, Thread.GetCurrentProcessorId());
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
    }
}