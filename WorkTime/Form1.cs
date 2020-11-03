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
        DateTime Came;
        bool GO;
        
        public Form1()
        {
            InitializeComponent();
            this.Text = DateTime.Today.ToString().Split(' ')[0];
            Timer t = new Timer();
            t.Interval = 500;
            t.Tick += T_Tick;
            t.Start();
            TopMost = true;
            GO = false;          

        }
        

        

        private void T_Tick(object sender, EventArgs e)
        {
            labelTimeNow.Text = DateTime.Now.ToString().Split(' ')[1];

            if (DateTime.Now >= Came && !buttonCame.Enabled && !GO)
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

            Came = DateTime.Today;            
            Came = Came.AddHours(h+9);
            Came = Came.AddMinutes(m);

            labelTimeGo.Text = Came.ToString().Split(' ')[1];

            buttonCame.Enabled = false;
            maskedTextBoxCame.Enabled = false;

            string H = (h >= 10) ? h.ToString() : "0" + h.ToString();
            string M = (m >= 10) ? m.ToString() : "0" + m.ToString();

            AddConfig(this.Text + " " + H + ":" + M);


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
    }
}
