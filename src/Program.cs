using System;
using System.Diagnostics;
using System.Net;
using System.Threading;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("start main thread:{0}", Thread.CurrentThread.ManagedThreadId);

            /*
            System.Threading.Timer是最简单的多线程定时器：它只有一个构造器和两个方法。
            在创建定时器之后仍然可以调用Change方法修改定时器的定时间隔。
            */
            using (Timer timer = new Timer(Run, null, 5000, 1000))
            {
                Console.ReadLine();
            }
            
            Console.WriteLine("finish main thread:{0}", Thread.CurrentThread.ManagedThreadId);
        }

        private static void Run(object? state)
        {
            Console.WriteLine("tick ..., thread:{0}", Thread.CurrentThread.ManagedThreadId);
        }
    }
}