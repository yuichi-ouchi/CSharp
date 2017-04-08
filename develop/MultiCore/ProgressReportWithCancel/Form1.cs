using System;
using System.Threading;
using System.Windows.Forms;

namespace ProgressReportWithCancel
{
    public partial class Form1 : Form
    {
        private delegate void SetProgressValueDelegate(int percent);
        class DoWorkParams
        {
            public SetProgressValueDelegate SetProgressValueDelagete;
            public CancellationToken CancellationToken;
            public int CancelledFlag = 0;
        }
        private DoWorkParams _doWorkParams;

        public Form1()
        {
            InitializeComponent();
            label1.Text = String.Empty;
            SetProgressValue(0);
            this.CancelButton.Enabled = false;
        }

        private void SetProgressValue(int percent)
        {
            //別スレッドから呼び出せるよう対策
            if (this.InvokeRequired)
            {
                this.Invoke(new SetProgressValueDelegate(SetProgressValue), percent);
            }
            else
            {
                this.progressBar1.Value = percent;
                //簡易的に100%で処理終了と判定
                if (percent >= 100)
                {
                    label1.ForeColor = System.Drawing.Color.DarkBlue;
                    label1.Text = "完了";
                    StartButton.Enabled = true;
                }
            }
        }

        private CancellationTokenSource _cts;
        private void StartButton_Click(object sender, EventArgs e)
        {
            this.StartButton.Enabled = false;
            progressBar1.Value = 0;

            _cts = new CancellationTokenSource();
            _doWorkParams = new DoWorkParams()
            {
                SetProgressValueDelagete = new SetProgressValueDelegate(SetProgressValue),
                CancellationToken = _cts.Token,
            };

            // デリゲートを渡して、ワーカースレッドを起動
            ThreadPool.QueueUserWorkItem(new WaitCallback(DoWork), _doWorkParams);
            label1.ForeColor = System.Drawing.Color.DodgerBlue;
            label1.Text = "実行中…";
            CancelButton.Enabled = true;
        }

        // スレッドプールで実行
        private void DoWork(object state)
        {
            DoWorkParams param = state as DoWorkParams;
            SetProgressValueDelegate progress = param.SetProgressValueDelagete;
            CancellationToken token = param.CancellationToken;
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(500); //時間のかかる処理
                progress.Invoke((i + 1) * 10);

                if (token.IsCancellationRequested)
                {
                    Thread.Sleep(1000); // 中断する処理
                    //フラグを立てて処理を抜ける
                    Interlocked.Increment(ref param.CancelledFlag);
                    break;
                }
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            CancelButton.Enabled = false;
            //中断要求
            _cts.Cancel();
            label1.ForeColor = System.Drawing.Color.DarkOrange;
            label1.Text = "中断処理中…";

            //処理の完了を待つ
            while (_doWorkParams.CancelledFlag == 0)
                Application.DoEvents();

            label1.ForeColor = System.Drawing.Color.DarkRed;
            label1.Text = "中断";
            StartButton.Enabled = true;
        }
    }
}
