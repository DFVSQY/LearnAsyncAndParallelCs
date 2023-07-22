using System;
using System.Net;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static EventWaitHandle? waitHandle;

        static void Main(string[] args)
        {
            try
            {
                waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset, "com.example.myapp");
                bool wait = false;
                if (args != null && args.Length >= 1)
                {
                    bool.TryParse(args[0], out wait);
                }

                if (wait)
                    waitHandle.WaitOne();
                else
                    waitHandle.Set();
            }
            catch (Exception exception)
            {
                Console.WriteLine("exception:{0}", exception.Message);
            }
        }
    }
}