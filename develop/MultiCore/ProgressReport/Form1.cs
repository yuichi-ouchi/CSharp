using System;
using System.Threading;
using System.Windows.Forms;

namespace ProgressReportWithCancel
{
    public partial class Form1 : Form
    {
        private delegate void SetProgressValueDelegate(int percent);
        public Form1()
        {
            InitializeComponent();
            label1.Text = String.Empty;
            SetProgressValue(0);
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

        private void StartButton_Click(object sender, EventArgs e)
        {
            this.StartButton.Enabled = false;

            // デリゲートを渡して、ワーカースレッドを起動
            ThreadPool.QueueUserWorkItem(new WaitCallback(DoWork), new SetProgressValueDelegate(SetProgressValue));
            label1.ForeColor = System.Drawing.Color.DodgerBlue;
            label1.Text = "実行中…";
        }

        // スレッドプールで実行
        private void DoWork(object state)
        {
            SetProgressValueDelegate progress = state as SetProgressValueDelegate;
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(500); //時間のかかる処理
                progress.Invoke((i + 1) * 10);
            }
        }
    }
}
