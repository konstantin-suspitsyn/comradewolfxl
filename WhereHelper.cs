using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace comradewolfxl
{
    public partial class WhereHelper : Form
    {

        public string cubeName;
        public bool isIn;
        public string fieldName;
        public string SelectedStaff = "";
        public string host;

        private const int heightOfWhereItem = 20;

        private ComradeService comradeService = new ComradeService();
        

        public WhereHelper(string fieldName, string cubeName, string hostName, bool isIn)
        {
            this.fieldName = fieldName;
            this.cubeName = cubeName;   
            this.isIn = isIn; 
            this.host = hostName;

            InitializeComponent();

            // Добавляем скролл по вертикали
            this.itemsPanel.AutoScroll = false;
            this.itemsPanel.HorizontalScroll.Enabled = false;
            this.itemsPanel.HorizontalScroll.Visible = false;
            this.itemsPanel.HorizontalScroll.Maximum = 0;
            this.itemsPanel.AutoScroll = true;


            this.getFilterData(); 

        }

        public WhereHelper()
        {
            InitializeComponent();
        }

        private async void getFilterData()
        {
            DistinctValuesDTO distinctVals = await this.comradeService.GetWhereHelp(fieldName, this.host, this.cubeName);
            int startingPosition = 0;
            foreach(string item in  distinctVals.distinct_data) {
                if (this.isIn)
                {
                    System.Windows.Forms.CheckBox checkBox = new System.Windows.Forms.CheckBox();
                    checkBox.Width = this.itemsPanel.Width - 10;
                    checkBox.Text = item;
                    this.itemsPanel.Controls.Add(checkBox);

                    checkBox.Location = new System.Drawing.Point(0, startingPosition);

                    startingPosition = startingPosition + heightOfWhereItem;
                }
                else
                {
                    System.Windows.Forms.RadioButton itemRadio = new System.Windows.Forms.RadioButton();
                    itemRadio.Text = item;
                    itemRadio.Width = this.itemsPanel.Width - 10;

                    this.itemsPanel.Controls.Add(itemRadio);

                    itemRadio.Location = new System.Drawing.Point(0, startingPosition);

                    startingPosition = startingPosition + heightOfWhereItem;
                }

            }


        }

        private void okButton_Click(object sender, EventArgs e)
        {


            if (this.isIn)
            {

                List<string> results = new List<string>();
                foreach (System.Windows.Forms.CheckBox item in this.itemsPanel.Controls)
                {
                    if (item.Checked)
                    {
                        results.Add(comradeService.EscapeSeparator(item.Text));
                    }
                }

                this.SelectedStaff = String.Join(";", results.ToArray());
            }
            else
            {
                foreach (System.Windows.Forms.RadioButton item in this.itemsPanel.Controls)
                {
                    if (item.Checked)
                    {
                        this.SelectedStaff = item.Text;
                        this.Close();
                    }
                }
            }
            this.Close();
        }

    }
}
