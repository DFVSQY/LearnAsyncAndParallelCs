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
            每一次只能有一个线程锁定同步对象（本例中的locker），而其他线程则被阻塞，直至锁释放。
            如果参与竞争的线程多于一个，则它们需要在准备队列中排队，并以先到先得的方式获得锁。
            排它锁会强制以所谓序列（serialized）的方式访问被锁保护的资源，因为线程之间的访问是不能重叠的。
            */
            lock (locker)
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
        }
    }
}