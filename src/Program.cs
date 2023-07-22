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

        class Test
        {
            /*
            第三种实现线程本地存储的方式是使用Thread类的GetData和SetData方法。
            这些方法会将数据存储在线程独有的“插槽”（slot）中。
            Thread.GetData负责读取线程独有的数据存储中读取数据，而Thread.SetData则向其中写入数据。
            这两个方法都需要使用LocalDataStoreSlot对象来获得这个插槽。
            所有的线程都可以获得相同的插槽，但是它们的值却是互相独立的。

            可以调用Thread.GetNamedDataSlot来获得一个命名插槽，这样我们就可以在整个应用程序中共享这个命名插槽了。
            此外，还可以调用Thread.Allocate-DataSlot来创建一个匿名插槽，这样就可以自由控制插槽的使用范围。

            Thread.FreeNamedDataSlot方法将释放所有线程中的命名插槽。
            但需要注意的是，只有当LocalDataStoreSlot对象的所有引用都已经在作用域之外并被垃圾回收时插槽才会释放。
            这确保了当线程需要特定数据插槽时，只要它保留了正确的LocalDataStoreSlot对象的引用，那么相应的数据插槽就不会丢失。
            */
            private LocalDataStoreSlot levelSlot = Thread.GetNamedDataSlot("test_cls_level_slot");

            public int level
            {
                get
                {
                    object obj = Thread.GetData(levelSlot);
                    return obj == null ? 0 : (int)obj;
                }
                set
                {
                    Thread.SetData(levelSlot, value);
                }
            }
        }

        static Test test = new Test();

        static void Main(string[] args)
        {
            for (int i = 0; i < 3; i++)
            {
                new Thread(Run).Start();
            }
        }

        private static void Run()
        {
            test.level += 5;
            Console.WriteLine("Run Result:{0}", test.level);
        }
    }
}