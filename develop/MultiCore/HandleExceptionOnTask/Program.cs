using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HandleExceptionOnTask
{
    class Program
    {
        static void Main(string[] args)
        {

            Task t = Task.Factory.StartNew(() => DoWork());
            Console.WriteLine("M: {0} Task.Factory.StartNew()",  DateTime.Now.ToString("ss.fff"));
            try
            {
                t.Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine("M: Catch Exception by try-catch");
                Console.WriteLine("Exception - {0}: {1}", ex.GetType().Name, ex.Message);
                Console.WriteLine("InnerException - {0}: {1}", ex.InnerException.GetType().Name, ex.InnerException.Message);
                Console.WriteLine("Hit any key to continue...");
                Console.ReadKey();

            }
            Console.WriteLine("Hit any key to exit...");
            Console.ReadKey();
        }

        private static void DoWork()
        {
            Console.WriteLine("T: {0} Thread Start",  DateTime.Now.ToString("ss.fff"));
            Thread.Sleep(500);
            Console.WriteLine("T: {0} Thread Raise Exception",  DateTime.Now.ToString("ss.fff"));
            throw new InvalidOperationException("スレッドで発生した例外");
        }
    }
}
