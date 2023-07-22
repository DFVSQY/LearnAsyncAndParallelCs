using System;
using System.Diagnostics;
using System.Net;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        /*
        Barrier类实现了一个线程执行屏障（thread execution barrier）。
        它允许多个线程在同一时刻汇合。这个类的执行速度很快，非常高效。它是基于Wait、Pulse和自旋锁实现的。

        使用这个类的步骤如下：
            1. 创建Barrier实例。指定参与汇合的线程的数量（此后还可以调用AddPartici-pants/RemoveParticipants方法对这个数量进行更改）。
            2. 当需要汇合时，在每一个线程上都调用SignalAndWait。

        创建Barrier对象时还可以指定一个后续操作（post-phase），这个功能非常有用。
        该操作是一个委托，它会在SignalAndWait调用n次之后，所有线程释放之前执行

        后续操作适用于从各个工作线程获得数据。它不需要担心抢占的问题，因为在它执行过程中所有的工作线程都会被阻塞。
        */
        static Barrier barrier = new Barrier(3, (br) =>
        {
            Console.WriteLine();
        });

        static void Main(string[] args)
        {
            for (int i = 0; i < 3; i++)
            {
                new Thread(Run).Start();
            }
        }

        private static void Run()
        {
            for (int i = 0; i < 5; i++)
            {
                Console.Write("{0} ", i);
                barrier.SignalAndWait();
            }
        }
    }
}