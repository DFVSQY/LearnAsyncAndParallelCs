using System;
using System.Net;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main()
        {
            for (int i = 1; i <= 5; i++)
            {
                int id = i;
                new Thread(Run).Start(id);
            }

            waitEvent.Wait();

            Console.WriteLine("main thread finished!");
        }

        /*
        CountdownEvent可用于等待多个线程。该类是在.NET Framework 4.0引入的，并同样具有高效的纯托管实现。
        若使用该类，需要在实例化时指定需要等待的线程“计数”。

        调用Signal会使计数递减；而调用Wait则会阻塞，直至计数减为零。

        调用AddCount方法可以重新增加CountdownEvent的计数。
        但是如果它的计数已经降为零，则调用该方法会抛出异常：我们无法通过调用AddCount来取消Count-downEvent的信号。
        为了避免抛出异常，还可以使用TryAddCount。
        若计数值为0，则该方法会返回false。
        调用Reset方法可以取消计数事件的信号：它不但取消信号，而且会将计数值重置为原始设定值。
        */
        static CountdownEvent waitEvent = new CountdownEvent(5);

        static void Run(object id)
        {
            Console.WriteLine("{0} start sleeping ...", id);
            Thread.Sleep((int)id * 1000);
            Console.WriteLine("{0} finish sleeping ...", id);
            waitEvent.Signal();
        }
    }
}