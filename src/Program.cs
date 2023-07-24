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
            IEnumerable<int> numbers = Enumerable.Range(3, 100000 - 3);

            /*
            PLINQ可以自动并行化本地LINQ查询。易于使用是PLINQ的优势，因为它将工作划分和结果整理的任务交给了Framework。
            要使用PLINQ只需直接在输入序列上调用AsParallel()方法，而后和先前一样编写普通的LINQ查询即可。

            AsParallel是System.Linq.ParallelEnumerable类的一个扩展方法，它将输入包装为一个以ParallelQuery<TSource>为基类的序列，
            这样，后续的LINQ查询运算符就会绑定到由ParallelEnumerable定义的另外一套扩展方法上。
            这些扩展方法为每一种标准查询运算符提供了并行化实现。
            基本上，它们的工作原理都是将输入序列划分为小块，并将每一块在不同的线程上执行，并将执行结果整理为一个输出序列以供使用。

            调用AsSequential()会将ParallelQuery序列包装解除，后续的查询运算符将会重新绑定到标准查询运算符上并顺序执行。
            这在调用有副作用或者非线程安全的代码之前是非常必要的。
            */
            var parallelQuery =
              from n in numbers.AsParallel()
              where Enumerable.Range(2, (int)Math.Sqrt(n)).All(i => n % i > 0)
              select n;

            int[] primes = parallelQuery.ToArray();

            foreach (int prime in primes)
            {
                Console.Write("{0} ", prime);
            }
        }
    }
}