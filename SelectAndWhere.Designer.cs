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
            this.addWhere = new System.Windows.Forms.Button();
            this.createCube = new System.Windows.Forms.Button();
            this.selectPanel = new System.Windows.Forms.Panel();
            this.panelWhere = new System.Windows.Forms.Panel();
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
            // addWhere
            // 
            this.addWhere.Location = new System.Drawing.Point(862, 455);
            this.addWhere.Name = "addWhere";
            this.addWhere.Size = new System.Drawing.Size(146, 23);
            this.addWhere.TabIndex = 0;
            this.addWhere.Text = "Добавить where";
            this.addWhere.UseVisualStyleBackColor = true;
            this.addWhere.Click += new System.EventHandler(this.addWhere_Click);
            // 
            // createCube
            // 
            this.createCube.Location = new System.Drawing.Point(552, 502);
            this.createCube.Name = "createCube";
            this.createCube.Size = new System.Drawing.Size(75, 23);
            this.createCube.TabIndex = 2;
            this.createCube.Text = "Ok";
            this.createCube.UseVisualStyleBackColor = true;
            this.createCube.Click += new System.EventHandler(this.createCube_ClickAsync);
            // 
            // selectPanel
            // 
            this.selectPanel.Location = new System.Drawing.Point(12, 24);
            this.selectPanel.Name = "selectPanel";
            this.selectPanel.Size = new System.Drawing.Size(654, 402);
            this.selectPanel.TabIndex = 3;
            // 
            // panelWhere
            // 
            this.panelWhere.Location = new System.Drawing.Point(690, 24);
            this.panelWhere.Name = "panelWhere";
            this.panelWhere.Size = new System.Drawing.Size(483, 402);
            this.panelWhere.TabIndex = 4;
            // 
            // SelectAndWhere
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1185, 554);
            this.Controls.Add(this.panelWhere);
            this.Controls.Add(this.addWhere);
            this.Controls.Add(this.selectPanel);
            this.Controls.Add(this.addSelect);
            this.Controls.Add(this.createCube);
            this.Name = "SelectAndWhere";
            this.Text = "SelectAndWhere";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button createCube;
        private System.Windows.Forms.Button addSelect;
        private System.Windows.Forms.Button addWhere;
        private System.Windows.Forms.Panel selectPanel;
        private System.Windows.Forms.Panel panelWhere;
    }
}