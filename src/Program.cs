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

        static bool _isInitialized = false;
        static object _lock = new object();

        static A _a1;
        static A a1
        {
            get
            {
                /*
                LazyInitializer是一个静态类。
                它和Lazy<T>工作方式很像，但是也有以下不同点：
                    1. 它直接使用静态方法操作自定义类型的字段。这样做可以避免引入一个间接层次，从而提高性能。它适用于一些需要极致优化的场合。
                    2. 它提供了另一种初始化模式，多个线程可以竞争实例化过程。
                    
                在访问字段之前，调用LazyInitializer的EnsureInitialized方法，并传入字段的引用和工厂委托即可。
                */
                LazyInitializer.EnsureInitialized(ref _a1, ref _isInitialized, ref _lock, () =>
                {
                    return new A();
                });
                return _a1;
            }
        }

        static void Main(string[] args)
        {
            for (int i = 0; i < 10; i++)
            {
                new Thread(Run).Start();
            }
        }

        private static void Run()
        {
            A a = a1;
        }
    }
}