namespace comradewolfxl
{
    partial class SelectAndWhere
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.addSelect = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.addWhere = new System.Windows.Forms.Button();
            this.createCube = new System.Windows.Forms.Button();
            this.selectPanel = new System.Windows.Forms.Panel();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // addSelect
            // 
            this.addSelect.Location = new System.Drawing.Point(191, 455);
            this.addSelect.Name = "addSelect";
            this.addSelect.Size = new System.Drawing.Size(163, 23);
            this.addSelect.TabIndex = 0;
            this.addSelect.Text = "Добавить select";
            this.addSelect.UseVisualStyleBackColor = true;
            this.addSelect.Click += new System.EventHandler(this.addSelect_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.addWhere);
            this.groupBox2.Location = new System.Drawing.Point(876, 13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(297, 446);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "where";
            // 
            // addWhere
            // 
            this.addWhere.Location = new System.Drawing.Point(68, 76);
            this.addWhere.Name = "addWhere";
            this.addWhere.Size = new System.Drawing.Size(75, 23);
            this.addWhere.TabIndex = 0;
            this.addWhere.Text = "button3";
            this.addWhere.UseVisualStyleBackColor = true;
            // 
            // createCube
            // 
            this.createCube.Location = new System.Drawing.Point(477, 499);
            this.createCube.Name = "createCube";
            this.createCube.Size = new System.Drawing.Size(75, 23);
            this.createCube.TabIndex = 2;
            this.createCube.Text = "button1";
            this.createCube.UseVisualStyleBackColor = true;
            // 
            // selectPanel
            // 
            this.selectPanel.Location = new System.Drawing.Point(12, 24);
            this.selectPanel.Name = "selectPanel";
            this.selectPanel.Size = new System.Drawing.Size(736, 402);
            this.selectPanel.TabIndex = 3;
            // 
            // SelectAndWhere
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1185, 554);
            this.Controls.Add(this.selectPanel);
            this.Controls.Add(this.addSelect);
            this.Controls.Add(this.createCube);
            this.Controls.Add(this.groupBox2);
            this.Name = "SelectAndWhere";
            this.Text = "SelectAndWhere";
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button createCube;
        private System.Windows.Forms.Button addSelect;
        private System.Windows.Forms.Button addWhere;
        private System.Windows.Forms.Panel selectPanel;
    }
}