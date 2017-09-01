using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;

namespace ParallelProc
{
    class Program
    {
        static void Main(string[] args)
        {
            /// データの並列化
            Console.WriteLine("[[ for ]]");
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(500); // 時間のかかる処理
                Console.WriteLine("{0} ThreadId: {1}", i + 1, Thread.CurrentThread.ManagedThreadId);
            }

            Console.WriteLine("[[ Paralle for ]]");
            Parallel.For(0, 10, (i) => {
                Thread.Sleep(500);
                Console.WriteLine("{0} ThreadId: {1}", i + 1, Thread.CurrentThread.ManagedThreadId);
            });
            Console.WriteLine("Hit any key to continue...");
            Console.ReadKey();


            // タスクの並列化
            Console.WriteLine("[[ Paralle Invoke ]]");
            Parallel.Invoke(new Action[] {
                () => ProcA(),
                () => ProcB(),
                () => ProcC(),
                () => ProcD(),
            });
            Console.WriteLine("Hit any key to continue...");
            Console.ReadKey();

            // PLINQ
            IEnumerable<int> nums = Enumerable.Range(0, 10);
            Console.WriteLine("[[ Paralle ForEach ]]");
            Parallel.ForEach(nums, (i) => {
                Thread.Sleep(100); //時間のかかる処理
                Console.WriteLine("{0} ThreadId: {1}", i + 1, Thread.CurrentThread.ManagedThreadId);
            });

            // PLINQ
            Console.WriteLine("[[ Paralle ForEach ]]");
            Parallel.ForEach(nums, (i) => {
                Thread.Sleep(100); //時間のかかる処理
                Console.WriteLine("{0} ThreadId: {1}", i + 1, Thread.CurrentThread.ManagedThreadId);
            });

            // ForAll拡張メソッド(値を返さない)
            Console.WriteLine("[[ PLINQ<i> ]]");
            nums.AsParallel().ForAll((i)  => { 
                Thread.Sleep(100); //時間のかかる処理
                Console.WriteLine("{0} ThreadId: {1}", i + 1, Thread.CurrentThread.ManagedThreadId);
            });

            // Select(値を返す)拡張メソッド
            var rdm = new Random();
            Console.WriteLine("[[ PLINQ<2> ]]");
            nums.AsParallel()
                .Select<int, string>((i) => {
                    Thread.Sleep(100 + rdm.Next(100)); //時間のかかる処理
                    Console.WriteLine("Select {0} ThreadId:{1}", i, Thread.CurrentThread.ManagedThreadId);
                    return (i + 1).ToString("00");
                })
                .ForAll((s) => {
                    Thread.Sleep(100 + rdm.Next(100)); //時間のかかる処理
                    Console.WriteLine("ForAll {0} ThreadId:{1}", s, Thread.CurrentThread.ManagedThreadId);
                });

            Console.WriteLine("Hit any key to exit...");
            Console.ReadKey();
        }

        static void ProcA()
        {
            Thread.Sleep(500);
            Console.WriteLine("{0} ThreadId:{1}",MethodBase.GetCurrentMethod().Name, Thread.CurrentThread.ManagedThreadId);
        }
        static void ProcB()
        {
            Thread.Sleep(500);
            Console.WriteLine("{0} ThreadId:{1}",MethodBase.GetCurrentMethod().Name, Thread.CurrentThread.ManagedThreadId);
        }
        static void ProcC()
        {
            Thread.Sleep(500);
            Console.WriteLine("{0} ThreadId:{1}",MethodBase.GetCurrentMethod().Name, Thread.CurrentThread.ManagedThreadId);
        }
        static void ProcD()
        {
            Thread.Sleep(500);
            Console.WriteLine("{0} ThreadId:{1}",MethodBase.GetCurrentMethod().Name, Thread.CurrentThread.ManagedThreadId);
        }
    }
}
