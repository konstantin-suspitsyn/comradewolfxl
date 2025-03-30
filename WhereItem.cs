using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace comradewolfxl
{
    public partial class WhereItem : UserControl
    {
        int whereId;
        OlapFields frontFields;
        private WhereTypes whereTypesDict = new WhereTypes();
        readonly FieldDataTypes fieldDataTypes = new FieldDataTypes();
        string cubeName;
        string currentHost;

        public WhereItem(int id, OlapFields frontFields, string cubeName, string currentHost)
        {
            InitializeComponent();

            this.cubeName = cubeName;
            this.currentHost = currentHost;

            this.whereId = id;
            this.frontFields = frontFields;
            foreach (KeyValuePair<string, OlapFieldsProperty> field in this.frontFields.fields)
            {
                this.whereField.Items.Add(field.Value.front_name);
            }

        }

        private void whereField_SelectedIndexChanged(object sender, EventArgs e)
        {
            changeWhereField();
        }

        private void changeWhereField()
        {
            this.whereType.Items.Clear();
            foreach (KeyValuePair<string, OlapFieldsProperty> field in this.frontFields.fields)
            {
                if (field.Value.front_name == (string)this.whereField.SelectedItem)
                {
                    this.addWhereCondition(field);
                }
            }

        }

            private void addWhereCondition(KeyValuePair<string, OlapFieldsProperty> field)
            {
                if (field.Value.data_type == this.fieldDataTypes.fieldTypes["TEXT"])
                {
                    this.whereType.Items.Add("В списке");
                    this.whereType.Items.Add("Не в списке");
                    this.whereType.Items.Add("Равно");
                    this.whereType.Items.Add("Содержит");
                    this.whereType.Items.Add("Начинается с");
                    this.whereType.Items.Add("Заканчивается на");
                    this.whereType.Items.Add("Не равно");
                }
        
                if (field.Value.data_type == this.fieldDataTypes.fieldTypes["BOOLEAN"])
                {
                this.whereType.Items.Add(whereTypesDict.where["="]);
            }

            if (field.Value.data_type == this.fieldDataTypes.fieldTypes["DATE"] |
                field.Value.data_type == this.fieldDataTypes.fieldTypes["NUMBER"] |
                field.Value.data_type == this.fieldDataTypes.fieldTypes["DATETIME"])
            {
                this.whereType.Items.Add("В списке");
                this.whereType.Items.Add("Не в списке");
                this.whereType.Items.Add("Равно");
                this.whereType.Items.Add(">");
                this.whereType.Items.Add(">=");
                this.whereType.Items.Add("<");
                this.whereType.Items.Add("<=");
                this.whereType.Items.Add("Не равно");
                this.whereType.Items.Add("Meжду");
            }

        }

        private void whereType_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateWhereItems();
        }

        private Tuple<bool, bool, bool> updateWhereItems()
        {

            // Clear where items
            this.dateTimePicker1.Visible = false;
            this.dateTimePicker2.Visible = false;
            this.textBox1.Visible = false;
            this.textBox2.Visible = false;
            this.filterHelp.Visible = false;
            this.filterHelp2.Visible = false;

            textBox1.Clear();
            textBox2.Clear();
            this.dateTimePicker1.Value = DateTimePicker.MinimumDateTime;
            this.dateTimePicker2.Value = DateTimePicker.MinimumDateTime;


            foreach (KeyValuePair<string, OlapFieldsProperty> field in this.frontFields.fields)
            {
                if (field.Value.front_name == (string)this.whereField.SelectedItem)
                {
                    Tuple<bool, bool, bool> allTypes = this.changeWhereConditions(field);
                    return allTypes;
                }
            }
            throw new Exception("No such condition");
        }

        private Tuple<bool, bool, bool> changeWhereConditions(KeyValuePair<string, OlapFieldsProperty> field)
        {
            
            bool isBetween = false;
            bool isDate = false;
            bool isDateTime = false;

            if (this.whereType.SelectedItem.Equals("Meжду")) { isBetween = true; }

            if (field.Value.data_type == this.fieldDataTypes.fieldTypes["DATETIME"])
            {
                this.dateTimePicker1.Visible = true;
                dateTimePicker1.Format = DateTimePickerFormat.Custom;
                dateTimePicker1.CustomFormat = "yyyy-mm-DD hh:mm";
                isDateTime = true;

                if (isBetween)
                {
                    this.dateTimePicker2.Visible = true;
                    dateTimePicker2.Format = DateTimePickerFormat.Custom;
                    dateTimePicker2.CustomFormat = "yyyy-mm-DD hh:mm";
                }
            }

            if (field.Value.data_type == this.fieldDataTypes.fieldTypes["DATE"])
            {
                this.dateTimePicker1.Visible = true;
                dateTimePicker1.Format = DateTimePickerFormat.Custom;
                dateTimePicker1.CustomFormat = "yyyy-mm-DD";
                isDate = true;

                if (isBetween)
                {
                    this.dateTimePicker2.Visible = true;
                    dateTimePicker2.Format = DateTimePickerFormat.Custom;
                    dateTimePicker2.CustomFormat = "yyyy-mm-DD";
                }
            }

            if (field.Value.data_type == this.fieldDataTypes.fieldTypes["NUMBER"] | field.Value.data_type == this.fieldDataTypes.fieldTypes["TEXT"])
            {
                this.textBox1.Visible = true;
                this.filterHelp.Visible = true;

                if (isBetween)
                {
                    this.textBox2.Visible = true;
                    this.filterHelp2.Visible = true;
                }
            }


            return new Tuple<bool, bool, bool>(isBetween, isDate, isDateTime);
        }


        private void deleteButton_Click(object sender, EventArgs e)
        {
            SelectAndWhere selectAndWhere = (SelectAndWhere)ParentForm;
            selectAndWhere.DeleteWhereAndUpdateAll(this.whereId);
        }

        internal void UpdateId(int i)
        {
            this.whereId = i;
        }

        internal string getWhereItem()
        {
            return this.whereField.SelectedItem.ToString();
        }
        internal string getWhereType()
        {
            return this.whereType.SelectedItem.ToString();
        }

        internal string getWhereCondition1()
        {
            return this.textBox1.Text.ToString();
        }

        internal string getWhereCondition2()
        {
            return this.textBox2.Text.ToString();
        }

        internal void addItem(string frontName, string whereTypeFront, string condition1, string condition2)
        {
            this.whereField.SelectedItem = frontName;
            this.changeWhereField();
            this.whereType.SelectedItem = whereTypeFront;

            Tuple<bool, bool, bool> whereItems = updateWhereItems();
            bool isBetween = whereItems.Item1;
            bool isDate = whereItems.Item2;
            bool isDateTime = whereItems.Item3;

            // TODO: Нужно понять что делать с временем и датой, если они пришли

            if (isDate || isDateTime )
            {
                // TODO Требуется тест
                this.dateTimePicker1.Text = condition1;
                if (isBetween)
                {
                    this.dateTimePicker2.Text = condition2;
            }
            } else
            {
                this.textBox1.Text = condition1;
                if (isBetween)
                {
                    this.textBox2.Text = condition2;
                }
            }

        }

        private string getAutoWhere()
        {
            bool isIn = false;
            string fieldName;

            string result = "";



            if (this.whereField.SelectedItem == null)
            {
                MessageBox.Show("Необходимо выбрать поле");
                return null;
            }

            fieldName = this.frontFields.getBackendNameByFrontend(this.whereField.Text);

            if (this.whereType.SelectedItem == null)
            {
                MessageBox.Show("Необходимо выбрать поле");
                return null;

            }

            if (this.whereType.Text == "В списке")
            {
                isIn = true;
            }

            using (WhereHelper whereHelper = new WhereHelper(fieldName, this.cubeName, this.currentHost, isIn))
            {
                whereHelper.ShowDialog();

                result = whereHelper.SelectedStaff;

            };

            return result;
        }

        private void filterHelp_Click(object sender, EventArgs e)
        {
            string result = this.getAutoWhere();
            if (this.textBox1.Visible == true)
            {
                this.textBox1.Text = result;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string result = this.getAutoWhere();

            if (this.textBox2.Visible == true)
            {
                this.textBox2.Text = result;
            }

        }
    }


}
