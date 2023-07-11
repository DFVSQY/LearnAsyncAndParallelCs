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
            Monitor还提供了TryEnter方法来指定一个超时时间（以毫秒为单位的整数或者一个TimeSpan值）。
            如果在指定时间内获得了锁，则该方法返回true，如果超时并且没有获得锁，该方法返回false。
            如果不给TryEnter方法提供任何参数，且当前无法获得锁，则该方法会立即超时。
            和Enter方法一样，TryEnter方法也在CLR 4.0中进行了重载，并在重载中接受lockTaken参数。
            */
            bool lockTaken = false;
            Monitor.TryEnter(locker, 1, ref lockTaken);
            if (lockTaken)
            {
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
                    Monitor.Exit(locker);
                }
            }
        }
    }
}