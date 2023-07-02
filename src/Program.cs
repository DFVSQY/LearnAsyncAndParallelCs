using System;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("main thread processorID:{0}", Thread.GetCurrentProcessorId());

            DisplayPrimeCounts();

            Console.WriteLine("waiting task, processorID:{0}", Thread.GetCurrentProcessorId());

            Console.ReadLine();
        }

        /*
        在第一次执行GetPrimesCountAsync方法时，由于出现了await表达式，因此执行点返回给调用者。
        当方法完成（或者出错）时，执行点会从停止之处恢复执行，同时保留本地变量和循环计数器的值。
        */
        static async void DisplayPrimeCounts()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("run idx:{0} processorID:{1} time:{2}", i, Thread.GetCurrentProcessorId(), DateTime.Now.Ticks / 1000);

                int result = await GetPrimesCountAsync(i * 1000000 + 2, 1000000);
                Console.WriteLine("result:{0} processorID:{1}", result, Thread.GetCurrentProcessorId());
            }

            Console.WriteLine("skip DisplayPrimeCounts, processorID:{0}", Thread.GetCurrentProcessorId());
        }

        static Task<int> GetPrimesCountAsync(int start, int count)
        {
            return Task.Run(() =>
            {
                Console.WriteLine("start runinng, processorID:{0}", Thread.GetCurrentProcessorId());
                int cnt = ParallelEnumerable.Range(start, count).Count(n =>
                Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0));
                Console.WriteLine("finish running, processorID:{0}", Thread.GetCurrentProcessorId());
                return cnt;
            });
        }
    }
}