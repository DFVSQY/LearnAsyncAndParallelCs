using System;
using System.Net;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("main thread processorID:{0}", Thread.GetCurrentProcessorId());

            Run();

            Console.WriteLine("waiting task, processorID:{0}", Thread.GetCurrentProcessorId());

            Console.ReadLine();
        }

        static async void Run()
        {
            Console.WriteLine("start run, processorID:{0}", Thread.GetCurrentProcessorId());
            int answer = await PrintAnswer1();
            Console.WriteLine("finish run, answer2:{0}, processorID:{1}", answer, Thread.GetCurrentProcessorId());
        }

        /// <summary>
        /// 编写从不等待的异步方法也是合法的，编译器会相应的生成警告信息。
        /// 另一种方式是使用Task.FromResult方法，这个方法会返回一个已经结束了的任务，
        /// 具体实现参考PrintAnswer1函数
        /// </summary>
        static async Task<int> PrintAnswer()
        {
            Console.WriteLine("start printanswer, processorID:{0}", Thread.GetCurrentProcessorId());
            int answer = Thread.GetCurrentProcessorId();
            Console.WriteLine("answer:{0}, processorID:{1}", answer, Thread.GetCurrentProcessorId());

            return answer;
        }

        static Task<int> PrintAnswer1()
        {
            Console.WriteLine("start printanswer, processorID:{0}", Thread.GetCurrentProcessorId());
            int answer = Thread.GetCurrentProcessorId();
            Console.WriteLine("answer:{0}, processorID:{1}", answer, Thread.GetCurrentProcessorId());

            return Task.FromResult<int>(answer);
        }
    }
}