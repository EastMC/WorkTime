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
        private static Dictionary<DateTime, int> workCalendar = new Dictionary<DateTime, int>();        
        private static string url;

        public string Url
        {
            set
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(value);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    url = value;
                }
                else url = "";
            }
            get { return url; }            
        }

        static WorkCalendar()
        {
        }
        public WorkCalendar()
        {
            ParseHTML(GetHTMLFromWebSite("https://calendar.yoip.ru/work/2020-proizvodstvennyj-calendar.html"));            
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
                //Console.WriteLine(HTMLCode);
                response.Close();
                readStream.Close();
            }
            return HTMLCode;
        }

        private static async void ParseHTML(string _HTML)
        {
            var config = Configuration.Default;
            var context = BrowsingContext.New(config);
            var source = _HTML;
            var document = await context.OpenAsync(req => req.Content(source));
            var days = document.All.Where(m => m.ClassList.Contains("_hd") || m.ClassList.Contains("_wd"));
            FillCalendarFromParsedHTML(days);
        }

        private static void FillCalendarFromParsedHTML(IEnumerable<AngleSharp.Dom.IElement> _parsedHTML)
        {
            int[,] workHoursMatrix = new int[12, 31];
            int lastDay = 0;
            int currentMonth = 0;
            foreach (var item in _parsedHTML)
            {
                int currentDay = Convert.ToInt32(item.TextContent) - 1;
                int currentWorkHours = GetWorkHoursFromParsedHTMLClassName(item.ClassName);
                if (currentDay < lastDay)
                {
                    currentMonth++;
                }
                workHoursMatrix[currentMonth, currentDay] = currentWorkHours;
                lastDay = currentDay;
            }

            FillCalendarFromHourMatrix(workHoursMatrix);
        }

        private static void FillCalendarFromHourMatrix(int[,] _workHoursMatrix)
        {
            workCalendar = new Dictionary<DateTime, int>();
            DateTime currentDate = new DateTime(DateTime.Now.Year, 1, 1);
            for (int i = 0; i < _workHoursMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < _workHoursMatrix.GetLength(1); j++)
                {
                    if (currentDate.Month == i + 1 && currentDate.Day == j + 1)
                    {
                        workCalendar.Add(currentDate, _workHoursMatrix[i, j]);
                        currentDate = currentDate.AddDays(1);
                    }
                    else continue;
                }
            }

            //foreach(object o in workCalendar.Keys)
                //Console.WriteLine($"{o} {workCalendar[(DateTime)o]}");


        }

        private static int GetWorkHoursFromParsedHTMLClassName(string _className)
        {
            int workHours = 0;
            if (_className.Contains("_hd")) workHours = 0;
            else if (_className.Contains("_wd active")) workHours = 7;
            else workHours = 8;
            return workHours;
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
