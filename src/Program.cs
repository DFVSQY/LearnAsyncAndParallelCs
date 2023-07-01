using System;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Task<int> task = Task.Run(Run);
            var waiter = task.GetAwaiter();

            waiter.OnCompleted(() =>
            {
                /*
                如果先导任务出现错误，则当延续代码调用awaiter.GetResult()的时候将会重新抛出异常。
                当然我们也可以访问先导任务的Result属性而不是调用GetResult方法。
                但如果先导任务失败，则调用GetResult方法就可以直接得到原始的异常，而不是包装后的AggregateException。
                因此，这种方式可以实现更加简洁清晰的catch代码块。
                
                对于非泛型任务，GetResult的返回值为void，而这个函数的用途完全是为了重新抛出异常。
                */
                int result = waiter.GetResult();
                Console.WriteLine("result:" + result.ToString());
            });

            Console.WriteLine("iscompleted:" + task.IsCompleted);

            Console.ReadLine();
        }

        static int Run()
        {
            Console.WriteLine("start run task");
            int sum = 0;
            for (int i = 1; i <= 100; i++) sum += i;
            Console.WriteLine("finish run task");
            return sum;
        }
    }
}