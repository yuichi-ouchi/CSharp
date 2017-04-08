using System;
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
        }

        private void SetProgressValue(ProgressReport report)
        {
            this.progressBar1.Value = report.Percent;
            if(0 < report.Percent && report.Percent < 100)
            {
                label1.Text = string.Format("あと{0:0.0}秒…", report.EstimatedRemain.TotalSeconds);
            }
        }

        private async void ButtonStart_Click(object sender, EventArgs e)
        {
            ButtonStart.Enabled = false;
            label1.ForeColor = System.Drawing.Color.DodgerBlue;
            label1.Text = "実行中…";

            //デリゲートを渡してIProgress<T>を生成
            // ※UIスレッド上でnew しなければならない
            var progress = new Progress<ProgressReport>(SetProgressValue);
            // ワーカースレッドの起動
            await DoWorkAsync(progress);

            // ワーカースレッド完了後
            label1.ForeColor = System.Drawing.Color.DarkRed;
            label1.Text = "完了";
            ButtonStart.Enabled = true;

        }

        //スレッドプールで実行する処理
        private async Task DoWorkAsync(IProgress<ProgressReport> progress)
        {
            const int Count = 10;
            const int PeriodMSec = 500;
            for (int i = 0; i < Count; i++)
            {
                await Task.Delay(PeriodMSec); //時間のかかる処理

                progress.Report(new ProgressReport()
                {
                    Percent = (i + 1) * Count,
                    EstimatedRemain = TimeSpan.FromMilliseconds((Count -1 - i) * PeriodMSec),
                });
            }
        }

    }
}
