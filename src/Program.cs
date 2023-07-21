using System;
using System.Net;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main()
        {
            for (int i = 1; i <= 5; i++)
            {
                int idx = i;
                new Thread(Run).Start(idx);
            }
        }

        /*
        信号量（semaphore）就像俱乐部一样：它有特定的容量，还有门卫保护。
        一旦满员之后，就不允许其他人进入了，人们只能在外面排队。每当有人离开时，才准许另外一个人进入。

        容量为1的信号量和Mutex和lock类似，但是信号量没有持有者这个概念，它是线程无关的。
        任何线程都可以调用Semaphore的Release方法。Mutex和lock则不然，只有持有锁的线程才能够释放锁。

        信号量有两个功能相似的实现：Semaphore和SemaphoreSlim。
        后者是在.NET Framework 4.0引入的。它进行了一些优化以适应并行编程对低延迟的需求。
        此外，它也适用于传统的多线程编程，因为它可以在等待时指定一个取消令牌。
        它还提供了WaitAsync方法以进行异步编程，但是它不能用于进程间通信。
        
        Semaphore在调用WaitOne和Release方法时大概会消耗1微秒的时间，而Sem-aphoreSlim的开销只有前者的十分之一。
        
        信号量可用于限制并发性，防止太多的线程同时执行特定的代码。
        */
        static SemaphoreSlim sem = new SemaphoreSlim(3);
        static void Run(object i)
        {
            Console.WriteLine("{0} want to enter", i);

            sem.Wait();
            Console.WriteLine("{0} is in", i);

            Thread.Sleep(1000 * (int)i);

            Console.WriteLine("{0} is leaving", i);
            sem.Release();
        }
    }
}