using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace comradewolfxl
{
    public partial class SelectItemPiece : UserControl
    {
        int selectId;
        readonly OlapFields frontFields;
        readonly Calculations calculations = new Calculations();
        readonly FieldDataTypes fieldDataTypes = new FieldDataTypes();

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
            selectAndWhere.DeleteSelectAndUpdateAll(this.selectId);
        }

        public void UpdateId(int id) { 
            this.selectId = id; 
        }

        public string getSelectedItem()
        {
            return this.selectItemBox.SelectedItem.ToString();
        }

        private void selectItemBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (KeyValuePair<string, OlapFieldsProperty> field in this.frontFields.fields)
            {
                if (field.Value.front_name == (string) this.selectItemBox.SelectedItem)
                {
                    this.addCalculationTypes(field);
                    
                }
            }
        }

        public void setSelectItem(string selectItem)
        {
            this.selectItemBox.SelectedItem = selectItem;
        }

        public void setCalculationType (string calculationType)
        {
            this.calculationType.SelectedItem = calculationType;
        }

        private void addCalculationTypes(KeyValuePair<string, OlapFieldsProperty> field)
        {
            this.calculationType.Items.Clear();
            if (field.Value.data_type == this.fieldDataTypes.fieldTypes["TEXT"] | field.Value.data_type == this.fieldDataTypes.fieldTypes["DATE"] | field.Value.data_type == this.fieldDataTypes.fieldTypes["DATETIME"])
            {
                this.addTextAndDateCalculationTypes();
            }
            if (field.Value.data_type == this.fieldDataTypes.fieldTypes["BOOLEAN"])
            {
                this.addBoolCalculationTypes();
            }
            if (field.Value.data_type == this.fieldDataTypes.fieldTypes["NUMBER"])
            {
                this.addNumberCalculationTypes();
            }
            this.calculationType.SelectedItem = this.calculations.calculations["none"];
        }
        private void addBoolCalculationTypes()
        {
            this.calculationType.Items.Add(this.calculations.calculations["none"]);
            this.calculationType.Items.Add(this.calculations.calculations["count"]);
            this.calculationType.Items.Add(this.calculations.calculations["sum"]);
            this.calculationType.Items.Add(this.calculations.calculations["count_distinct"]);
            this.calculationType.Items.Add(this.calculations.calculations["min"]);
            this.calculationType.Items.Add(this.calculations.calculations["max"]);
            this.calculationType.Items.Add(this.calculations.calculations["distinct"]);

        }
        private void addNumberCalculationTypes()
        {
            this.calculationType.Items.Add(this.calculations.calculations["none"]);
            this.calculationType.Items.Add(this.calculations.calculations["count"]);
            this.calculationType.Items.Add(this.calculations.calculations["sum"]);
            this.calculationType.Items.Add(this.calculations.calculations["count_distinct"]);
            this.calculationType.Items.Add(this.calculations.calculations["min"]);
            this.calculationType.Items.Add(this.calculations.calculations["max"]);
            this.calculationType.Items.Add(this.calculations.calculations["avg"]);
            this.calculationType.Items.Add(this.calculations.calculations["distinct"]);

        }

        private void addTextAndDateCalculationTypes()
        {
            this.calculationType.Items.Add(this.calculations.calculations["none"]);
            this.calculationType.Items.Add(this.calculations.calculations["count"]);
            this.calculationType.Items.Add(this.calculations.calculations["count_distinct"]);
            this.calculationType.Items.Add(this.calculations.calculations["distinct"]);

        }

        internal string getCalculation()
        {
            return this.calculationType.SelectedItem.ToString();
        }
    }
}
