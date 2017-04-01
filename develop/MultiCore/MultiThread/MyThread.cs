using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThread
{
    class MyThread
    {
        public static void RunThread()
        {
            Thread t = new Thread(OutputMessage);

            t.Start();

        }
        //public static void RunThreadPool()
        //{
        //    object state = false;
        //    bool queRequest = ThreadPool.QueueUserWorkItem(
        //        new WaitCallback(OutputMessage(), state)
        //        );
        //}
        private static void OutputMessage()
        {
            Console.ReadKey();
            Console.WriteLine("--- start ---");
            Thread.Sleep(10000);
            Console.WriteLine("--- end ---");
            Console.ReadKey();
        }
    }
}
