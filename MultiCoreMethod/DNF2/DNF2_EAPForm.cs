using AsyncWorkers;
using System;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DNF2
{
    public partial class DNF2_EAPForm : Form
    {
        public DNF2_EAPForm()
        {
            InitializeComponent();
        }

        private int _clickThreadId;
        private int _callbackThreadId;
        private DateTime _start;
        private DateTime _stop;


        #region " events "
        private void startButton_Click(object sender, EventArgs e)
        {
            this._start = DateTime.Now;
            this._clickThreadId = Thread.CurrentThread.ManagedThreadId;
            this.textBox.Text = string.Format("開始: {0:HH:mm:ss.fff}", this._start);

            EapWorker worker = new EapWorker();
            worker.DoWoorkCompleted += Completed;
            worker.DoWorkAsync(2);
        }
        #endregion


        private void ShowResult(EapWorker worker)
        {
            string msg = FormatResult(this._start, this._stop, worker, this._clickThreadId, this._callbackThreadId);
            SetTextboxText(msg);
        }

        private void SetTextboxText(string msg)
        {
#if false
            // BackgroundWorker利用な不要
            //if (this.InvokeRequired)
            //    this.Invoke(new SetTextboxDelegate(SetTextboxText), msg);
            //else
#endif
            this.textBox.Text = msg;
        }

        private string FormatResult(DateTime start, DateTime stop, EapWorker worker, int threadIdStart, int threadIdCallback)
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

        private void Completed(object sender, DoWorkCompletedEventArgs e)
        {
            this._stop = DateTime.Now;
            this._callbackThreadId = Thread.CurrentThread.ManagedThreadId;
            EapWorker result = e.Worker;
            ShowResult(result);
        }
    }
}
