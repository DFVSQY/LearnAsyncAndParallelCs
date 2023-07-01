using System;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Task<int> task = Task.Run(Run);

            /*
            任务可以方便地传播异常，这和线程是截然不同的。
            因此，如果任务中的代码抛出一个未处理异常（换言之，如果你的任务出错（fault）），
            那么调用Wait()或者访问Task<TResult>的Result属性时，该异常就会被重新抛出。
            */
            try
            {
                /*
                通过查询Result属性就可以获得任务的返回值。
                如果当前任务还没有执行完毕，则调用该属性会阻塞当前线程，直至任务结束。
                */
                int result = task.Result;
                Console.WriteLine("result:" + result.ToString());
            }
            catch (AggregateException e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine(task.IsFaulted);
        }

        static int Run()
        {
            Console.WriteLine("start run task");
            int result = 100, div = 0;
            result = result / div;
            Console.WriteLine("finish run task");
            return result;
        }
    }
}