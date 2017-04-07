using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HandleExceptionOnDelegate
{
    class Program
    {
        delegate void DoWorkDelegate();
        static bool isRunning = true;
        static void Main(string[] args)
        {

            IAsyncResult ar = (new DoWorkDelegate(DoWork)).BeginInvoke(new AsyncCallback(WorkComplete), null);
            Console.WriteLine("M: {0} BeginInvoke()",  DateTime.Now.ToString("ss.fff"));
            ar.AsyncWaitHandle.WaitOne();
            Console.WriteLine("M: {0} WaitOne()",  DateTime.Now.ToString("ss.fff"));

            while (isRunning)
                Thread.Sleep(100);

            Console.WriteLine("Catch Exception by try-chach ...");
            Console.ReadKey();

        }

        private static void WorkComplete(IAsyncResult ar)
        {
            DoWorkDelegate d = (DoWorkDelegate) ((AsyncResult)ar).AsyncDelegate;
            try
            {
                d.EndInvoke(ar);
            }
            catch (Exception ex)
            {
                // EndInvolke()で例外をトラップできる
                // ただし、ここはUIスレッドではないので注意!!
                Console.WriteLine("Catch Exception at EndInvoke.");
                Console.WriteLine("{0}: {1}", ex.GetType().Name, ex.Message);
                Console.WriteLine("Hit any key to continue...");
                Console.ReadKey();
            }
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
