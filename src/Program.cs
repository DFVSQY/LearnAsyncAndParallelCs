using System;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Task task = Task.Run(Run);

            Console.WriteLine(task.IsCompleted);

            // 阻塞当前主线程，直到任务执行完毕
            task.Wait();

            Console.WriteLine(task.IsCompleted);

            // task运行的都是后台线程，当主线程运行完毕时后台线程也会随之结束，
            // 所以需要阻塞主线程，避免主线程关闭。
            Console.ReadLine();
        }

        static void Run()
        {
            Console.WriteLine("start run task");
            Console.WriteLine("finish run task");
        }
    }
}