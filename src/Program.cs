using System;
using System.Net;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("start, now:{0} processorID:{1}", DateTime.Now, Thread.GetCurrentProcessorId());

            /*
            Lambda表达式或匿名方法中捕获的局部变量也可以作为同步对象进行锁定。
            */
            object obj = new object();
            Action action = () =>
            {
                lock (obj)
                {
                    // do something ...
                }
            };
        }
    }

    internal class ThreadSafe
    {
        object locker = new object();

        List<int> list = new List<int>();

        /*
        若一个对象在各个参与线程中都是可见的，那么该对象就可以作为同步对象。但是该对象必须是一个引用类型的对象（这是必须满足的条件）。
        同步对象通常是私有的（因为这样便于封装锁逻辑），而且一般是实例字段或者静态字段。

        锁本身不会限制同步对象的访问功能。即x.ToString()不会因为其他线程调用了lock(x)而被阻塞。
        只有两个线程均执行lock(x)语句才会发生阻塞。
        */
        void SafeRun1()
        {
            // 引用类型的普通同步对象
            lock (locker)
            {
                // do something ...
            }
        }

        /*
        同步对象本身也可以是被保护的对象，例如：list
        */
        void SafeRun2()
        {
            // 被保护对象是同步对象
            lock (list)
            {
                list.Add(5);

                // do something ...
            }
        }

        // 容器的对象（this）也可以做为同步对象
        void SafeRun3()
        {
            lock (this)
            {
                // do something ...
            }
        }

        /*
        对象的类型也可以用作同步对象，该锁定方式有一个缺点，即无法封装锁逻辑，因此难以避免死锁或者长时间阻塞。
        而类型上的锁甚至可以跨越（同一进程中的）应用程序域的边界。
        */
        void SafeRun4()
        {
            lock (typeof(int))
            {
                // do something ...
            }
        }
    }
}