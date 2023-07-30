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
            Task task1 = Task.Run(() =>
            {
                Thread.Sleep(1000);
                throw null;
            });

            Task task2 = Task.Run(() =>
            {
                Thread.Sleep(2000);
                throw null;
            });

            Task task3 = Task.Run(() =>
            {
                Thread.Sleep(3000);
                throw null;
            });

            /*
            可以调用Task.WaitAll（等待所有任务执行结束）静态方法和Task.WaitAny（等待任意一个任务执行结束）同时等待多个任务。
            WaitAll方法类似于轮流等待每一个任务，但是它的效率更高，因为它至多只需要进行一次上下文切换。
            此外，如果有一个或者多个任务中抛出了未处理的异常，则WaitAll仍然会等待所有任务完成，然后再组合所有失败任务的异常，
            重新抛出一个AggregateException（而这也是AggregateException发挥真正作用的时候）。
            
            以上过程相当于如何代码：
            */
            var exceptions = new List<Exception>();
            try { task1.Wait(); } catch (AggregateException ex) { exceptions.Add(ex); }
            try { task2.Wait(); } catch (AggregateException ex) { exceptions.Add(ex); }
            try { task3.Wait(); } catch (AggregateException ex) { exceptions.Add(ex); }
            if (exceptions.Count > 0) throw new AggregateException(exceptions);

            /*
            调用WaitAny相当于等待一个ManualResetEventSlim对象。这个对象会在任意一个任务完成时触发。
            除了超时时间之外，还可以向每一个Wait方法中传入一个取消令牌来取消等待过程（而不是取消任务本身）。
            */

            Console.WriteLine("all finish");
        }

        static void Run(object? msg)
        {
            Console.WriteLine(msg);
        }
    }
}