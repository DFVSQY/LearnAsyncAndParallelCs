using System;
using System.Net;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main()
        {
            new Thread(Run).Start();

            waitReady.WaitOne();
            lock (locker) msg = "001";
            waitGo.Set();

            waitReady.WaitOne();
            lock (locker) msg = "002";
            waitGo.Set();

            waitReady.WaitOne();
            lock (locker) msg = null;
            waitGo.Set();
        }

        /*
        假设主线程需要向工作线程连续发送三次信号。
        如果主线程单纯地连续调用Set方法若干次，那么第二次或者第三次发送的信号就有可能丢失，因为工作线程需要时间来处理每一次的信号。
        其解决方案是主线程等待工作线程准备就绪之后再发送信号。这可以通过引入另一个AutoResetEvent来实现。
        */
        static AutoResetEvent waitReady = new AutoResetEvent(false);
        static AutoResetEvent waitGo = new AutoResetEvent(false);

        static object locker = new object();

        static string? msg;

        static void Run()
        {
            while (true)
            {
                waitReady.Set();
                waitGo.WaitOne();

                lock (locker)
                {
                    if (msg == null) return;
                    Console.WriteLine(msg);
                }
            }
        }
    }
}