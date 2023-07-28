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
            在调用StartNew方法（或在实例化Task对象）时，可以指定一个TaskCreationOptions枚举值来调整任务的执行方式。
            TaskCreationOptions是一个标志枚举类型。

            LongRunning字段会通知调度器为任务指定一个线程。这种方式非常适合I/O密集型任务和长时间执行的任务。
            如果不这样做，那么那些执行时间很短的任务反而可能需要等待很长的时间才能被调度。
            */
            Task task = Task.Factory.StartNew(Run, "Hello Task", TaskCreationOptions.LongRunning);
            task.Wait();

            Console.WriteLine("all finish");
        }

        static void Run(object? msg)
        {
            Console.WriteLine(msg);
        }
    }
}