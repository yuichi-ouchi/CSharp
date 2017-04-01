using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebScraipingSample
{
    class SampleScraping
    {

        public string GetHtml(string url)
        {
            var req = (HttpWebRequest)WebRequest.Create(url);
            var html = "";

            using (var res = (HttpWebResponse)req.GetResponse())
            {
                using(var resStr = res.GetResponseStream())
                {
                    using(var sr = new StreamReader(resStr, Encoding.UTF8))
                    {
                        html = sr.ReadToEnd();
                    }
                    return html;
                }
            }
        }

        public string GetTitle(string html)
        {
            var reg = new Regex(@"<title>(?<title>.*?)</title>",
                RegexOptions.IgnoreCase | RegexOptions.Singleline
                );
            var m = reg.Match(html);
            return m.Groups["title"].Value;
        }
    }
}
