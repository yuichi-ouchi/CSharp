using System;
using System.Threading;
using System.Threading.Tasks;

namespace MemberAccessSeparate2
{
    class Program
    {
        static void Main(string[] args)
        {
            Task t1 = Task.Run(() => (new Program()).Worker(true));
            Thread.Sleep(500);
            Task t2 = Task.Run(() => (new Program()).Worker(false));

            Task.WaitAll(t1, t2);
            Console.WriteLine("Hit any key to exit...");
            Console.ReadKey();
        }

        //[ThreadStatic]
        int number;
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
