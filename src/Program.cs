using System;
using System.Net;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main()
        {
            /*
            Mutex和C#的lock类似，但是它可以支持多个进程。获得或者释放Mutex比lock要慢。

            Mutex类的WaitOne方法将获得该锁，ReleaseMutex方法将释放该锁。Mutex只能在获得锁的线程释放锁。
            */
            using (var mutex = new Mutex(true, "example.com OneAtATimeDemo"))
            {
                /*
                Mutex.WaitOne(TimeSpan timeout, bool exitContext)函数是一个实例方法，
                它可以封锁当前线程，直到当前的Mutex对象收到信号或超时为止，同时指定是否在等待之前退出同步域。
                */
                if (!mutex.WaitOne(TimeSpan.FromSeconds(3), false))
                {
                    Console.WriteLine("Another instance of the app is running. Bye! ");
                    return;
                }
                try
                {
                    RunProgram();
                }
                finally
                {
                    mutex.ReleaseMutex();
                }
            }
        }

        static void RunProgram()
        {
            Console.WriteLine("Running. Press Enter to exit");
            Console.ReadLine();
        }
    }
}