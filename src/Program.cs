using System;
using System.Diagnostics;
using System.Net;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        /*
        对于多线程并发，有时需要将数据隔离，保证每一个线程都有一个独立的副本。
        局部变量就可以实现这个目标，但它们仅适合保存临时数据。
        另一个方案是使用线程本地存储（thread-local storage）。
        实际上，隔离到每一个线程上的数据本质上就是临时数据。
        这可能有些难以想象。但是其主要用途就是存储“过程外”数据，并作为执行路径的基础设施，例如消息、事务、安全令牌等。
        如果将这些数据以方法参数的形式进行传递则代码就会非常难看，因为几乎每一个方法都需要接受它们。
        而如果将这种数据存储在静态字段中那么它又可以被所有的线程共享而失去独立性。
        
        线程本地存储还可用于对并行代码进行优化。
        它允许每一个线程无须使用锁就可以独立访问属于该线程的（非线程安全）对象。
        同时它无须在（同一线程的）方法调用过程中重建这个对象。
        但是，线程本地存储并不适合在异步代码中使用，因为有一些延续可能会运行在之前的线程上。
        */


        /*
        ThreadLocal<T>是.NET Framework 4.0中新增的类型。它对静态和实例字段都提供了线程本地存储支持，并允许指定默认值。

        此后就可以调用字段的Value属性来访问线程本地值了。
        ThreadLocal的值是延迟计算的：其中的工厂函数会在（每一个线程）第一次调用时计算实际的值。
        */
        static ThreadLocal<int> field = new ThreadLocal<int>(() => 5);

        static void Main(string[] args)
        {
            for (int i = 0; i < 3; i++)
            {
                new Thread(Run).Start();
            }
        }

        private static void Run()
        {
            for (int i = 1; i <= 10; i++)
            {
                field.Value += i;
            }
            Console.WriteLine("Run Result:{0}", field.Value);
        }
    }
}