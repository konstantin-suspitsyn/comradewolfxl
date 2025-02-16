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
    public partial class SelectCube : Form
    {
        ComradeWolfUtils wolfUtils;
        ComradeHttpUtils httpUtils;

        string token = null;
        string currentHost = null;

        public SelectCube()
        {
            wolfUtils = new ComradeWolfUtils();
            httpUtils = new ComradeHttpUtils();
            InitializeComponent();
            checkCredentials();

        }

        private async void buttonSelectCube_Click(object sender, EventArgs e)
        {
            string cube;

            if (this.comboSelectCube.SelectedItem == null)
            {
                MessageBox.Show("Необходимо выбрать куб из выпадающего списка");
                return;
            }

            cube = this.comboSelectCube.SelectedItem.ToString();

            OlapFields frontFields = await httpUtils.GetFields(currentHost, cube, token);
            this.Close();

            this.createSelectAndWherePanel(frontFields, cube, currentHost);
        }

        private void createSelectAndWherePanel(OlapFields frontFields, string cubeName, string currentHost)
        {
            SelectAndWhere selectAndWhere = new SelectAndWhere(frontFields, cubeName, currentHost);
            selectAndWhere.Show();
        }

        private void checkCredentials()
        {

            this.СheckHost();
            this.CheckToken();

            if ((token == null) || (currentHost == null))
            {
                MessageBox.Show("Нет логина — нет прогресса");
                this.DialogResult = DialogResult.Cancel;
                this.Close();
                return;
            }

            this.labelSelectedHost.Text = currentHost;

            // if ok then load data
            this.GetCubes(currentHost, token);
        }

        private void СheckHost()
        {
            Dictionary<int, string> hosts = wolfUtils.getHostInfo();

            if (!hosts.ContainsKey(0))
            {
                LoginForm loginForm = new LoginForm();

                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    hosts = wolfUtils.getHostInfo();
                    currentHost = hosts[0];
                }
                else
                {
                    MessageBox.Show("Сервер не выбран. Необходимо залогиниться");

                    return;
                }

            }

            currentHost = hosts[0];

        }

        private void CheckToken()
        {

            if (currentHost == null)
            {
                this.СheckHost();
            }

            // TODO REFACTOR REPEATED CODE
            // check for credentials
            string currentToken = wolfUtils.ReadFromRegistry(httpUtils.GetAuthPostfix() + currentHost, null);


            if ((currentToken == null) || (!httpUtils.IsTokenValid(currentToken)))
            {
                // if not then open login form
                MessageBox.Show("Необходимо залогиниться");
                LoginForm loginForm = new LoginForm();
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show("Тута");
                    CheckToken();
                }

            }
            else
            {
                token = currentToken;
            }
        }

        private async void GetCubes(string currentHost, string token)
        {
            Dictionary<int, string> cubes = await this.httpUtils.GetCubesAsync(currentHost, token);
            foreach (KeyValuePair<int, string> entry in cubes)
            {
                this.comboSelectCube.Items.Add(entry.Value);
            }
        }

    }
}
