using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AsyncWorkers;
using System.ComponentModel;
using System.Threading;

namespace DNF35
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private Worker _worker;
        private int _clickThreadId;
        private int _callbackThreadId;
        private DateTime _start;
        private DateTime _stop;

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            var bw = new BackgroundWorker() { WorkerReportsProgress = true};
            bw.DoWork += (o, arg) => CallDoWork(o, arg);
            bw.RunWorkerCompleted += (o, arg) => Completed(o, arg);

            bw.RunWorkerAsync();
        }

        #region " Update UI "
        private delegate void SetTextboxDelegate(string msg);

        private void ShowResult(Worker worker)
        {
            string msg = FormatResult(this._start, this._stop, worker, this._clickThreadId, this._callbackThreadId);
            SetTextboxText(msg);
        }

        private void SetTextboxText(string msg)
        {
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
