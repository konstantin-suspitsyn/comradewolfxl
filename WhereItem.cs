﻿using System;
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

        public WhereItem(int id, OlapFields frontFields)
        {
            InitializeComponent();
            this.whereId = id;
            this.frontFields = frontFields;
            foreach (KeyValuePair<string, OlapFieldsProperty> field in this.frontFields.fields)
            {
                this.whereField.Items.Add(field.Value.front_name);
            }

        }

        private void whereField_SelectedIndexChanged(object sender, EventArgs e)
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
            // Clear where items
            this.dateTimePicker1.Visible = false;
            this.dateTimePicker2.Visible = false;
            this.textBox1.Visible = false;
            this.textBox2.Visible = false;
            textBox1.Clear();
            textBox2.Clear();
            this.dateTimePicker1.Value = DateTimePicker.MinimumDateTime;
            this.dateTimePicker2.Value = DateTimePicker.MinimumDateTime;


            foreach (KeyValuePair<string, OlapFieldsProperty> field in this.frontFields.fields)
            {
                if (field.Value.front_name == (string)this.whereField.SelectedItem)
                {
                    this.changeWhereConditions(field);
                }
            }
        }

        private void changeWhereConditions(KeyValuePair<string, OlapFieldsProperty> field)
        {
            
            bool isBetween = false;
            

            if (this.whereType.SelectedItem.Equals("Meжду")) { isBetween = true; }

            if (field.Value.data_type == this.fieldDataTypes.fieldTypes["DATETIME"])
            {
                this.dateTimePicker1.Visible = true;
                dateTimePicker1.Format = DateTimePickerFormat.Custom;
                dateTimePicker1.CustomFormat = "yyyy-mm-DD hh:mm";

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

                if (isBetween)
                {
                    this.textBox2.Visible = true;
                }
            }



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
    }


}
