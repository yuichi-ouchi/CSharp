using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ComMemberAccess
{
    class Program
    {
        static void Main(string[] args)
        {
            Program instance = new Program();
            Task t1 = Task.Run(() => instance.Worker(true));
            Thread.Sleep(500);
            Task t2 = Task.Run(() => instance.Worker(false));

            Task.WaitAll(t1, t2);
            Console.WriteLine("Hit any key to exit...");
            Console.ReadKey();
        }

        // この属性でスレッド間で変数が別々に扱われる
        [ThreadStatic]
        static int number;
        private void Worker(bool increment)
        {
            for (int i = 0; i < 3; i++)
            {
                if(increment)
                {
                    Interlocked.Increment(ref number);
                    Console.WriteLine("Increment: {0}", number);
                }
                else
                {
                    Interlocked.Decrement(ref number);
                    Console.WriteLine("Decrement: {0}", number);
                }
            Thread.Sleep(1000);
            }
        }
    }
}
