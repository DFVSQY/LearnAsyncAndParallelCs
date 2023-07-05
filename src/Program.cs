using System;
using System.Net;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("main thread processorID:{0}", Thread.GetCurrentProcessorId());

            // CancellationToken cancellationToken = new CancellationToken();

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(6000);
            await Run(cancellationTokenSource.Token);

            Console.WriteLine("all finish, processorID:{0}", Thread.GetCurrentProcessorId());
        }

        static async Task Run(CancellationToken cancellation)
        {
            try
            {
                for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine("i:{0} processorID:{1}", i, Thread.GetCurrentProcessorId());
                    await Task.Delay(5000, cancellation);
                }
            }
            catch (OperationCanceledException e)
            {
                Console.WriteLine("msg:{0} processorID:{1}", e.Message, Thread.GetCurrentProcessorId());
            }
            finally
            {
                Console.WriteLine("run finally, processorID:{0}", Thread.GetCurrentProcessorId());
            }
        }
    }
}