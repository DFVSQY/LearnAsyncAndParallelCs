using System;
using System.Net;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("start, now:{0} processorID:{1}", DateTime.Now, Thread.GetCurrentProcessorId());

            /*
            Task.WhenAll只在所有的任务完成之后才会完成，即使中间出现了错误也一样。
            如果多个任务发生了错误，那么这些异常会组合到任务的AggregateException中
            （这也是AggregateException真正发挥作用的时候，你可以从中得到所有的异常）。
            */
            try
            {
                int[] results = await Task.WhenAll(Delay1(), Delay2(), Delay3());
                Console.WriteLine("finish, result's num:{0} now:{1} processorID:{2}", results.Length, DateTime.Now, Thread.GetCurrentProcessorId());
            }
            catch (AggregateException e)
            {
                Console.WriteLine("exception num:{0}", e.InnerExceptions.Count);
            }
        }

        static async Task<int> Delay1()
        {
            await Task.Delay(1000);
            return 1;
        }

        static async Task<int> Delay2()
        {
            await Task.Delay(2000);
            return 2;
        }

        static async Task<int> Delay3()
        {
            await Task.Delay(3000);
            return 3;
        }
    }
}