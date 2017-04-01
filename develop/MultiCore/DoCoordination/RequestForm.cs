using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

namespace DoCoordination
{
    public partial class RequestForm : Form,INotifyPropertyChanged
    {
        #region " Constractor "
        public RequestForm()
        {
            InitializeComponent();
            //InitalizeWorkFile();
        }
        private void InitalizeWorkFile()
        {
            File.Open(this.WorkFilePath, FileMode.Create).Dispose();
        }
        #endregion

        const string WorkFileName = "workfile.txt";
        private string WorkFilePath
        {
            get {
                return Path.Combine(Environment.CurrentDirectory, WorkFileName);
            }
        }

        #region " Properties "
        private string _displayString;
        public string DisplayString
        {
            get { return _displayString; }
            private set
            {
                if (value != _displayString)
                {
                    _displayString = value;
                    NotifyPropertyChanged("DisplayString");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region " common methods "

        private bool IsMainProcess
        {
            get
            {
                return (Environment.GetCommandLineArgs().Length == 1);
            }
        }

        private Stream OpenWorkfileWithRetry()
        {
            const int RetryCountMax = 5;
            const int RetryInterval = 100;

            FileStream fs = null;
            for (int retryCount = RetryCountMax; retryCount > 0; retryCount--)
            {
                try
                {
                    fs = File.Open(this.WorkFilePath, FileMode.Open);
                    return fs;
                }
                catch (IOException)
                {
                    if (fs != null)
                    {
                        fs.Dispose();
                        fs = null;
                    }
                }
                Thread.Sleep(RetryInterval);
            }
            return null;
        }

        private void RequestForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseAllProcss();
            StopTimer();
        }
        #endregion

        #region " request side methods "
        List<Process> _processList = new List<Process>();

        private void AppendButton_Click(object sender, EventArgs e)
        {
            AppendNewLine();
            DisplayWorkFile();
        }

        private int _count;
        private void AppendNewLine()
        {
            Stream fs = OpenWorkfileWithRetry();
            if (fs == null)
                return;

            _count++;
            using (fs)
            {
                fs.Seek(0, SeekOrigin.End);
                using (TextWriter writer = new StreamWriter(fs))
                    writer.WriteLine("{0},{1}", _count, _count + 1);
            }
        }

        private string ThisAssemblyPath
        {
            get
            {
                var asm = Assembly.GetEntryAssembly();
                return asm.Location;
            }
        }

        private void OnProcessExited(object sender, EventArgs e)
        {
            StopTimer();

            Process p = sender as Process;
            DisplayString = string.Format("非同期処理終了: {0}", p.ExitCode);
        }

        private void CloseAllProcss()
        {
            foreach (var p in _processList)
            {
                if (p != null && !p.HasExited)
                {
                    p.CloseMainWindow();
                }
            }
        }

        #endregion

        #region " coordination methods "

        private Point GetStartLocation()
        {
            int x = int.Parse(Environment.GetCommandLineArgs()[1]);
            int y = int.Parse(Environment.GetCommandLineArgs()[2]);
            return new Point(x, y);
        }

        private void DisplayWorkFile()
        {
            try
            {
                this.DisplayString = File.ReadAllText(this.WorkFilePath);
            }
            catch (IOException)
            {
                //(void)
            }
        }
        
        /// <summary>
        /// 単体テスト可能にするためinternalアクセス
        /// </summary>
        internal void DoWork()
        {

            int targetIndex;
            string targetRecord = GetTargetRecord(out targetIndex);
            if (targetRecord == null)
                return; //未処理の行はなかった。またはファイルオープン失敗

            this.DisplayString = string.Format("処理中：{0}", targetRecord);
            Application.DoEvents();


            int result = this.Calc(targetRecord); //時間のかかる処理
            string resultRecord = string.Format("{0},{1}", targetRecord, result);

            //ファイルに書き出し
            if (!WriteWorkfile(targetIndex, resultRecord))
            {
                this.DisplayString = string.Format("書き出し失敗: {0}", resultRecord);
                return;
            }
            this.DisplayString = string.Format("処理完了: {0}", resultRecord);
        }

        private bool WriteWorkfile(int targetIndex, string resultRceord)
        {
            Stream fs = OpenWorkfileWithRetry();
            if (fs == null)
                return false; // 計算が無駄になるがあきらめる

            using (fs)
            using (TextReader reader = new StreamReader(fs))
            {
                List<string> newRecords = new List<string>();
                while (reader.Peek() >= 0)
                    newRecords.Add(reader.ReadLine()); //全レコードを読み込む

#if DEBUG
                //該当レコードをチェック
                string[] items = newRecords[targetIndex].Split(new char[] { ',', });
                if (items.Length > 2)
                    throw new ApplicationException(
                        string.Format("すでに計算済みの行です:{0}", newRecords[targetIndex])
                        );
#endif
                //計算結果
                newRecords[targetIndex] = resultRceord;
                fs.Position = 0;
                fs.SetLength(0);
                using (TextWriter writer = new StreamWriter(fs))
                {
                    foreach (var line in newRecords)
                    {
                        writer.WriteLine(line);
                    }
                }
            }
            return true;
        }

        private string GetTargetRecord(out int targetIndex)
        {
            targetIndex = -1;

            List<string> records = ReadWorkfile();
            if (records == null) //ファイルが空、またはオープン失敗
                return null;

            targetIndex = FindTargetRecord(records); //未処理の行を探す
            if (targetIndex < 0)
                return null; //未処理の行なし

            return records[targetIndex];
        }

        private List<string> ReadWorkfile()
        {
            Stream fs = OpenWorkfileWithRetry();
            if (fs == null)
                return null;

            using (fs)
            using (TextReader reader = new StreamReader(fs))
            {
                List<string> records = new List<string>();
                while (reader.Peek() >= 0)
                    records.Add(reader.ReadLine());
                return records;
            }
        }

        private int Calc(string record)
        {
            string[] items = record.Split(new char[] { ',', });
            int x = int.Parse(items[0]);
            int y = int.Parse(items[1]);

            Thread.Sleep(1000);

            return x + y;
        }

        private int FindTargetRecord(List<string> records)
        {
            for (int i = 0; i < records.Count; i++)
            {
                string[] items = records[i].Split(new char[] { ',', });
                if (items.Length == 2) //未処理の行
                    if ((int.Parse(items[0]) % divisor) == remainder)
                        return i;
            }
            return -1;
        }

        private void RunAsync()
        {
            const int ProcessCount = 2;
            for (int i = 0; i < ProcessCount; i++)
            {
                var psi = new ProcessStartInfo(ThisAssemblyPath);
                //psi.Arguments = String.Format("{0} {1}", this.Location.X + this.Width, this.Location.Y);
                psi.Arguments = string.Format("{0} {1} {2} {3}",
                    this.Location.X + this.Width * (i + 1),
                    this.Location.Y,
                    ProcessCount,
                    i
                    );

                var p = new Process();
                p.StartInfo = psi;
                p.EnableRaisingEvents = true;
                p.Exited += new EventHandler(OnProcessExited);
                p.SynchronizingObject = this;
                p.Start();

                this._processList.Add(p);
            }

        }
        #endregion
        
        #region " Timer "
        private System.Timers.Timer _timer;

        private void StartTimer(ElapsedEventHandler timerEventHandler, int interval)
        {
            if (_timer != null)
                return;

            _timer = new System.Timers.Timer();
            _timer.SynchronizingObject = this;

            if (this.components == null)
                this.components = new Container();
            this.components.Add(_timer);

            _timer.Elapsed += new ElapsedEventHandler(timerEventHandler);
            _timer.Interval = interval;
            _timer.Start();
        }

        public void OnTimerEventMain(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();
            DisplayWorkFile();
            _timer.Start();
        }

        public void OnTimerEventAsync(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();
            DoWork();
            _timer.Start();
        }

        private void StopTimer()
        {
            if (_timer == null)
                return;

            _timer.Stop();
            _timer.Dispose();
            _timer = null;
        }

        private void RequestForm_Load(object sender, EventArgs e)
        {
            this.textBox1.DataBindings.Add("Text", this, "DisplayString");

#if TEST_01
            divisor = 2; //割る数
            remainder = 1; //剰余
            DoWork();
            return;

#endif
            if (IsMainProcess)
            {
                this.Text = "依頼する側";
                this.Show();
                RunAsync();
                StartTimer(new ElapsedEventHandler(OnTimerEventMain), 1000);
            }
            else
            {
                this.Text = string.Format("非同期処理 - {0}", Process.GetCurrentProcess().Id);
                this.AppendButton.Enabled = false;
                this.ControlBox = false;
                this.StartPosition = FormStartPosition.Manual;
                this.Location = GetStartLocation();
                GetTargetPattern();
                //タイマーの時間より短いと再入が発生する想定
                StartTimer(new ElapsedEventHandler(OnTimerEventAsync), 1000);
            }
        }

        internal int divisor; //割る数
        internal int remainder; //剰余
        private void GetTargetPattern()
        {
            this.divisor = int.Parse(Environment.GetCommandLineArgs()[3]);
            this.remainder = int.Parse(Environment.GetCommandLineArgs()[4]);
        }
#endregion
    }
}
