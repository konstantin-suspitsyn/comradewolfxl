using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

            bool canConnect = false;
            string hostName = this.hostBox.Text;
            string username = this.username.Text;
            string password = this.password.Text;

            if (hostName == "")
            {
                MessageBox.Show("Необходимо добавить Хост");
                return;
            }
            else
            {

                canConnect = await comradeHttpUtils.healthCheckAsync(hostName);
            }

            if (!canConnect)
            {
                MessageBox.Show("Сервер не отвечает или вы ввели неправильный адрес");
                return;
            }

            if (canConnect)
            {


                Dictionary<int, string> hosts = wolfUtils.checkHost(hostName);
                wolfUtils.saveHostInfo(hosts);

                string token = await comradeHttpUtils.getToken(hostName, username, password);

                wolfUtils.StoreInRegistry("AUTH_TOKEN" + hostName, token);

                this.DialogResult = DialogResult.OK;

                this.Close();

            }


        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            string hostName = this.hostBox.Text;
            if (hostName == "")
            {
                MessageBox.Show("Необходимо выбрать host");
                return;
            }

            wolfUtils.DeleteFromRegistry("AUTH_TOKEN" + hostName);

        }
    }
}
