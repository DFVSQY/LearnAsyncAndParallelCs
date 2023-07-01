using System;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Delay(5000).GetAwaiter().OnCompleted(() =>
            {
                Console.WriteLine("task finish");
            });

            Console.WriteLine("waiting task ...");
            Console.ReadLine();
        }

        /*
        TaskCompletionSource的真正作用是创建一个不绑定线程的任务。
        可以利用TaskCompletionSource实现一个非堵塞的Delay方法
        */
        static Task Delay(int milliseconds)
        {
            var tcs = new TaskCompletionSource<object>();
            var timer = new System.Timers.Timer(milliseconds) { AutoReset = false };
            timer.Elapsed += delegate { timer.Dispose(); tcs.SetResult(null); };
            timer.Start();
            return tcs.Task;
        }
    }
}