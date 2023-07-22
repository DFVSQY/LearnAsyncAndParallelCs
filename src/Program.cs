using System;
using System.Diagnostics;
using System.Net;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static ManualResetEvent waitHandle = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            /*
            如果不希望等待一个句柄从而阻塞线程，还可以调用ThreadPool.RegisterWaitForSingleObject方法来将一个延续操作附加在等待句柄上。

            当等待句柄接到信号时（或者超时后），委托就会在一个线程池线程中执行。之后，还需要调用Unregister解除非托管的句柄和回调之间的关系。
            */
            RegisteredWaitHandle reg = ThreadPool.RegisterWaitForSingleObject(waitHandle, Run, "Some Data", -1, true);

            Thread.Sleep(5000);

            Console.WriteLine("start signal...");
            waitHandle.Set();

            Console.ReadLine();
            reg.Unregister(waitHandle);
        }

        static void Run(object data, bool timeOut)
        {
            Console.WriteLine("start run:{0}", data);
        }
    }
}