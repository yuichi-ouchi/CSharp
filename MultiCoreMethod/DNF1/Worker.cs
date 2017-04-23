using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DNF2
{
    internal class Worker
    {
        private const int CountOfNumbers = 10;
        private int _currentIndex;
        private object _currentIndexLock = new object();

        #region " constractors & initalize "
        public Worker()
        {
            InitalizeData();
        }
        private void InitalizeData()
        {
            for (int i = 0; i < CountOfNumbers; i++)
            {
                InputData[i] = i + 1;
                OutputData[i] = null;
            }
            this._currentWorkerThreads = 0;
            this._currentIndex = 0;
        }
        #endregion

        private int FindNextIndex()
        {
            lock (this._currentIndexLock)
            {
                int current = this._currentIndex;
                if (current < CountOfNumbers)
                {
                    _currentIndex++;
                    return current;
                }
            }
            return -1;
        }

        private int _currentWorkerThreads;
        public int CurrentWorkerThreads
        {
            get
            {
                return _currentWorkerThreads;
            }
        }

        /// <summary>
        /// スレッドプールのキューにDoProcessを追加
        /// </summary>
        /// <param name="threadNumber">追加する数</param>
        public void StartWorkerThread(int threadNumber)
        {
            for (int i = 0; i < threadNumber; i++)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(DoProcess));

            }
        }

        private void DoProcess(object state) //引数未使用
        {
            Interlocked.Increment(ref this._currentWorkerThreads);
            try
            {
                Console.WriteLine("スレッド開始 ManagedThreadId:{0}", Thread.CurrentThread.ManagedThreadId);

                while (ProcessNextNumber()) ;

                Console.WriteLine("スレッド終了 ManagedThreadId:{0}", Thread.CurrentThread.ManagedThreadId);
            }
            finally
            {
                Interlocked.Decrement(ref this._currentWorkerThreads);
                if (state != null)
                {
                    //DoWorkからの実行時には、AutoResetEventが入っている
                    AutoResetEvent e = (AutoResetEvent)state;
                    e.Set();
                }
            }
        }

        internal bool ProcessNextNumber()
        {
            int index = FindNextIndex();
            if (index < 0)
                return false;

            ProcessANumber(index);
            return true;
        }
        internal void ProcessANumber(int index)
        {
            int input = InputData[index];
            string output = string.Format("{0:000}", input);
            RandomWait();
            OutputData[index] = output;

            Console.WriteLine("OutputDate[{0}] = {1},ManagedThreadId={2}"
                , index, output, Thread.CurrentThread.ManagedThreadId);

        }

        // 複数のスレッドから書き換えは発生しないのでロック不要
        public int[] InputData = new int[CountOfNumbers];
        public string[] OutputData  = new string[CountOfNumbers];
        
        /// <summary>
        ///動作をわかりやすくするためWaitを入れる(平均300ms) 
        /// </summary>
        private void RandomWait()
        {
            int waitTime = (new Random()).Next(100, 500);
            Thread.Sleep(waitTime);
        }


        public static Worker DoWork(int threadNumber)
        {
            // AutoResetEvent配列
            var autoEvents = new AutoResetEvent[threadNumber];
            for (int i = 0; i < threadNumber; i++)
                autoEvents[i] = new AutoResetEvent(false);


            //複数ワーカースレッドの立ち上げ
            // 全て処理終了後に制御を戻す
            var instance = new Worker();
            for (int i = 0; i < threadNumber; i++)
                ThreadPool.QueueUserWorkItem(new WaitCallback(instance.DoProcess), autoEvents[i]);

            WaitHandle.WaitAll(autoEvents);
            return instance;
        }

    }
}
