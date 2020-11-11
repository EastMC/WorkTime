using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorkTime
{
    public partial class Settings : Form
    {
        private Form1 parent;
        private string url;

        public Settings(Form1 _parent)
        {
            InitializeComponent();
            parent = _parent;
            LoadURLfromINI();
            textBoxURL.Text = url;
        }

        private void buttonAccept_Click(object sender, EventArgs e)
        {
            url = textBoxURL.Text;
            LoadURLToINI();
            parent.InitializeCalendar();
            this.Close();
        }

        private void LoadURLToINI()
        {

        }

        private void LoadURLfromINI()
        {
            url = "https://calendar.yoip.ru/work/2020-proizvodstvennyj-calendar.html";
        }

        private bool IsURLValid()
        {
            return parent.IsURLValid(url);
        }

        public string GetURL()
        {
            return url;
        }

     
        private void Settings_Shown(object sender, EventArgs e)
        {
            ShowAllIsOK();
        }

        private void textBoxURL_TextChanged(object sender, EventArgs e)
        {
            url = textBoxURL.Text;
            ShowAllIsOK();
        }

        private void ShowAllIsOK()
        {
            bool result = IsURLValid();
            buttonAccept.Enabled = result;
            if (result)
            {
                labelValidity.ForeColor = Color.Green;
                labelValidity.Text = "Ссылка действительна.";
            }
            else
            {
                labelValidity.ForeColor = Color.Red;
                labelValidity.Text = "Ссылка недействительна.";
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
