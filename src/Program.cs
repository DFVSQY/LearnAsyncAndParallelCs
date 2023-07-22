using System;
using System.Diagnostics;
using System.Net;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static ManualResetEvent waitHandle = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            waitHandle.ToTask().GetAwaiter().OnCompleted(() =>
            {
                Console.WriteLine("task finished");
            });

            Console.WriteLine("waiting ...");
            Thread.Sleep(5000);
            Console.WriteLine("start signal ...");
            waitHandle.Set();

            Console.ReadLine();
        }

        static void Run(object data, bool timeOut)
        {
            Console.WriteLine("start run:{0}", data);
        }
    }

    static class WaitHandleExt
    {
        public static Task<bool> ToTask(this WaitHandle waitHandle, int timeout = -1)
        {
            var tcs = new TaskCompletionSource<bool>();

            RegisteredWaitHandle token = null;

            var tokenReady = new ManualResetEventSlim();

            WaitOrTimerCallback action = (state, timeOut) =>
            {
                tokenReady.Wait();
                tokenReady.Dispose();
                token.Unregister(waitHandle);
                tcs.SetResult(!timeOut);
            };

            token = ThreadPool.RegisterWaitForSingleObject(waitHandle, action, null, timeout, true);
            tokenReady.Set();

            return tcs.Task;
        }
    }
}