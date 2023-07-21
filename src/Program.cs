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

        /*
        有时最好能在一个原子操作中将读锁转换为写锁。例如，我们希望当列表不包含特定元素时才将这个元素添加到列表中。
        理想情况下，我们希望尽可能缩短持有写锁（排它锁）的时间，假设可以采取如下的操作步骤：
            1．获取一个读锁
            2．判断该元素是否已经位于列表中，如果确已存在，则释放读锁并返回
            3．释放读锁
            4．获得写锁
            5．添加该元素
        上述操作的问题在于，另一个线程可能会在第3和第4步之间插入并修改链表（例如，添加同一个元素）。

        而ReaderWriterLockSlim可以通过第三种锁来解决这个问题，称为可升级锁（upgradable lock）。
        一个可升级锁就像读锁一样，但是它可以在随后通过一个原子操作升级为一个写锁。
        其使用方式：
            1．调用EnterUpgradableReadLock。
            2．执行读操作（例如，判断该元素是否已经存在于列表中）。
            3．调用EnterWriteLock（该操作将可升级锁转化为写锁）。
            4．执行基于写的操作（例如，将该元素添加到列表中）。
            5．调用ExitWriteLock（将写锁转换回可升级锁）。
            6．执行其他读操作。
            7．调用ExitUpgradableReadLock。

        可升级锁和读锁还有一个重要的区别：虽然可升级锁可以和任意数目的读锁并存，但是一次只能获取一个可升级锁。
        这可以将锁的升级竞争序列化从而避免在升级中出现死锁，这和SQL Server中的更新锁是一致的。
        */
        static void WriteItems(object threadID)
        {
            while (true)
            {
                int num = GetRandomNum(100);

                locker.EnterUpgradeableReadLock();
                if (!items.Contains(num))
                {
                    locker.EnterWriteLock();
                    items.Add(num);
                    locker.ExitWriteLock();
                    Console.WriteLine("threadID:{0} num:{1}", threadID, num);
                }
                locker.ExitUpgradeableReadLock();

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