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

            for (int i = 1; i <= 5; i++)
            {
                Thread.Sleep(1000);
                waitHandle.Set();
            }
        }

        /*
        AutoResetEvent就像验票机的闸门一样：插入一张票据只允许一个人通过。
        其名称中的Auto指的是开放的闸机在行人通过后会自动关闭或重置。
        线程可以调用WaitOne方法在闸机门口等待、阻塞（在“一个”闸机前等待，直至闸机门开启）。
        调用Set方法即向闸机中插入一张票据。如果有一系列的线程调用了WaitOne，那么它们会在闸机后排队等待。
        票据可以来自任何线程，即任何一个能够访问AutoResetEvent对象的非阻塞线程都可以调用Set方法来释放一个阻塞的线程。

        创建AutoResetEvent的方法有两种。
        第一种是使用其构造器：
        var wait = new AutoResetEvent(false)
        （如果在构造器中以true为参数，则相当于立刻调用Set方法）。
        
        第二种方法则是使用如下方式创建AutoResetEvent：
        var wait = new EventWaitHandle(false, EventResetMode.AutoReset);

        在没有任何线程等待的情况下调用Set方法会导致句柄一直处于打开状态，直至有线程调用了WaitOne方法。
        这种行为可以避免即将到达闸机前的线程和插入票据的线程产生竞争（如果票据插入的时间早了一微秒，那现在只能一直等待了）。
        但是在一个没有线程等待的闸机对象上重复调用Set方法不会导致多个到达的线程一次性通过；
        只有下一个线程可以通过，而其他的票据就被“浪费”了。

        在AutoResetEvent对象上调用Reset方法可以无须等待或阻塞就关闭闸机的门（若原本处于开启状态的话）。
        而WaitOne可以接受一个可选的超时参数。如果在超时时间内没有收到信号，则返回false。

        可以使用0作为超时时间调用WaitOne来确认一个等待句柄是否处于“开放”状态，并且不会造成调用者阻塞。
        但需要注意的是，如果AutoReset-Event处于开放状态的话上述操作会将状态重置。
        */
        static EventWaitHandle waitHandle = new AutoResetEvent(false);


        static void Run(object id)
        {
            Console.WriteLine("{0} waiting ...", id);

            waitHandle.WaitOne();

            Console.WriteLine("{0} notified", id);
        }
    }
}