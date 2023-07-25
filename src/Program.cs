using System;
using System.Diagnostics;
using System.Net;
using System.Timers;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
            PFX在Parallel类中提供了三个静态方法作为结构化并行的基本形式：
                1. Parallel.Invoke方法：并行执行一组委托。
                2. Parallel.For方法：执行与C# for循环等价的并行方法。
                3. Parallel.ForEach方法：执行与C#的foreach循环等价的并行方法。
            这三个方法都会阻塞线程直到所有工作完成为止。
            和PLINQ一样，在出现未处理异常之后，其他的工作线程将会在它们当前的迭代完成之后停止，并将异常包装为AggregateException抛出给调用者。

            和PLINQ一样，Parallel.*方法是针对计算密集型任务而不是I/O密集型任务进行优化的。
            */

            /*
            Parallel.Invoke方法并行执行一组Action委托，然后等待它们完成。

            从表面看来Parallel.Invoke就像是创建了两个绑定到线程的Task对象，然后等待它们执行结束的快捷操作。
            但是它们存在一个重要区别：如果将一百万个委托传递给Parallel.Invoke方法，它仍然能够有效工作。
            这是因为该方法会将大量的元素划分为若干批次，并将其分派给底层的Task，而不会单纯为每一个委托创建一个独立的Task。
            */
            Parallel.Invoke(
                () =>
                {
                    new WebClient().DownloadFile("http://www.baidu.com", "baidu.html");
                    Console.WriteLine("download from baidu");
                },
                () =>
                {
                    new WebClient().DownloadFile("http://www.sohu.com", "sohu.html");
                    Console.WriteLine("download from sohu");
                }
            );

            Console.WriteLine("main thread finish!");
        }
    }
}