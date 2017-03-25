using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace DoCoordination
{
    public partial class RequestForm : Form,INotifyPropertyChanged
    {
        public RequestForm()
        {
            InitializeComponent();
        }


        const string WorkFileName = "workfile.txt";
        private string WorkFilePath
        {
            get {
                return Path.Combine(Environment.CurrentDirectory, WorkFileName);
            }
        }

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

        private void AppendButton_Click(object sender, EventArgs e)
        {
            AppendNewLine();
            DisplayWorkFile();
            RunAsync();
        }

        private void DisplayWorkFile()
        {
            try
            {
                this.DisplayString = File.ReadAllText(this.WorkFilePath);
            }
            catch (IOException)
            {
            }
        }

        private int _count;
        private void AppendNewLine()
        {
            _count++;
            using (var writer = new StreamWriter(WorkFilePath, true))
            {
                writer.Write("{0},{1}", _count, _count + 1);
            }
        }

        private void DoWork()
        {
            using (FileStream fs = File.Open(WorkFilePath, FileMode.Open,
                                            FileAccess.ReadWrite, FileShare.Read))
            {
                using (TextReader reader = new StreamReader(fs))
                {
                    var records = new List<string>();
                    while (reader.Peek() >= 0)
                        records.Add(reader.ReadLine());

                    int targetIndex = FindTargetRecord(records);
                    if (targetIndex < 0)
                        return;
                    
                    DisplayString = string.Format("処理中: {0}", records[targetIndex]);
                    Application.DoEvents();

                    int result = this.Calc("aaa"); //時間のかかる処理
                    records[targetIndex] = string.Format("{0},{1}", records[targetIndex], result);
                    fs.Position = 0; //先頭シーク
                    fs.SetLength(0); //ファイル内容の切り捨て
                    using (TextWriter writer = new StreamWriter(fs))
                    {
                        foreach (var line in records)
                        {
                            writer.WriteLine(line); //全レコード書き出し
                        }
                    }
                    DisplayString = string.Format("処理完了: {0}", records[targetIndex]);
                }
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

        private int FindTargetRecord(object redords)
        {
            throw new NotImplementedException();
        }

        private void RunAsync()
        {
            var psi = new ProcessStartInfo(ThisAssemblyPath);
            psi.Arguments = "計算しろ!";

            var p = new Process();
            p.StartInfo = psi;
            p.EnableRaisingEvents = true;
            p.Exited += OnProcessExited;
            p.SynchronizingObject = this;
            p.Start();
        }
        private string ThisAssemblyPath
        {

            get {
                var asm = Assembly.GetEntryAssembly();
                return asm.Location;
            }
        }


        private void OnProcessExited(object sender, EventArgs e)
        {
            StopTimer();

            var p = sender as Process;
            DisplayString = string.Format("非同期処理終了: {0}", p.ExitCode);
        }

        private System.Timers.Timer _timer;
        private void StartTimer()
        {
            _timer = new System.Timers.Timer();
            _timer.SynchronizingObject = this;

            if (this.components == null)
                this.components = new Container();
            this.components.Add(_timer);

            //_timer.Elapsed += new ElapsedEventHandler(OnTimerEvent);
            _timer.Interval = 1000;
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
            if (Environment.GetCommandLineArgs().Length > 1)
            {

            }
            else
            { 
                InitalizeWorkFile();
            }
        }

        private void InitalizeWorkFile()
        {
            File.Open(this.WorkFilePath, FileMode.Create);
        }
        private Stream OpenWorkfileWithRetry()
        {
            const int RetryCountMax = 5;
            const int RetryInterval = 100;

            FileStream fs = null;
            for (int retryCount = RetryCountMax; retryCount > 0, retryCount--)
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
    }
}
