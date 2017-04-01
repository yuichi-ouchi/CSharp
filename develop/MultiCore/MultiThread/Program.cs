using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThread
{
    class Program
    {
        static int _flag = 0;
        static void Main(string[] args)
        {
            Console.WriteLine("##: {0} WAIT", DateTime.Now.ToString("ss.fff"));

            for (int i = 0; i < 3; i++)
            {
                int index = i;
                Task.Run(() => DoWork(index));
                Thread.Sleep(50); //  わざと50msずつ遅らせてスタート
            }

            using (Timer timer = new Timer((state) => KickWork(state), null, 1000, 1000))
            {
                //キーが押されるまで
                Console.WriteLine("Hit any key to stop ...");
                Console.ReadKey();
            }
            Console.WriteLine("Hit any key to exit ...");
            Console.ReadKey();

        }

        static object KickWork(object state)
        {
            Console.WriteLine("##: {0} FLAG ON", DateTime.Now.ToString("ss.fff"));
            Interlocked.Increment(ref _flag);

            //100ms以内に全スレッドが動き出す(余裕をもって150msまってフラグを倒す）
            Thread.Sleep(150);
            Console.WriteLine("##: {0} FLAG OFF", DateTime.Now.ToString("ss.fff"));
            Interlocked.Decrement(ref _flag);

            return null;

        }

        //スレッドの処理
        static void DoWork(int index)
        {
            int count = 0;
            int threadId = Thread.CurrentThread.ManagedThreadId;
            while(true)
            {
                //フラグが立つまで待機
                while(_flag == 0)
                    Thread.Sleep(100);

                //フラグが変わったら一斉スタート
                Console.WriteLine("#{0}: {1} {2} START-{3}", index, DateTime.Now.ToString("ss.fff"), threadId, ++count);

                // 処理が終わったらフラグが倒されるまで待機
                while(_flag != 0)
                {
                    Thread.Sleep(100);
                }
            }
        }
    }
}
