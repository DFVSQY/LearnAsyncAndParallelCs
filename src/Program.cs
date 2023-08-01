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
            PLINQ、Parallel类和Task会自动将异常封送给消费者。这个操作是必不可少的。

            PLINQ和Parallel类在遇到第一个异常时就会结束查询或循环的执行，不会进一步处理循环体中的其他元素。
            但是即使这样，在当前循环完成之前也有可能抛出更多的异常。
            若访问AggregateException的InnerException属性则只会获得第一个异常。

            AggregateException类提供了Flatten方法和Handle方法简化异常的处理过程。
            */

            /*
            AggregateException通常会包含其他的AggregateException。
            例如，当子任务抛出异常的时候就会出现这种情况。
            调用Flatten方法可以消除任意层级的嵌套以简化处理过程。
            这个方法会返回一个新的AggregateException对象，并包含展平的内部异常列表。
            */
            Task task = Task.Factory.StartNew(() =>
            {
                Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(1000);
                    throw new NullReferenceException();
                }, TaskCreationOptions.AttachedToParent);

                Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(1000);
                    throw new TimeoutException();
                }, TaskCreationOptions.AttachedToParent);
            });

            try
            {
                task.Wait();
            }
            catch (AggregateException exception)
            {
                var exs = exception.Flatten().InnerExceptions;
                foreach (var ex in exs)
                {
                    Console.WriteLine(ex.GetType());
                }
            }

            Console.WriteLine("all finish");
        }
    }
}