using System;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TaskCompletionSource<int> tcs = new TaskCompletionSource<int>();

            new Thread(() =>
            {
                Thread.Sleep(5000);
                tcs.SetResult(42);
            })
            {
                IsBackground = true,
            }.Start();

            Task<int> task = tcs.Task;
            Console.WriteLine(task.Result);
        }
    }
}