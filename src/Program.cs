using System;
using System.Net;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("start, now:{0} processorID:{1}", DateTime.Now, Thread.GetCurrentProcessorId());

            object locker1 = new object();
            object locker2 = new object();

            new Thread(() =>
            {
                lock (locker1)
                {
                    Console.WriteLine("start new thread");
                    Thread.Sleep(1000);
                    lock (locker2)
                    {                      // Deadlock
                        Console.WriteLine("new thread!");
                    }
                }
            }).Start();

            lock (locker2)
            {
                Console.WriteLine("start main thread");
                Thread.Sleep(1000);
                lock (locker1)              // Deadlock
                {
                    Console.WriteLine("main thread");
                }
            }

            Console.WriteLine("finish!");
        }

    }
}