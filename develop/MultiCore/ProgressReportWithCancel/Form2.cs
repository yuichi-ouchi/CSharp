using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgressReportWithCancel
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            label1.Text = String.Empty;
            SetProgressValue(new ProgressReport {
                Percent = 0,
            });
            CancelButton.Enabled = false;
        }

        private void SetProgressValue(ProgressReport report)
        {
            this.progressBar1.Value = report.Percent;
            if(0 < report.Percent && report.Percent < 100)
            {
                label1.Text = string.Format("あと{0:0.0}秒…", report.EstimatedRemain.TotalSeconds);
            }
        }

        private CancellationTokenSource _cts;
        private async void ButtonStart_Click(object sender, EventArgs e)
        {
            ButtonStart.Enabled = false;
            CancelButton.Enabled = true;
            label1.ForeColor = System.Drawing.Color.DodgerBlue;
            label1.Text = "実行中…";

            //デリゲートを渡してIProgress<T>を生成
            // ※UIスレッド上でnew しなければならない
            var progress = new Progress<ProgressReport>(SetProgressValue);

            // キャンセルトークン生成
            _cts = new CancellationTokenSource();
            CancellationToken token = _cts.Token;
            try
            {
                // ワーカースレッドの起動
                await DoWorkAsync(progress,token);
                
                // ワーカースレッド完了後
                label1.ForeColor = System.Drawing.Color.DarkRed;
                label1.Text = "完了";
            }
            catch (OperationCanceledException)
            {
                
                label1.ForeColor = System.Drawing.Color.DarkRed;
                label1.Text = "中断";
            }
            CancelButton.Enabled = false;
            ButtonStart.Enabled = true;

        }

        //スレッドプールで実行する処理
        private async Task DoWorkAsync(IProgress<ProgressReport> progress, CancellationToken token)
        {
            const int Count = 10;
            const int PeriodMSec = 500;
            for (int i = 0; i < Count; i++)
            {
                await Task.Delay(PeriodMSec); //時間のかかる処理

                if (token.IsCancellationRequested) //中断要求をチェック
                {
                    Thread.Sleep(1000); //中断するための処理
                    //例外をスローして終了
                    token.ThrowIfCancellationRequested();
                }

                progress.Report(new ProgressReport()
                {
                    Percent = (i + 1) * Count,
                    EstimatedRemain = TimeSpan.FromMilliseconds((Count -1 - i) * PeriodMSec),
                });
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {

            _cts.Cancel();

            CancelButton.Enabled = false;
            label1.ForeColor = System.Drawing.Color.DarkOrange;
            label1.Text = "中断処理中…";
        }
    }
}
