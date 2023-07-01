using System;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Task.Delay(5000).GetAwaiter().OnCompleted(() =>
            {
                Console.WriteLine("task finish");
            });

            Console.WriteLine("waiting task ...");
            Console.ReadLine();
        }
    }
}