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
            int answer = await PrintAnswer();
            Console.WriteLine("finish run, answer:{0}, processorID:{1}", answer, Thread.GetCurrentProcessorId());
        }

        /// <summary>
        /// 异步函数的方法体内并不需要显式返回一个任务。
        /// 编译器会负责生成任务，并在方法完成之前或出现未处理的异常时触发任务。
        /// 这样就很容易创建异步调用链。
        /// 编译器会展开异步函数，使用TaskCompletionSource创建一个任务，并将Task返回。
        /// 其展开版本类似于PrintAnswerExpand函数。
        /// 异步函数中若方法体返回TResult则函数的返回值为Task<TResult>
        /// </summary>
        static async Task<int> PrintAnswer()
        {
            Console.WriteLine("start printanswer, processorID:{0}", Thread.GetCurrentProcessorId());
            await Task.Delay(5000);
            int answer = 5 * 12;
            Console.WriteLine("answer:{0}, processorID:{1}", answer, Thread.GetCurrentProcessorId());
            return answer;
        }

        /// <summary>
        /// PrintAnswer的编译器展开版本
        /// </summary>
        static Task<int> PrintAnswerExpand()
        {
            var tcs = new TaskCompletionSource<int>();
            var awaiter = Task.Delay(5000).GetAwaiter();
            awaiter.OnCompleted(() =>
            {
                try
                {
                    awaiter.GetResult();     // Re-throw any exceptions
                    int answer = 5 * 12;
                    Console.WriteLine("answer:{0}, processorID:{1}", answer, Thread.GetCurrentProcessorId());
                    tcs.SetResult(answer);
                }
                catch (Exception ex) { tcs.SetException(ex); }
            });
            return tcs.Task;
        }
    }
}