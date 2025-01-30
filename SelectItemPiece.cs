using System;
using System.Collections;
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
    public partial class SelectItemPiece : UserControl
    {
        int selectId;
        OlapFields frontFields;
        Calculations calculations = new Calculations();

        public SelectItemPiece()
        {
            InitializeComponent();
        }

        public SelectItemPiece(int id, OlapFields frontFields)
        {
            InitializeComponent();
            this.selectId = id;
            this.frontFields = frontFields;

            foreach (KeyValuePair<string, OlapFieldsProperty> field in this.frontFields.fields)
            {
                this.selectItemBox.Items.Add(field.Value.front_name);
            }

            this.calculationType.Items.Add(this.calculations.calculations["none"]);
            this.calculationType.SelectedItem = this.calculations.calculations["none"];


        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            SelectAndWhere selectAndWhere = (SelectAndWhere)ParentForm;
            selectAndWhere.DeleteButtonAndUpdateAll(this.selectId);
        }

        public void UpdateId(int id) { 
            this.selectId = id; 
        }
    }
}
