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
            有时只需捕获特定类型的异常，并重新抛出其他类型的异常。
            AggregateException类的Handle方法可以快捷实现上述功能。
            它接受一个异常的断言并在每一个内部异常上验证该断言。

            如果断言返回true，说明该异常已经得到“处理”。
            在所有异常验证完毕之后会出现以下几种情况：
                1. 如果所有的异常都被“处理”了（即委托返回true），则不会重新抛出异常。
                2. 如果其中有异常的委托返回值为false（“未处理”），则会抛出一个新的Aggregate-Exception，且其中包含所有未处理的异常。
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
                try
                {
                    task.Wait();
                }
                catch (AggregateException exception)
                {
                    exception.Handle((e) =>
                    {
                        if (e.InnerException?.GetType() == typeof(TimeoutException))
                        {
                            Console.WriteLine("catch TimeoutException");
                            return true;
                        }
                        return false;
                    });
                }
            }
            catch (AggregateException exception)
            {
                var exs = exception.Flatten().InnerExceptions;
                foreach (var ex in exs)
                {
                    Console.WriteLine("other exception type:{0}", ex.GetType());
                }
            }

            Console.WriteLine("all finish");
        }
    }
}