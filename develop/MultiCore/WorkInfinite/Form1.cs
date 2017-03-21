﻿using System;
using System.Diagnostics;
using System.Reflection;
using System.Timers;
using System.Windows.Forms;

namespace WorkInfinite
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public bool CanStart { get { return !IsRunning; } }
        public bool CanStop { get { return IsRunning; } }

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

        private void StartButton_Click(object sender, EventArgs e)
        {
            this.StartButton.Enabled = false;
            this.StopButton.Enabled = true;

            WorkInfinite();
        }

        private bool IsRunning;
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

                        r = (double)i / (double)j;
                        Application.DoEvents();
                    }
                }
            }
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
        private void RunAsync()
        {
            var asm = Assembly.GetEntryAssembly();
            var psi = new ProcessStartInfo(asm.Location);
            psi.Arguments = "計算しろ!";
            Process.Start(psi);

        }

        private System.Timers.Timer _timer;
        private void StartTimer()
        {
            _timer = new System.Timers.Timer();
            _timer.SynchronizingObject = this;

            if (this.components == null)
            {
                this.components = new System.ComponentModel.Container();
            }
            this.components.Add(_timer);

            _timer.Elapsed += new ElapsedEventHandler(OnTimerEvent);
            _timer.Interval = 1000;
            _timer.Start();
        }

        public void OnTimerEvent(object sender, ElapsedEventArgs e)
        {
            this.label1.Text = string.Format("現在時刻: {0}秒", e.SignalTime.Second);
        }

    }
}