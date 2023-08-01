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

            /*
            生产者/消费者集合主要有两种使用场景：
                · 添加一个元素（“生产”）
                · 检索一个元素并删除它（“消费”）
                
            栈和队列都是典型的生产者/消费者集合。
            这种集合在并行编程中有重要的意义，因为它们有利于实现高效的无锁设计。
            IProducerConsumerCollection<T>接口代表了一个线程安全的生产者/消费者集合。

            IProducerConsumerCollection<T>扩展了ICollection接口，并添加了以下方法：
             void CopyTo (T[] array, int index);
             T[] ToArray();
             bool TryAdd (T item);
             bool TryTake (out T item);

            其中TryAdd和TryTake方法会测试添加和删除操作是否可以执行，如果可以则执行该操作。
            测试和执行是以原子方式执行的，因此无须像传统集合那样在操作时加锁。

            TryTake方法在集合为空的情况下会返回false。
            TryAdd在现有的三个实现类中都必定成功，并返回true。
            但如果自定义的集合不允许出现重复元素（例如并发的集合），则该方法应在欲添加元素已经存在的情况下返回false。
            
            不同类型的TryTake方法执行的操作也各有差异：
                · 对于ConcurrentStack类型，TryTake会删除最近添加的元素。
                · 对于ConcurrentQueue类型，TryTake会删除最早添加的元素。
                · 对于ConcurrentBag类型，哪个元素删除效率最高，TryTake方法就会删除那个元素。
                
            以上三个具体类型都显式实现了TryTake和TryAdd方法，并用更加特定的公有方法来提供相应的功能，例如TryDequeue和TryPop。
            */


            Console.WriteLine("all finish");
        }
    }
}