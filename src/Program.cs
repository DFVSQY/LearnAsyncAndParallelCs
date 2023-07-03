using System;
using System.Net;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("main thread processorID:{0}", Thread.GetCurrentProcessorId());

            Go();

            Console.WriteLine("waiting task, processorID:{0}", Thread.GetCurrentProcessorId());

            Console.ReadLine();
        }

        static async void Go()
        {
            string[] urls = new string[]{
                "https://www.baidu.com/",
                "https://www.sohu.com/",
                "https://www.sina.com.cn/",
            };

            try
            {
                Console.WriteLine("start go, processorID:{0}", Thread.GetCurrentProcessorId());
                foreach (string url in urls)
                {
                    WebClient webClient = new WebClient();
                    byte[] data = await webClient.DownloadDataTaskAsync(url);
                    Console.WriteLine("url:{0} len:{1} processorID:{2}", url, data.Length, Thread.GetCurrentProcessorId());
                }
                Console.WriteLine("finish go, processorID:{0}", Thread.GetCurrentProcessorId());
            }
            catch (WebException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Console.WriteLine("finally, processorID:{0}", Thread.GetCurrentProcessorId());
            }
        }


    }
}