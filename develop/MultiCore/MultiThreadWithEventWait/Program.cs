using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreadWithEventWait
{
    class Program
    {
        static int ThreadNumber = 3; // 実行スレッド数
        static void Main(string[] args)
        {

            Console.WriteLine("##: {0} WAIT", DateTime.Now.ToString("ss.fff"));

            using (var waitHandle = new EventWaitHandle(false, EventResetMode.ManualReset))
            using (var countEvent = new CountdownEvent(0))
            {
                for (int i = 0; i < ThreadNumber; i++)
                {
                    int index = i;
                    Task.Run(() => DoWork(index, waitHandle, countEvent));
                    Thread.Sleep(50); // 50msほどずつ遅らせてスタート
                }

                var param = new TimerParameters()
                {
                    WaitHandle = waitHandle,
                    CountEvent = countEvent,
                };

                using(var timer = new Timer((state) => KickWork(state), param, 1000, 1000))
                {
                    Console.WriteLine("Hit any key to stop...");
                    Console.ReadKey();
                }

                countEvent.Wait(); //全スレッドの処理が止まるまで待機

                Console.WriteLine("Hit any key to stop...");
                Console.ReadKey();
            }
        }

        static object KickWork(object state)
        {
            var param = (TimerParameters)state;
            // カウントを設定
            param.CountEvent.Reset(ThreadNumber);
            // EventWaitHandleをシグナル状態にして、スレッドを一斉に動かす
            Console.WriteLine("##: {0} EventWaitHandle SIGNAL", DateTime.Now.ToString("ss.fff"));
            param.WaitHandle.Set();

            //他のスレッドを動かしたら、直ちに非シグナル状態に
            Thread.Sleep(0);
            param.WaitHandle.Reset();
            Console.WriteLine("##: {0} EventWaitHandle NON-SIGNAL", DateTime.Now.ToString("ss.fff"));

            // 全スレッドがCountDownEventにシグナルを送り終わるまで待機(不要)
            // ※時刻表示のためだけにWaitしている。本来は不要。
            param.CountEvent.Wait();
            Console.WriteLine("##: {0} CountEvent FIRED", DateTime.Now.ToString("ss.fff"));

            return null;
        }

        class TimerParameters
        {
                public EventWaitHandle WaitHandle { get; set; }
                public CountdownEvent CountEvent { get; set; }
        }

        private static void DoWork(int index, EventWaitHandle waitHandle, CountdownEvent countEvent)
        {
            int count = 0;
            int threadId = Thread.CurrentThread.ManagedThreadId;
            var rnd = new Random();

            while (true)
            {
                //EventWaitHandleがシグナル状態になるまで待機
                waitHandle.WaitOne();

                //シグナル状態になったら、一斉にスタート
                Console.WriteLine("#{0}: {1} {2} START-{3}", index, DateTime.Now.ToString("ss.fff"), threadId, ++count);

                // ここの処理は100ミリ秒ほどかかるものとする
                Thread.Sleep(100 + rnd.Next(-20, +20));
                Console.WriteLine("#{0}: {1} {2} End-{3}", index, DateTime.Now.ToString("ss.fff"), threadId, ++count);

                // 処理が終わったらCountdownEventにシグナルを送る
                countEvent.Signal();
            }
        }
    }
}
