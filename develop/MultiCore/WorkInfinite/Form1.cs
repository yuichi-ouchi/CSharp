using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

namespace WorkInfinite
{
    public partial class Form1 : Form, INotifyPropertyChanged
    {
        #region " Constractor "
        public Form1()
        {
            InitializeComponent();
        }
        #endregion

        #region " Properties "

        private bool _isRunning;
        public bool IsRunning
        {
            get { return _isRunning; }
            private set
            {
                if (value != _isRunning)
                {
                    _isRunning = value;
                    NotifyRunningPropertiesChanged();
                }
            }
        }

        public bool CanStart { get { return !_isRunning; } }

        public bool CanStop { get { return _isRunning; } }

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
        private void NotifyRunningPropertiesChanged()
        {
            NotifyPropertyChanged("IsRunning");
            NotifyPropertyChanged("CanStart");
            NotifyPropertyChanged("CanStop");
        }

        #endregion

        #region " Events "

        private void Form1_Load(object sender, EventArgs e)
        {
            this.StartButton.DataBindings.Add("Enabled", this, "CanStart");
            this.StopButton.DataBindings.Add("Enabled", this, "CanStop");
            this.label1.DataBindings.Add("Text", this, "DisplayString");

            string[] args = Environment.GetCommandLineArgs();
            if(args.Length > 1 && args[1] == "計算しろ!")
            {
                this.Text += string.Format(" - {0}", Process.GetCurrentProcess().Id);
                this.AsyncStart.Enabled = false;
                this.Show();
                Application.DoEvents();

                // [StartButtonの処理]
                WorkInfinite();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.IsRunning = false;
            Environment.ExitCode = this._currentJ; //終了コード
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            WorkInfinite();
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AsyncStart_Click(object sender, EventArgs e)
        {
            StartTimer();
            RunAsync();
        }
        #endregion

        #region " Methods "       

        private int _currentJ;
        private void WorkInfinite()
        {
            this.IsRunning = true;

            double r = 0.0;
            while (true)
            {
                for (var i = 1; i <= int.MaxValue; i++)
                {
                    DisplayString = i.ToString("#,##0");
                    Application.DoEvents();

                    for (int j = 1; j <= int.MaxValue; j++)
                    {
                        _currentJ = j;

                        if (!this.IsRunning)
                            return;

                        r = (double)i / (double)j;
                        Application.DoEvents();
                    }
                }
            }
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

            _timer.Elapsed += new ElapsedEventHandler(OnTimerEvent);
            _timer.Interval = 1000;
            _timer.Start();
        }
        private string ThisAssemblyPath
        {

            get {
                Assembly asm = Assembly.GetEntryAssembly();
                return asm.Location;
            }
        }

        private void StopTimer()
        {
            if (_timer == null)
                return;

            _timer.Stop();
            _timer.Dispose();
            _timer = null;
    

        }

        public void OnTimerEvent(object sender, ElapsedEventArgs e)
        {
            DisplayString = string.Format("現在時刻: {0}秒", e.SignalTime.Second);
        }

        
        #endregion

    }
}
