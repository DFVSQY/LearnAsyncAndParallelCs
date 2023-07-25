﻿using System;
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
            Parallel.For和Parallel.ForEach分别等价于C#中的for和foreach循环，但是每一次迭代都是并行而非顺序执行的。
            */

            List<int> list = new List<int>() { 2, 5, 3, 1, 6, 0, 9, 8, 7 };
            Parallel.For(0, list.Count, (i) =>
            {
                Console.WriteLine("for i:{0}", i);
            });
            Console.WriteLine("for finish");

            Parallel.ForEach(list, (item) =>
            {
                Console.WriteLine("foreach item:{0}", item);
            });
            Console.WriteLine("foreach finish");

            /*
            有些情况下，循环迭代中的索引用处很大。对于顺序执行的foreach循环中，为了线程安全，必须使用以下版本的ForEach语句。

            我们必须将结果整理到一个线程安全的集合中，这是与PLINQ相比的缺点。
            而这种方式胜过PLINQ之处在于索引化的ForEach比索引化的Select查询运算符的执行效率高。
            ConcurrentBag是一个线程安全的集合。
            */
            ConcurrentBag<int> bag = new ConcurrentBag<int>();
            Parallel.ForEach(list, (item, state, i) =>
            {
                bag.Add(item);
                Console.WriteLine("foreach i:{0}", i);
            });
            Console.WriteLine("foreach finish");

            Console.WriteLine("main thread finish!");
        }
    }
}