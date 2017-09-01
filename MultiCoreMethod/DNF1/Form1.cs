using AsyncWorkers;
using System;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DNF1
{
    public partial class DNF1_Form : Form
    {
        public DNF1_Form()
        {
            InitializeComponent();
        }

        private Worker _worker;
        private DateTime _start;
        private DateTime _stop;
        private void startButton_Click(object sender, EventArgs e)
        {
            this._start = DateTime.Now;
            this.textBox.Text = string.Format("開始: {0:HH:mm:ss.fff}",this._start);
            this._worker = new Worker();
            this._worker.StartWorkerThread(2);

            this.timer1.Interval = 100;
            this.timer1.Start();
        }
        //System.Windows.Forms.Timerクラスで、UIスレッドでの実行を保証している
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this._worker.CurrentWorkerThreads == 0)
            {
                this.timer1.Stop();
                this._stop = DateTime.Now;

                string msg = FormatResult(this._start, this._stop, this._worker);
                SetTextboxText(msg);
            }
        }

        private string FormatResult(DateTime start, DateTime stop, Worker worker)
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Format("開始{0:HH:mm:ss.fff}", start));
            for (int i = 0; i < worker.OutputData.Length; i++)
            {
                sb.AppendFormat("{0}: '{1}'\r\n", i, worker.OutputData[i]);
            }
            sb.AppendLine(string.Format("終了{0:HH:mm:ss.fff}", stop));
            return sb.ToString();
        }


        private int _clickThreadId;
        private int _callbackThreadId;
        private delegate Worker DoWorkDelegate(int threadNumber);
        private delegate void SetTextboxDelegate(string msg);
        private void StartCbButton_Click(object sender, EventArgs e)
        {
            this._start = DateTime.Now;
            this._clickThreadId = Thread.CurrentThread.ManagedThreadId;
            this.textBox.Text = string.Format("開始: {0:HH:mm:ss.fff}", this._start);
            var d = new DoWorkDelegate(Worker.DoWork);
            d.BeginInvoke(2, new AsyncCallback(WorkComplete), null);
        }

        private void WorkComplete(IAsyncResult ar)
        {
            this._stop = DateTime.Now;
            this._callbackThreadId = Thread.CurrentThread.ManagedThreadId;

            DoWorkDelegate d = (DoWorkDelegate)((AsyncResult)ar).AsyncDelegate;
            Worker instance = d.EndInvoke(ar);

            ShowResult(instance);
        }

        private void ShowResult(Worker worker)
        {
            string msg = FormatResult(this._start, this._stop, worker, this._clickThreadId, this._callbackThreadId);
            SetTextboxText(msg);
        }

        private void SetTextboxText(string msg)
        {
            if (this.InvokeRequired)
                this.Invoke(new SetTextboxDelegate(SetTextboxText), msg);
            else
                this.textBox.Text = msg;

        }

        private string FormatResult(DateTime start, DateTime stop, Worker worker, int threadIdStart, int threadIdCallback)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("開始: {0:HH:mm:ss.fff}@スレッド:{1}", start, threadIdStart));
            for (int i = 0; i < worker.OutputData.Length; i++)
            {
                sb.AppendFormat("{0}: '{1}'\r\n", i, worker.OutputData[i]);
            }
            sb.AppendLine(string.Format("終了: {0:HH:mm:ss.fff}@スレッド:{1}", stop, threadIdCallback));
            return sb.ToString();
        }
    }
}
