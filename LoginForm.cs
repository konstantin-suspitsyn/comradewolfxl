using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace comradewolfxl
{
    public partial class LoginForm : Form
    {
        ComradeWolfUtils wolfUtils;
        public LoginForm()
        {
            wolfUtils = new ComradeWolfUtils();
            InitializeComponent();
            update_dropdown();
        }

        private void update_dropdown()
        {
            Dictionary<int, string> hosts = wolfUtils.getHostInfo();
            foreach (var host in hosts)
            {
                this.hostBox.Items.Add(host.Value);
                this.hostBox.SelectedIndex = 0;
            }
        }

        private async void enter_Click(object sender, EventArgs e)
        {
            ComradeHttpUtils comradeHttpUtils = new ComradeHttpUtils();

            bool canConnect;
            string hostName = this.hostBox.Text;
            string username = this.username.Text;
            string password = this.password.Text;
            string token = "";

            canConnect = await this.AddHostToDictionary(hostName);

            if (canConnect)
            {

                try
                {
                    token = await comradeHttpUtils.getToken(hostName, username, password);
                } catch (UnauthorizedAccessException) {
                    MessageBox.Show("Логин или пароль некорректные");
                }

                if (token != "")
                {
                    wolfUtils.StoreInRegistry("AUTH_TOKEN" + hostName, token);

                    this.DialogResult = DialogResult.OK;

                    this.Close();
                } 

            }


        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            string hostName = this.hostBox.Text;
            if (this.emptyHostMessage(hostName)) { return; }

            wolfUtils.DeleteFromRegistry("AUTH_TOKEN" + hostName);

        }

        private void delhost_Click(object sender, EventArgs e)
        {
            string hostName = this.hostBox.Text;
            if (this.emptyHostMessage(hostName)) { return; }

            //Remove host
            this.hostBox.SelectedIndex = -1;
            this.hostBox.SelectedItem = null;
            this.hostBox.Items.Remove(hostName);
            
            Dictionary<int, string> hosts = new Dictionary<int, string>();

            int i = 0;

            foreach (string item in this.hostBox.Items)
            {
                hosts[i] = item;
                i++;
            }

            wolfUtils.saveHostInfo(hosts);

        }

        private bool emptyHostMessage(string hostName)
        {
            if (hostName == "")
            {
                MessageBox.Show("Необходимо выбрать host");
                return true;
            }
            return false;
        }

        private async void addHostButton_Click(object sender, EventArgs e)
        {
            //Check host and add it if it exists
            await this.AddHostToDictionary(this.hostBox.Text);

        }

        private async Task<bool> AddHostToDictionary(string hostName)
        {
            ComradeHttpUtils comradeHttpUtils = new ComradeHttpUtils();

            bool canConnect = false;

            if (hostName == "")
            {
                MessageBox.Show("Необходимо добавить Хост");
                return canConnect;
            }
            else
            {

                canConnect = await comradeHttpUtils.healthCheckAsync(hostName);
            }

            if (!canConnect)
            {
                MessageBox.Show("Сервер не отвечает или вы ввели неправильный адрес");
                return canConnect;
            }

            if (canConnect)
            {

                Dictionary<int, string> hosts = wolfUtils.checkHost(hostName);
                wolfUtils.saveHostInfo(hosts);

                return canConnect;

            }

            return canConnect;
        }
    }
}
