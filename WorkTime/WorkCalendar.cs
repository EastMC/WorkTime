using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace WorkTime
{
    enum DayType
    {
        FullWork = 8,
        ShortWork = 7,
        Weekend = 0
    }
    class WorkCalendar
    {
        public WorkCalendar()
        {
            Parse(GetHTMLFromWebSite("https://calendar.yoip.ru/work/2020-proizvodstvennyj-calendar.html"));
        }

        private string GetHTMLFromWebSite(string _url)
        {
            string HTMLCode = string.Empty;

            string urlAddress = _url;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (String.IsNullOrWhiteSpace(response.CharacterSet))
                    readStream = new StreamReader(receiveStream);
                else
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));

                HTMLCode = readStream.ReadToEnd();

                response.Close();
                readStream.Close();
            }
            MessageBox.Show(HTMLCode);
            return HTMLCode;
        }

        private static async void Parse(string _HTML)
        {
            var config = Configuration.Default;
            var context = BrowsingContext.New(config);
            var source = _HTML;
            var document = await context.OpenAsync(req => req.Content(source));
            MessageBox.Show(document.DocumentElement.OuterHtml);
        }


        DateTime GetWorkDateTimeFromDate(DateTime date)
        {
            DateTime WorkDayTime = new DateTime();



            return WorkDayTime;
        }


        DateTime GetWorkDayTimeForToday()
        {
            DateTime TodayWorkDayTime = new DateTime();



            return TodayWorkDayTime;
        }
    }
}
