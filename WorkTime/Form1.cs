using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;



namespace WorkTime
{
    public partial class Form1 : Form
    {
        DateTime came;
        Time toGo;
        bool GO;
        WorkCalendar WC = new WorkCalendar();
        Time weekTimeLeft = new Time();
        Time weekTimeLeftCount = new Time();
        Timer mainTimer = new Timer();
        
        public Form1()
        {
            InitializeComponent();
            this.Text = DateTime.Today.ToLongDateString();
            mainTimer.Interval = 500;
            mainTimer.Tick += T_Tick;
            //TopMost = true;
            GO = false;

            WC.InitializeCalendar("https://calendar.yoip.ru/work/2020-proizvodstvennyj-calendar.html");
            weekTimeLeft = GetLeftWeekTime();                  

        }

        private Time GetLeftWeekTime()
        {
            try
            {
                if (IsWorkWeekStart())
                {
                    return WC.GetThisWeekWorkTime();
                }
                else
                {
                    using (StreamReader sr = new StreamReader("config.txt"))
                    {
                        string read = sr.ReadLine();
                        string leftWorkTimeString = string.Empty;
                        while (read != null)
                        {
                            leftWorkTimeString = read;
                            read = sr.ReadLine();
                        }
                        return new Time(Convert.ToInt32(leftWorkTimeString.Split(':')[0]),
                            Convert.ToInt32(leftWorkTimeString.Split(':')[1]),
                            Convert.ToInt32(leftWorkTimeString.Split(':')[2]));
                    }
                }
            }
            catch
            {
                MessageBox.Show("Отсутствует файл \"config.txt\" или в " +
                    "существующем файле некорректная запись оставшегося рабочего времени.");
                this.Close();
            }

            return new Time();
        }

        private bool IsWorkWeekStart()
        {
            DateTime currentDate = DateTime.Now;
            if (WC.GetWorkDayTimeForToday() == 0) return false;

            bool answer = true;
            while (currentDate.DayOfWeek != DayOfWeek.Monday)
            {
                currentDate = currentDate.AddDays(-1);
                if (WC.GetWorkDateTimeFromDate(currentDate) > 0)
                {
                    answer = false;
                    break;
                }
            }
            return answer;
        }

        private void ReadURLFromIni(string _path)
        {

        }
               

        private void T_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            labelTimeNow.Text = now.ToLongTimeString();
            int secondsFromCame = now.Hour * 60 * 60 + now.Minute * 60 + now.Second -
                (came.Hour * 60 * 60 + came.Minute * 60 + came.Second);
            weekTimeLeftCount = weekTimeLeft - new Time(0, 0, secondsFromCame);
            labelTimeWeekLeft.Text = weekTimeLeftCount.ToString();


            if (Time.Now() >= toGo && !buttonCame.Enabled && !GO)
            {
                labelTimeGo.BackColor = Color.Green;
                labelTimeGo.ForeColor = Color.Yellow;
                labelGo.BackColor = Color.Green;
                labelGo.ForeColor = Color.Yellow;

                NotifyIcon NI = new NotifyIcon();
                NI.BalloonTipText = "Андрюха, у нас труп! Возможно, криминал!";
                NI.BalloonTipTitle = "По коням!";
                NI.BalloonTipIcon = ToolTipIcon.Info;
                NI.Icon = this.Icon;
                NI.Visible = true;
                NI.ShowBalloonTip(1000);
                GO = true;

            }
        }

        private void ButtonCame_Click(object sender, EventArgs e)
        {
            int h = 0;       
            Int32.TryParse(maskedTextBoxCame.Text.Split(':')[0], out h);
            int m = 0;
            Int32.TryParse(maskedTextBoxCame.Text.Split(':')[1], out m);

            DateTime now = DateTime.Now;
            came = new DateTime(now.Year, now.Month, now.Day, h, m, 0);
            toGo = new Time(h + WC.GetWorkDayTimeForToday(), m, 0);
            labelTimeGo.Text = toGo.ToString();

            buttonCame.Enabled = false;
            buttonGone.Enabled = true;
            maskedTextBoxCame.Enabled = false;

            string H = (h >= 10) ? h.ToString() : "0" + h.ToString();
            string M = (m >= 10) ? m.ToString() : "0" + m.ToString();

            AddConfig(this.Text + " " + H + ":" + M);

            mainTimer.Start();

        }
              

        private void AddConfig(string _text)
        {
            using (StreamWriter sw = new StreamWriter("config.txt", true, System.Text.Encoding.Default))
            {
                sw.WriteLine(_text);
            }
        }



        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
        }

        private void maskedTextBoxCame_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                ButtonCame_Click(this, new EventArgs());
            }
        }

        private void SaveGoneTime()
        {
            int h = DateTime.Now.Hour;
            int m = DateTime.Now.Minute;
            string H = (h >= 10) ? h.ToString() : "0" + h.ToString();
            string M = (m >= 10) ? m.ToString() : "0" + m.ToString();
            AddConfig(this.Text + " " + H + ":" + M);
            AddConfig(weekTimeLeftCount.ToString());
        }

        private void buttonGone_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            SaveGoneTime();
        }
    }
}
