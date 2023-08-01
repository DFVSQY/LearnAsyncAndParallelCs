using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using System.Timers;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {

        class ObjectPool<T>
        {
            /*
            ConcurrentBag<T>是一个无序的对象集合（而且集合中允许出现重复的对象）。
            如果我们不关心调用Take或者TryTake时所获得的元素的顺序，就可以使用Concu-rrentBag<T>类。
            ConcurrentBag<T>可以非常高效地在多个线程上并行调用Add方法而几乎不会出现竞争。
            相反，ConcurrentQueue和ConcurrentStack的Add方法会造成一些竞争（但即使这样，其竞争也比单纯用锁来保护非并发集合的做法小得多）。
            若每个线程移除的元素比其添加的元素少，那么ConcurrentBag<T>的Take方法执行也是非常高效的。

            一个ConcurrentBag<T>对象上的每一个线程都有自己的私有链表。
            线程在调用Add方法时会将元素添加到自己的私有链表中，因此不会出现竞争。
            当我们枚举集合中的元素时，其枚举器会遍历每一个线程的私有链表，依次返回其中的每一个元素。

            在调用Take时，ConcurrentBag<T>首先会查询当前线程的私有列表，如果列表中至少有一个元素存在，那么该操作就可以在不引入竞争的情况下完成。
            但是，如果私有列表是空的，则必须从其他线程的私有列表中“窃取”一个元素，而这种操作可能造成竞争。
            因此，准确地说，Take方法将返回调用线程在集合中最近添加的元素。
            如果该线程上已经没有任何元素，它会返回其他线程（随机挑选）最近添加的元素。
            当集合的并行操作大部分是添加元素时，或者各个线程添加元素和移除元素数目基本平衡时，那么ConcurrentBag<T>类是理想的选择。

            ConcurrentBag<T>不适于实现生产者/消费者队列，因为元素的添加和移除操作是在不同的线程间执行的。
            */
            private readonly ConcurrentBag<T> bag;
            private readonly Func<T> generator;

            public ObjectPool(Func<T> gen)
            {
                generator = gen ?? throw new ArgumentNullException(nameof(gen));
                bag = new ConcurrentBag<T>();
            }

            public T Get() => bag.TryTake(out T? item) ? item : generator();

            public void Return(T item) => bag.Add(item);
        }

        class DataST
        {
            public int data { get; set; }
        }

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

            ObjectPool<DataST> pool = new ObjectPool<DataST>(() => new DataST());

            Parallel.For(0, 100, (idx) =>
            {
                DataST st = pool.Get();

                int old_value = st.data;
                st.data = idx;
                int new_value = st.data;

                Console.WriteLine("old_value:{0} new_value:{1}", old_value, new_value);

                pool.Return(st);
            });

            Console.WriteLine("all finish");
        }
    }
}