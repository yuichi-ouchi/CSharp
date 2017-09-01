using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HandleExceptionOnUnhandledEx
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.GetDomain().UnhandledException += new UnhandledExceptionEventHandler(UnhandledExceptionHandler);

            try
            {
                Thread t = new Thread(new ThreadStart(DoWork)); //スレッド開始
                t.Start();
                Console.WriteLine("M: {0} Thread.Start()",  DateTime.Now.ToString("ss.fff"));
                t.Join();
                Console.WriteLine("M: {0} Thread.Join()",  DateTime.Now.ToString("ss.fff"));

            }
            catch (Exception)
            {
                // ここではキャッチできない
                Console.WriteLine("Catch Exception by try-chach ...");
                Console.ReadKey();
            }
            Console.WriteLine("Hit any key to continue...");
            Console.ReadKey();
        }

        private static void DoWork()
        {    
            Console.WriteLine("T: {0} Thread Start",  DateTime.Now.ToString("ss.fff"));
            Thread.Sleep(500);
            Console.WriteLine("T: {0} Thread Raise Exception",  DateTime.Now.ToString("ss.fff"));
            throw new InvalidOperationException("スレッドで発生した例外");

        }

        private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            if(ex != null)
            {
                Console.WriteLine("M: {0} Catch Exception by UnhandledExceptionHandler",
                    DateTime.Now.ToString("ss.fff"));
                Console.WriteLine(ex.ToString());
                Console.WriteLine("Hit any key to continue...");
                Console.ReadKey();

                //キャッチはできた。しかし、このハンドルを抜けると
                //プログラムは終了させられてしまう
            }
        }
    }
}
