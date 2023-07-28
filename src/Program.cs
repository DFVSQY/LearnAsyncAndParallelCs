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
            当一个任务启动另一个任务时，可以使用askCreationOptions.AttachedToParent选项为它们确定父子任务关系。

            子任务是一类特殊的任务。因为父任务必须在所有子任务结束之后才能结束。
            而父任务结束时，子任务中发生的异常才会向上抛出。
            */
            Task task = Task.Factory.StartNew(
                () =>
                {
                    Console.WriteLine("this is parent task");

                    Task.Factory.StartNew(() =>
                    {
                        Console.WriteLine("this is detached task");
                    });

                    Task.Factory.StartNew(() =>
                    {
                        Console.WriteLine("this is a child task1");
                    }, TaskCreationOptions.AttachedToParent);

                    Task.Factory.StartNew(() =>
                    {
                        Thread.Sleep(1000);
                        Console.WriteLine("this is child task2");
                        throw null;
                    }, TaskCreationOptions.AttachedToParent);

                    Thread.Sleep(5000);
                    Console.WriteLine("parent task finish");
                }
            );
            task.Wait();

            Console.WriteLine("all finish");
        }

        static void Run(object? msg)
        {
            Console.WriteLine(msg);
        }
    }
}