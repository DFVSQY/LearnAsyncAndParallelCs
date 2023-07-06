using System;
using System.Net;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("main thread processorID:{0}", Thread.GetCurrentProcessorId());

            /*
            Progress<T>类的构造器可以接受一个Action<T>委托并对其进行包装。
            (Progress<T>还有一个ProgressChanged事件。我们可以订阅这个事件而无须在构造函数中传入一个委托)
            在Progress<int>实例化时，如果拥有同步上下文，该类就会捕获同步上下文。
            当Run调用Report方法时，该对象就会使用同步上下文调用委托。
            */
            Progress<int> progress = new Progress<int>((value) =>
            {
                Console.WriteLine("prg:{0}% processorID:{1}", value, Thread.GetCurrentProcessorId());
            });

            await Run(progress);

            Console.WriteLine("processorID:{0}", Thread.GetCurrentProcessorId());

            Console.ReadLine();
        }

        static Task Run(IProgress<int> progress)
        {
            return Task.Run(() =>
            {
                Console.WriteLine("start run, processorID:{0}", Thread.GetCurrentProcessorId());
                for (int i = 1; i <= 1000; i++)
                {
                    if (i % 10 == 0)
                    {
                        int prg = i / 10;
                        Console.WriteLine("value:{0} processorID:{1}", prg, Thread.GetCurrentProcessorId());
                        progress.Report(prg);
                    }
                }
                Console.WriteLine("finish run, processorID:{0}", Thread.GetCurrentProcessorId());
            });
        }
    }
}