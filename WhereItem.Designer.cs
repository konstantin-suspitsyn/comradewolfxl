namespace comradewolfxl
{
    partial class WhereItem
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.whereField = new System.Windows.Forms.ComboBox();
            this.whereType = new System.Windows.Forms.ComboBox();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.deleteButton = new System.Windows.Forms.Button();
            this.filterHelp = new System.Windows.Forms.Button();
            this.filterHelp2 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // whereField
            // 
            this.whereField.FormattingEnabled = true;
            this.whereField.Location = new System.Drawing.Point(1, 3);
            this.whereField.Name = "whereField";
            this.whereField.Size = new System.Drawing.Size(155, 21);
            this.whereField.TabIndex = 0;
            this.whereField.SelectedIndexChanged += new System.EventHandler(this.whereField_SelectedIndexChanged);
            // 
            // whereType
            // 
            this.whereType.FormattingEnabled = true;
            this.whereType.Location = new System.Drawing.Point(175, 3);
            this.whereType.Name = "whereType";
            this.whereType.Size = new System.Drawing.Size(121, 21);
            this.whereType.TabIndex = 1;
            this.whereType.SelectedIndexChanged += new System.EventHandler(this.whereType_SelectedIndexChanged);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(1, 28);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(155, 20);
            this.dateTimePicker1.TabIndex = 2;
            this.dateTimePicker1.Visible = false;
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Location = new System.Drawing.Point(175, 28);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(121, 20);
            this.dateTimePicker2.TabIndex = 3;
            this.dateTimePicker2.Visible = false;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(1, 28);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(155, 20);
            this.textBox1.TabIndex = 4;
            this.textBox1.Visible = false;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(175, 28);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(121, 20);
            this.textBox2.TabIndex = 5;
            this.textBox2.Visible = false;
            // 
            // deleteButton
            // 
            this.deleteButton.Location = new System.Drawing.Point(303, -1);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(36, 23);
            this.deleteButton.TabIndex = 7;
            this.deleteButton.Text = "del";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // filterHelp
            // 
            this.filterHelp.Location = new System.Drawing.Point(1, 52);
            this.filterHelp.Name = "filterHelp";
            this.filterHelp.Size = new System.Drawing.Size(36, 23);
            this.filterHelp.TabIndex = 8;
            this.filterHelp.Text = "help";
            this.filterHelp.UseVisualStyleBackColor = true;
            this.filterHelp.Visible = false;
            this.filterHelp.Click += new System.EventHandler(this.filterHelp_Click);
            // 
            // filterHelp2
            // 
            this.filterHelp2.Location = new System.Drawing.Point(175, 52);
            this.filterHelp2.Name = "filterHelp2";
            this.filterHelp2.Size = new System.Drawing.Size(36, 23);
            this.filterHelp2.TabIndex = 9;
            this.filterHelp2.Text = "help";
            this.filterHelp2.UseVisualStyleBackColor = true;
            this.filterHelp2.Visible = false;
            this.filterHelp2.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.panel1.Controls.Add(this.filterHelp2);
            this.panel1.Controls.Add(this.filterHelp);
            this.panel1.Controls.Add(this.deleteButton);
            this.panel1.Controls.Add(this.textBox2);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.dateTimePicker2);
            this.panel1.Controls.Add(this.dateTimePicker1);
            this.panel1.Controls.Add(this.whereType);
            this.panel1.Controls.Add(this.whereField);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(341, 79);
            this.panel1.TabIndex = 10;
            // 
            // WhereItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "WhereItem";
            this.Size = new System.Drawing.Size(342, 82);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox whereField;
        private System.Windows.Forms.ComboBox whereType;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button filterHelp;
        private System.Windows.Forms.Button filterHelp2;
        private System.Windows.Forms.Panel panel1;
    }
}
