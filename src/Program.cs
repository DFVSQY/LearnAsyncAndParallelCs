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
                int id = i;
                new Thread(Run).Start(id);
            }

            Thread.Sleep(1000);

            waitEvent.Set();
        }

        /*
        ManualResetEvent的作用就像是一个大门。
        调用Set方法就开启大门，并允许任意数目的调用WaitOne方法的线程通过大门。
        而调用Reset方法则会关闭大门。在大门关闭时调用WaitOne方法会发生阻塞。
        而当大门再次打开时，线程会立刻释放。除这些区别之外，ManualResetEvent的功能和AutoResetEvent是一样的。

        .NET 4.0引入了一种新的ManualResetEvent称为ManualResetEventSlim。
        后者对短时期的等待进行了优化，即选择进行几个迭代的自旋操作。
        此外，它还拥有更加高效的托管实现。
        并支持在Wait时使用Can-cellationToken取消等待操作。但是它不能进行跨进程的信号发送。
        ManualResetEventSlim并没有从WaitHandle派生。
        但是它拥有一个WaitHandle属性，访问该属性将返回一个（使用传统等待句柄性能配置的）WaitHandle派生类型的对象。

        ManualResetEvent适用于用一个线程来释放其他所有线程的情形，而CountdownEvent则适用于相反的情形。
        */
        static ManualResetEvent waitEvent = new ManualResetEvent(false);

        static void Run(object id)
        {
            Console.WriteLine("{0} waiting ...", id);
            waitEvent.WaitOne();
            Console.WriteLine("{0} notified", id);
        }
    }
}