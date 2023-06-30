using System;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static bool done;

        static void Main(string[] args)
        {
            Thread thread = new Thread(Run);
            thread.Start();

            Run();
        }

        private static object _locker = new object();
        static void Run()
        {
            lock (_locker)
            {
                if (!done)
                {
                    Console.WriteLine("run!");
                    done = true;
                }
                else
                {
                    Console.WriteLine("no run!");
                }
            }
        }
    }
}