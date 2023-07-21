using System;
using System.Net;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main()
        {
            new Thread(Run).Start(false);
        }

        /*
        通常，ReaderWriterLockSlim禁止使用嵌套锁或者递归锁。这样的操作会抛出异常。

        通过在构造函数中指定LockRecursionPolicy.SupportsRecursion可以支持锁递归。

        递归锁的基本原则是，一旦获得了锁，后续的递归锁级别可以更低，但不能更高。
        其等级顺序如下：读锁→可升级锁→写锁
        但需要指出的是，将可升级锁提升为写锁的请求总是合法的。
        */
        static ReaderWriterLockSlim locker = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

        static List<int> items = new List<int>();

        static void Run(object read)
        {
            if (!(bool)read)
            {
                locker.EnterWriteLock();
                items.Add(5);
                Run(true);
                locker.ExitWriteLock();
            }
            else
            {
                locker.EnterReadLock();
                Console.WriteLine("items count:{0}", items.Count);
                locker.ExitReadLock();
            }
        }

    }
}