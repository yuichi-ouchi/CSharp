using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebScraipingSample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void GetButton_Click(object sender, EventArgs e)
        {

            StatusLable.Visible = true;
            StatusLable.Text = "取得中";
            StatusLable.BringToFront();
            StatusLable.Update();

            var scr = new SampleScraping();
            string url = UrlText.Text;
            string html = scr.GetHtml(url);
            HtmlText.Text = html;

            string title = scr.GetTitle(html);
            TitleText.Text = title;

            //
            StatusLable.Visible = false;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string url = UrlText.Text;
            var result = ScraipingTest.GetScr(url);
            this.HtmlText.Text = result;
        }
    }
}
