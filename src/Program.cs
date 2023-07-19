using System;
using System.Net;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static object locker = new object();

        static void Main(string[] args)
        {
            Console.WriteLine("start, now:{0} processorID:{1}", DateTime.Now, Thread.GetCurrentProcessorId());

            for (int i = 0; i < 100; i++)
            {
                new Thread(Run1).Start();
            }
        }

        static int x = 5;

        /*
        线程可以用嵌套（重入）的方式重复锁住同一个对象。
        在使用嵌套锁时，只有最外层的lock语句退出时（或者执行相同数目的Monitor.Exit时）对象的锁才会解除。

        当一个锁中的方法调用另一个方法时，嵌套锁很奏效。
        该示例中，线程只会阻塞在第一个（最外层的）锁上。

        对这个简单的示例来说，Run2不进行锁定也是线程安全的，因为Run1已经进行了锁定。
        */
        static void Run1()
        {
            lock (locker)
            {
                Run2();
            }
        }

        static void Run2()
        {
            lock (locker)
            {
                x++;
                Console.WriteLine("Run2 result:{0} processorID:{1}", x, Thread.GetCurrentProcessorId());
            }
        }
    }
}