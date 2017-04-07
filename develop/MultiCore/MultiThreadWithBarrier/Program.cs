using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreadWithBarrier
{
    class Program
    {
        static int ThreadNumber = 3; //同時実行するスレッド数

        // 独立処理用バリア
        static Barrier _barrierA = new Barrier(ThreadNumber, (b) =>
                // バリア通過時の時刻を表示
                Console.WriteLine("##: {0} >--- BARRIER A ---<", DateTime.Now.ToString("ss.fff"))
            );
        // 情報交換用バリア
        static Barrier _barrierB = new Barrier(ThreadNumber, (b) =>
                Console.WriteLine("##: {0} >--- BARRIER A ---<", DateTime.Now.ToString("ss.fff"))
            );

        static int _stopFlag = 0; //スレッド停止用フラグ


        static void Main(string[] args)
        {
            Console.WriteLine("##: {0} PROGRAM START", DateTime.Now.ToString("ss.fff"));
            Task[] tasks = new Task[ThreadNumber];
            for (int i = 0; i < ThreadNumber; i++)
            {
                int index = i;
                tasks[i] = Task.Run(() => DoWork(index));
                Thread.Sleep(50);
            }
            Console.WriteLine("Hit any key to stop...");
            Console.ReadKey();

            //スレッドを全部止める
            Interlocked.Increment(ref _stopFlag);
            Task.WaitAll(tasks);
            Console.WriteLine("##: {0} PROGRAM STOPPED", DateTime.Now.ToString("ss.fff"));

            Console.WriteLine("Hit any key to stop...");
            Console.ReadKey();

            _barrierA.Dispose();
            _barrierB.Dispose();
        }

        static void DoWork(int index)
        {
            int count = 0;
            int threadId = Thread.CurrentThread.ManagedThreadId;
            var rnd = new Random();

            while(_stopFlag == 0)
            {
                //スレッドごとに独立した処理
                // ここでは500msほどかかるものとする
                Console.WriteLine("#{0}: {1} {2} START 独立処理-{3}", index, DateTime.Now.ToString("ss.fff"), threadId, ++count);
                Thread.Sleep(500 + rnd.Next(-20, +20));
                Console.WriteLine("#{0}: {1} {2} END 独立処理-{3}", index, DateTime.Now.ToString("ss.fff"), threadId, ++count);

                //全スレッドがバリアに到達するのをまつ
                _barrierA.SignalAndWait();

                //スレッド間で情報を交換する処理
                // ここでは500msほどかかるものとする
                Console.WriteLine("#{0}: {1} {2} START 情報交換-{3}", index, DateTime.Now.ToString("ss.fff"), threadId, ++count);
                Thread.Sleep(500 + rnd.Next(-20, +20));
                Console.WriteLine("#{0}: {1} {2} END 情報交換-{3}", index, DateTime.Now.ToString("ss.fff"), threadId, ++count);

                //全スレッドがバリアに到達するのをまつ
                _barrierB.SignalAndWait();
            }

        }
    }
}
