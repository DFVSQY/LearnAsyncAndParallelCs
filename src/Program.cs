using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using System.Timers;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main()
        {
            /*
            .NET Framework 4.0在System.Collections.Concurrent命名空间下引入了一系列新的集合。所有这些集合都是完全线程安全的。

            并发集合对高并发场景进行了优化。但是它们也可以单纯作为一般的线程安全的集合使用（替代用锁保护的一般集合）。
            但是使用时需要注意：
                · 传统集合在非并发场景下的性能要高于并发集合。
                · 线程安全的集合并不能保证使用它们的代码是线程安全的。
                · 在枚举并发集合时，如果另一个线程更新了集合的内容，不会抛出任何异常。相反的，我们会得到一个新旧内容混合的结果。
                · List<T>没有对应的并发集合。
                · ConcurrentStack、ConcurrentQueue和ConcurrentBag类型内部是使用链表实现的。
                
            因此，其内存利用不如非并发的Stack和Queue高效。但是它们适用于并发访问，因为链表更容易实现无锁算法或者少锁的算法。
            （这是因为在链表中插入一个节点只需要更新几个引用，而在一个类似List<T>的结构中插入一个元素可能需要移动数以千计的现有元素。）
            因此，并发集合绝不仅仅是在普通集合上加了一把锁这么简单。

            并发集合和传统集合的另一个不同在于并发集合提供了原子的检测并执行操作，例如TryPop。
            其中大部分方法是通过IProducerConsumerCollection<T>接口统一起来的。
            */


            Console.WriteLine("all finish");
        }
    }
}