using System;
using System.Diagnostics;
using System.Net;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        class A
        {
            public A()
            {
                Console.WriteLine("A ctor call");
            }
        }

        /*
        从.NET Framework 4.0开始引入了Lazy<T>类，该类实现了延迟初始化（lazy initialization）功能。
        如果实例化时以true为参数，则它就会使用线程安全的初始化模式。
        
        Lazy<T>实际上还在锁上进行了微小的优化，称为“双检锁”（doublech-ecked lock）。
        双检锁执行一次volatile读操作，避免在对象初始化后进行锁操作。
        
        使用Lazy<T>时可以传入一个工厂委托方法来指明如何创建一个新的实例，同时传入第二个参数true，
        之后就可以使用Value属性访问实例的值了
        */
        static Lazy<A> lazy_a = new Lazy<A>(() =>
        {
            return new A();
        }, true);

        static void Main(string[] args)
        {
            for (int i = 0; i < 10; i++)
            {
                new Thread(Run).Start();
            }
        }

        private static void Run()
        {
            A a = lazy_a.Value;
        }
    }
}