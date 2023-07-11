using System;
using System.Net;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("start, now:{0} processorID:{1}", DateTime.Now, Thread.GetCurrentProcessorId());

            for (int i = 0; i < 5; i++)
            {
                Thread thread = new Thread(Go);
                thread.Start();
            }

            Console.WriteLine("main thread finish, processorID:{0}", Thread.GetCurrentProcessorId());
        }

        static readonly object locker = new object();

        static int var1 = 1, var2 = 1;

        /// <summary>
        /// 线程安全的方法
        /// </summary>
        static void Go()
        {
            /*
            事实上，C#的lock语句是包裹在try/finally语句块中的Monitor.Enter和Monitor.Exit语法糖。

            如果在Monitor.Enter和try语句块之间抛出了异常（例如，调用该线程的Abort方法，或者抛出了OutOfMemoryException）那么锁的状态是不确定的。
            但若已经获得了锁，那么这个锁就永远无法释放，因为我们已经没有机会进入try/finally代码块了。因此这种情况会造成锁泄露。

            为了防范这种风险，CLR 4在设计时对Monitor.Enter进行了重载，增加了lockTaken参数。

            Enter方法执行结束后，当且仅当该方法执行时抛出了异常且没有获得锁时，lockTaken为false。
            因而从C# 4.0开始，lock语句将会翻译为以下模式（该模式较之前更加健壮）。
            */

            bool lockTaken = false;
            Monitor.Enter(locker, ref lockTaken);
            try
            {
                if (var2 != 0)
                {
                    Console.WriteLine("result:{0} processorID:{1}", var1 / var2, Thread.GetCurrentProcessorId());
                }
                else
                {
                    Console.WriteLine("var2 is zero, processorID:{0}", Thread.GetCurrentProcessorId());
                }

                var2 = 0;
            }
            finally
            {
                if (lockTaken)
                {
                    Monitor.Exit(locker);
                }
            }
        }
    }
}