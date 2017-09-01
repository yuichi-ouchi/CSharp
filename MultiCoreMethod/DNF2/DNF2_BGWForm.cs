using AsyncWorkers;
using System;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DNF2
{
    public partial class DNF2_BGWForm : Form
    {
        public DNF2_BGWForm()
        {
            InitializeComponent();
        }

        private DateTime _start;
        private DateTime _stop;
        private int _clickThreadId;
        private int _callbackThreadId;

        #region " events "
        private void startButton_Click(object sender, EventArgs e)
        {
            this._start = DateTime.Now;
            this._clickThreadId = Thread.CurrentThread.ManagedThreadId;
            this.textBox.Text = string.Format("開始: {0:HH:mm:ss.fff}", this._start);

            this.backgroundWorker1.RunWorkerAsync();
        }
        #endregion

        #region " Update UI "
        private delegate void SetTextboxDelegate(string msg);

        private void ShowResult(Worker worker)
        {
            string msg = FormatResult(this._start, this._stop, worker, this._clickThreadId, this._callbackThreadId);
            SetTextboxText(msg);
        }

        private void SetTextboxText(string msg)
        {
#if false
            // BackgroundWorker利用の場合不要
            //if (this.InvokeRequired)
            //    this.Invoke(new SetTextboxDelegate(SetTextboxText), msg);
            //else
#endif
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
        #endregion 

        #region " BackgroundWorker Events "
        private void CallDoWork(object sender, DoWorkEventArgs e)
        {
            Worker wk = Worker.DoWork(2);
            e.Result = wk;
        }

        private void Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            this._stop = DateTime.Now;
            this._callbackThreadId = Thread.CurrentThread.ManagedThreadId;
            Worker wk = (Worker)e.Result;
            ShowResult(wk);
        }
        #endregion
    }
}
