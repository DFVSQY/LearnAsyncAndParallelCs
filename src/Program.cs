using System;
using System.Diagnostics;
using System.Net;
using System.Timers;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("start main thread:{0}", Thread.CurrentThread.ManagedThreadId);

            /*
            .NET Framework在System.Timers命名空间中提供了另外一个同名定时器类。
            它简单包装了System.Threading.Timer，在相同底层引擎的基础上提供了额外的易用性。

            它的附加功能：
                1. 实现了IComponent接口，允许嵌入到Visual Studio设计器的组件托盘中。
                2. 提供了Interval属性替代Change方法。
                3. 提供了Elapsed事件取代回调委托。
                4. 提供了Enabled属性来开始和停止计时器（默认值为false）。
                5. 如果不习惯使用Enabled属性还可以使用Start和Stop方法。
                6. 提供了AutoReset标志，用于指示重复的事件（默认值为true）。
                7. 提供了SynchronizingObject属性。可调用该对象的Invoke和BeginInvoke方法安全地调用WPF元素和Windows Forms控件的方法。
            */
            using (System.Timers.Timer timer = new System.Timers.Timer())
            {
                timer.Interval = 1000;
                timer.Elapsed += Run;

                timer.Start();
                Console.ReadLine();
                timer.Stop();
            }

            Console.WriteLine("finish main thread:{0}", Thread.CurrentThread.ManagedThreadId);
        }

        private static void Run(object? sender, EventArgs e)
        {
            Console.WriteLine("tick ..., thread:{0}", Thread.CurrentThread.ManagedThreadId);
        }
    }
}