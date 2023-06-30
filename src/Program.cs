using System;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Thread thread = new Thread(() => { Print("hello world"); });
            thread.Start();
        }

        static void Print(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}