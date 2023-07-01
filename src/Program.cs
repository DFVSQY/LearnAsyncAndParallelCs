using System;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
            默认情况下，CLR会将任务运行在线程池线程上，这种线程非常适合执行短小的计算密集的任务。
            如果要执行长时间阻塞的操作（如上面的例子）则可以按照以下方式避免使用线程池线程。

            在线程池上运行一个长时间执行的任务并不会造成问题；
            但是如果要并行运行多个长时间运行的任务（特别是会造成阻塞的任务），则会对性能造成影响。
            */
            Task task = Task.Factory.StartNew(LongRun, TaskCreationOptions.LongRunning);

            Console.WriteLine(task.IsCompleted);

            task.Wait();

            Console.WriteLine(task.IsCompleted);

            // task运行的都是后台线程，当主线程运行完毕时后台线程也会随之结束，
            // 所以需要阻塞主线程，避免主线程关闭。
            Console.ReadLine();
        }

        static void LongRun()
        {
            Console.WriteLine("start run task");
            Console.WriteLine("finish run task");
        }
    }
}