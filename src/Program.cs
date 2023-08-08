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
            Console.WriteLine("all finish");
        }
    }

    /// <summary>
    /// 生产者/消费者队列
    /// </summary>
    public class PCQueue : IDisposable
    {
        /*
        我们介绍了三种生产者/消费者集合：
            ConcurrentStack<T>
            ConcurrentQueue<T>
            ConcurrentBag<T>
        
        若调用上述任一类型的TryTake方法，且相应的集合为空，则该方法会返回false。但是有时候我们更希望等待集合中出现新的元素。
        
        PFX的设计者并没有为这个功能重载TryTake方法（这可能导致必须添加一系列的成员方法来接收取消令牌和超时时间设置），
        而是将这个功能封装在了BlockingCollection<T>包装类中。这种阻塞集合可以包装任意实现了IProducerConsumerCollection<T>接口的集合类型。
        而其Take方法将从内部集合中取出一个元素，并在内部集合为空时阻塞操作。阻塞集合还支持限制集合的总体大小，并在集合元素数目超出设定时阻塞生产者。
        具有这种限制的集合成为有界阻塞集合（bounded blocking collection）。
        
        使用BlockingCollection<T>的步骤如下：
            1．创建阻塞集合的实例，（可选）指定需要包装的IProducerConsumerCollection<T>与集合的最大元素数目（边界）。
            2．调用Add或者TryAdd方法在底层集合上添加元素。
            3．调用Take或者TryTake从底层集合中移除（消费）元素。

        GetConsumingEnumerable是另一种消费阻塞集合元素的方法。
        该方法返回一个无穷序列。当集合中有元素可供消费时，该序列就会返回这些元素。
        调用CompleteAdding方法会强制终止该序列，亦无法继续向该集合中添加新元素。
        BlockingCollection<T>中的AndToAny和TakeFromAny静态方法分别向若干个阻塞集合中添加元素，或从若干个阻塞集合中取出元素；
        其中第一个响应请求的集合将执行操作。
        */
        private readonly BlockingCollection<Action> taskQueue = new BlockingCollection<Action>();

        public PCQueue(int workerCount)
        {
            for (int i = 0; i < workerCount; i++)
                Task.Factory.StartNew(Consume);
        }

        public void Enqueue(Action action)
        {
            taskQueue.Add(action);
        }

        private void Consume()
        {
            foreach (Action action in taskQueue.GetConsumingEnumerable())
                action();
        }

        public void Dispose()
        {
            taskQueue.CompleteAdding();
        }
    }
}