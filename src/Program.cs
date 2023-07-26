using System;
using System.Collections.Concurrent;
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
            Parallel.For()/Parallel.ForEach()循环也支持“breaking”（中断）循环并取消任何进一步迭代的操作。
            然而，由于涉及并行执行，因此这里的“中断”表示在中断迭代之后不应开始新的迭代，但是所有当前正在执行的迭代都将继续运行完成。

            当希望在循环体内部中断循环时可以调用ParallelLoopState对象的Break()或Stop()方法。
            Break()方法指示不再需要执行索引值高于当前值的迭代；Stop()方法表明根本不需要运行更多的迭代。

            例如，假设有一个Parallel.For()循环将执行10次迭代。
            其中一些迭代可能比其他迭代运行得更快，并且任务调度程序不保证它们会以任何特定顺序运行。
            假设迭代1已经完成；迭代3、5、7和9正在“进行中”并被安排到四个不同的线程；迭代5和7都调用Break()。
            在这种情况下，迭代6和8就永远不会开始了，但是迭代2和4仍会被调度执行。
            迭代3和9仍将正常运行完成，因为它们在中断发生时已经开始了。

            具体详情可参考《C#本质论》。
            */
            List<int> list = new List<int>() { 2, 5, 3, 1, 6, 0, 9, 8, 7 };
            Parallel.ForEach(list, (item, state, i) =>
            {
                if (item == 0)
                    state.Break();
                else
                    Console.Write(i);
            });
            Console.WriteLine();

            Parallel.ForEach(list, (item, state, i) =>
            {
                if (item == 0)
                    state.Stop();
                else
                    Console.Write(i);
            });
            Console.WriteLine();
        }
    }
}