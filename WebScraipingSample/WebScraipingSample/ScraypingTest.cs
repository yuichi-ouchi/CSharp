using ScrapySharp.Extensions;
using ScrapySharp.Network;
using System;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraipingSample
{
    class ScraipingTest
    {

        public static string GetScr(string uriString)
        {
            var browser = new ScrapingBrowser();
            browser.AllowAutoRedirect = true;
            browser.AllowMetaRedirect = true;
            try
            {
                WebPage pageResult = browser.NavigateToPage(new Uri("http://yahoo.co.jp/page"));
                var str = pageResult.Html.CssSelect("tr td").First().InnerText;
                //pageResult.Html.CssSelect("ul li").First(elem => elem.InnerText.Contains("hhh")).InnerText;
                return str;
            }
            catch (Exception)
            {

                throw;
            }


        }
    }
}
