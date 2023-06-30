using System;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        private static ManualResetEvent signal = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            Thread thread = new Thread(Run);
            thread.Start();

            Thread.Sleep(2000);
            signal.Set();
        }

        static void Run()
        {
            Console.WriteLine("start wait one signal");
            signal.WaitOne();
            signal.Dispose();
            Console.WriteLine("finish one signal");
        }
    }
}