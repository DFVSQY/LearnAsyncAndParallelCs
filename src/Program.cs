using System;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("main thread processorID:{0}", Thread.GetCurrentProcessorId());

            /*
            以下两句代码等效于：
            var awaiter = GetPrimesCountAsync(2, 1000000).GetAwaiter();
            awaiter.OnCompleted(() =>
            {
                int result = awaiter.GetResult();
                Console.WriteLine("result:{0} processorID:{1}", result, Thread.GetCurrentProcessorId());
            });
            */
            int result = await GetPrimesCountAsync(2, 1000000);

            // 以下的代码不一定在主线程执行了
            Console.WriteLine("result:{0} processorID:{1}", result, Thread.GetCurrentProcessorId());
        }

        static Task<int> GetPrimesCountAsync(int start, int count)
        {
            return Task.Run(() =>
            {
                Console.WriteLine("start runing ...");
                int cnt = ParallelEnumerable.Range(start, count).Count(n =>
                Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0));
                Console.WriteLine("finish running, processorID:{0}", Thread.GetCurrentProcessorId());
                return cnt;
            });
        }
    }
}