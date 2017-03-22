﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Timers;
using System.Windows.Forms;

namespace WorkInfinite
{
    public partial class Form1 : Form
    {
        #region " Constractor "
        public Form1()
        {
            InitializeComponent();
        }
        #endregion

        #region " Properties "
        public bool CanStart { get { return !IsRunning; } }
        public bool CanStop { get { return IsRunning; } }
        #endregion

        #region " Fields "

        private int _currentJ;

        private int _count;

        private bool IsRunning;

        private System.Timers.Timer _timer;
        private string WorkFilePath = "Z:\\work.txt";

        #endregion

        #region " Events "

        private void Form1_Load(object sender, EventArgs e)
        {
            this.StartButton.Enabled = true;
            this.StopButton.Enabled = false;

            string[] args = Environment.GetCommandLineArgs();
            if(args.Length > 1 && args[1] == "計算しろ!")
            {
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
            this.StartButton.Enabled = false;
            this.StopButton.Enabled = true;

            WorkInfinite();
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            this.IsRunning = false;

            this.StartButton.Enabled = true;
            this.StopButton.Enabled = false;
        }

        private void AsyncStart_Click(object sender, EventArgs e)
        {
            StartTimer();
            RunAsync();

        }
        #endregion

        #region " Methods "
        private void WorkInfinite()
        {
            this.IsRunning = true;

            double r = 0.0;
            while (true)
            {
                for (var i = 1; i <= int.MaxValue; i++)
                {
                    for (int j = 1; j < int.MaxValue; j++)
                    {
                        if (!this.IsRunning)
                            return;

                        _currentJ = j;
                        r = (double)i / (double)j;
                        Application.DoEvents();
                    }
                }
            }
        }

        private void RunAsync()
        {
            var asm = Assembly.GetEntryAssembly();
            var psi = new ProcessStartInfo(asm.Location);
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
            var p = sender as Process;
            this.label1.Text = string.Format("非同期処理終了: {0}", p.ExitCode);
        }

        private void StartTimer()
        {
            _timer = new System.Timers.Timer();
            _timer.SynchronizingObject = this;

            if (this.components == null)
                this.components = new System.ComponentModel.Container();
            this.components.Add(_timer);

            _timer.Elapsed += new ElapsedEventHandler(OnTimerEvent);
            _timer.Interval = 1000;
            _timer.Start();
        }

        public void OnTimerEvent(object sender, ElapsedEventArgs e)
        {
            this.label1.Text = string.Format("現在時刻: {0}秒", e.SignalTime.Second);
        }

        private void AppendNewLine()
        {
            _count++;
            using (var writer = new StreamWriter(this.WorkFilePath, true))
            {
                writer.Write("{0},{1}", _count, _count + 1);
            }
        }

        private void DoWork()
        {
            using (FileStream fs = File.Open(this.WorkFilePath, FileMode.Open, 
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



                }
            }
        }

        private int FindTargetRecord(object redords)
        {
            throw new NotImplementedException();
        }
        
        #endregion

    }
}
