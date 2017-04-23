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
        private DateTime _start;
        private DateTime _stop;

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            var bw = new BackgroundWorker();
            bw.WorkerReportsProgress = true;
        }
    }
}
