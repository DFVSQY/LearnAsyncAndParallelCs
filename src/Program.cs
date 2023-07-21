using System;
using System.Net;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main()
        {
            for (int i = 0; i < 3; i++)
            {
                new Thread(ReadItems).Start();
            }

            new Thread(WriteItems).Start("A");
            new Thread(WriteItems).Start("B");
        }

        /*
        通常，一个类型实例的并发读操作是线程安全的，而并发更新操作则不然。
        虽然可以简单地使用一个排它锁来保护对实例的任何形式的访问，但是如果其读操作很多但更新操作很少，则使用单一的锁限制并发性就不太合理了。

        ReaderWriterLockSlim是专门为这种情形进行设计的，它可以最大限度地保证锁的可用性。
        ReaderWriterLockSlim是在.NET Framework 3.5中引入的。它替代了笨重的ReaderWriterLock类。
        虽然后者具有相似的功能，但是它比前者的执行速度慢数倍，并且其本身存在一些锁升级处理机制的设计缺陷。

        与常规的lock（Monitor.Enter/Exit）相比ReaderWriterLockSlim的执行速度仍然慢一倍，但是它可以在大量的读操作和少量写操作的环境下减少锁竞争。

        ReaderWriterLockSlim和ReaderWriterLock都拥有两种基本的锁，即读锁和写锁：
            * 写锁是全局排它锁
            * 读锁可以兼容其他的读锁

        因此，一个持有写锁的线程将阻塞其他任何试图获取读锁或写锁的线程（反之亦然）。
        但是如果没有任何线程持有写锁的话，那么其他任意数量的线程都可以并发获得读锁。
        */
        static ReaderWriterLockSlim locker = new ReaderWriterLockSlim();

        static Random rand = new Random();

        static List<int> items = new List<int>();
        static void ReadItems()
        {
            while (true)
            {
                locker.EnterReadLock();
                foreach (int item in items) Thread.Sleep(100);
                locker.ExitReadLock();
            }
        }

        static void WriteItems(object threadID)
        {
            while (true)
            {
                int num = GetRandomNum(100);
                locker.EnterWriteLock();
                items.Add(num);
                locker.ExitWriteLock();

                Console.WriteLine("threadID:{0} num:{1}", threadID, num);

                Thread.Sleep(100);
            }
        }

        static int GetRandomNum(int max)
        {
            lock (rand)
            {
                int result = rand.Next(max);
                return result;
            }
        }
    }
}