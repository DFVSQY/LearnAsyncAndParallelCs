using System;
using System.Net;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("start, now:{0} processorID:{1}", DateTime.Now, Thread.GetCurrentProcessorId());

            Task<int> task = await Task.WhenAny(Delay1(), Delay2(), Delay3());
            int result = await task;

            Console.WriteLine("finish, result:{0} now:{1} processorID:{2}", result, DateTime.Now, Thread.GetCurrentProcessorId());
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