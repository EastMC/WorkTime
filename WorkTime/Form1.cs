using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace WorkTime
{
    public partial class Form1 : Form
    {
        private DateTime came;
        private Time toGo;
        private bool isItTimeToGo = false;
        private WorkCalendar WC = new WorkCalendar();
        private Time weekTimeLeft = new Time();
        private Time weekTimeLeftCount = new Time();
        private Timer mainTimer = new Timer();
        private ToolTip buttonSettingsHoverTip = new ToolTip();
        private Settings settings;
        private const string journalFilePath = "journal.txt";


        public Form1()
        {
            InitializeComponent();
            settings = new Settings(this);
            buttonSettingsHoverTip.SetToolTip(ButtonSettings, "Изменить параметры");
            this.Text = DateTime.Today.ToLongDateString();
            mainTimer.Interval = 500;
            mainTimer.Tick += T_Tick;
            //TopMost = true;        
            InitializeCalendar();
        }

        public void InitializeCalendar()
        {
            if (WC.InitializeCalendar(settings.GetURL()))
            {
                weekTimeLeft = GetLeftWeekTime();
                UnblockInputs();
            }
            else
            {
                BlockInputs();
                MessageBox.Show("Невозможно инициализировать рабочий календарь. Проверьте ссылку на сайт производственного календаря в параметрах.");
            }
        }

        public bool IsURLValid(string _url)
        {
            return WC.InitializeCalendar(_url);
        }

        private void BlockInputs()
        {
            MaskedTextBoxCame.Enabled = false;
            buttonCame.Enabled = false;
        }

        private void UnblockInputs()
        {
            MaskedTextBoxCame.Enabled = true;
            buttonCame.Enabled = true;
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
                    using (StreamReader sr = new StreamReader(journalFilePath))
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
                MessageBox.Show("Отсутствует файл \"journal.txt\" или в " +
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
               

        private void T_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            labelTimeNow.Text = now.ToLongTimeString();
            int secondsFromCame = now.Hour * 60 * 60 + now.Minute * 60 + now.Second -
                (came.Hour * 60 * 60 + came.Minute * 60 + came.Second);
            weekTimeLeftCount = weekTimeLeft - new Time(0, 0, secondsFromCame);
            labelTimeWeekLeft.Text = weekTimeLeftCount.ToString();


            if (Time.Now() >= toGo && !buttonCame.Enabled && !isItTimeToGo)
            {
                ColorMusicGotToGo();
                NotifyGotToGo();                
                isItTimeToGo = true;
            }
        }

        private void ColorMusicGotToGo()
        {
            labelTimeGo.BackColor = Color.Green;
            labelTimeGo.ForeColor = Color.Yellow;
            labelGo.BackColor = Color.Green;
            labelGo.ForeColor = Color.Yellow;
        }

        private void NotifyGotToGo()
        {
            NotifyIcon NI = new NotifyIcon
            {
                BalloonTipText = "Андрюха, у нас труп! Возможно, криминал!",
                BalloonTipTitle = "По коням!",
                BalloonTipIcon = ToolTipIcon.Info,
                Icon = this.Icon,
                Visible = true
            };
            NI.ShowBalloonTip(1000);
        }

        private void ButtonCame_Click(object sender, EventArgs e)
        {
            Regex cameTimeRegular = new Regex(@"\d\d:\d\d");
            if (cameTimeRegular.IsMatch(MaskedTextBoxCame.Text))
            {
                Int32.TryParse(MaskedTextBoxCame.Text.Split(':')[0], out int h);
                Int32.TryParse(MaskedTextBoxCame.Text.Split(':')[1], out int m);

                DateTime now = DateTime.Now;
                came = new DateTime(now.Year, now.Month, now.Day, h, m, 0);
                if (now >= came)
                {
                    toGo = new Time(h + WC.GetWorkDayTimeForToday(), m, 0);
                    labelTimeGo.Text = toGo.ToString();

                    buttonCame.Enabled = false;
                    ButtonGone.Enabled = true;
                    ButtonSettings.Enabled = false;
                    ButtonSettings.Visible = false;
                    MaskedTextBoxCame.Enabled = false;

                    string H = (h >= 10) ? h.ToString() : "0" + h.ToString();
                    string M = (m >= 10) ? m.ToString() : "0" + m.ToString();

                    AddConfig(this.Text + " " + H + ":" + M);

                    mainTimer.Start();
                }
                else MessageBox.Show($"Текущее время ({now.ToShortTimeString()}) меньше введенного. Проверьте данные ввода.");
            }
            else MessageBox.Show("Некорректно введено время. Формат ввода времени - HH:MM.");

        }
              

        private void AddConfig(string _text)
        {
            using (StreamWriter sw = new StreamWriter(journalFilePath, true, Encoding.Default))
            {
                sw.WriteLine(_text);
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

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(mainTimer.Enabled)
                SaveGoneTime();
        }


        private void MaskedTextBoxCame_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                ButtonCame_Click(this, new EventArgs());
            }
        }
        
        private void ButtonGone_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ButtonSettings_Click(object sender, EventArgs e)
        {
            settings.ShowDialog();
        }

        private void MaskedTextBoxCame_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}
