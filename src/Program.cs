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
            Task.Run方法会创建并启动一个Task或者Task<TResult>对象。
            这个方法实际上和Task.Factory.StartNew是等价的。而后者具有更多的重载版本，也更具灵活性。
            */

            /*
            Task.Factory.StartNew方法可以指定一个状态对象，这个对象会作为参数传递给目标方法，因此目标方法的签名中也必须包含一个object类型的参数。
            该方式可以避免在Lambda表达式中直接调用Run而造成闭包开销。这种优化并不明显，因此实践中很少使用。
            */
            Task task1 = Task.Factory.StartNew(Run, "vscode");
            task1.Wait();

            Console.WriteLine("task1 finish");

            /*
            可以利用状态对象来给任务指定一个有意义的名称。之后就可以使用AsyncState属性查询这个名称。
            Visual Studio在并行任务窗口中会显示每一个任务的AsyncState，因此取一个有意义的名字可以让调试变得更加轻松。
            */
            Task task2 = Task.Factory.StartNew(state => { Run("vscode"); }, "stateInfo");
            task2.Wait();
            Console.WriteLine("task2 finish, state:{0}", task2.AsyncState);
        }

        static void Run(object? msg)
        {
            Console.WriteLine(msg);
        }
    }
}