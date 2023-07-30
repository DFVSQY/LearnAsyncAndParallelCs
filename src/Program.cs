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
            延续任务和普通任务一样，其类型也可以是Task<TResult>类型并返回数据。
            在下面的示例中，我们将使用一串任务来计算Math.Sqrt(8 * 2)，并输出结果。
            */
            Task<double> task = Task.Factory.StartNew(() =>
            {
                return 8;
            }).ContinueWith((task1) =>
            {
                return task1.Result * 2;
            }).ContinueWith((task2) =>
            {
                return Math.Sqrt(task2.Result);
            });

            task.Wait();
            Console.WriteLine("Result:{0}", task.Result);

            Console.WriteLine("all finish");
        }
    }
}