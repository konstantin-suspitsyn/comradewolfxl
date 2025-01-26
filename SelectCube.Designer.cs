namespace comradewolfxl
{
    partial class SelectCube
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
            this.labelSelectedHost = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboSelectCube = new System.Windows.Forms.ComboBox();
            this.buttonSelectCube = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelSelectedHost
            // 
            this.labelSelectedHost.AutoSize = true;
            this.labelSelectedHost.Location = new System.Drawing.Point(12, 9);
            this.labelSelectedHost.Name = "labelSelectedHost";
            this.labelSelectedHost.Size = new System.Drawing.Size(87, 13);
            this.labelSelectedHost.TabIndex = 0;
            this.labelSelectedHost.Text = "Хост не выбран";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Выбрать куб";
            // 
            // comboSelectCube
            // 
            this.comboSelectCube.FormattingEnabled = true;
            this.comboSelectCube.Location = new System.Drawing.Point(15, 56);
            this.comboSelectCube.Name = "comboSelectCube";
            this.comboSelectCube.Size = new System.Drawing.Size(202, 21);
            this.comboSelectCube.TabIndex = 2;
            // 
            // buttonSelectCube
            // 
            this.buttonSelectCube.Location = new System.Drawing.Point(15, 95);
            this.buttonSelectCube.Name = "buttonSelectCube";
            this.buttonSelectCube.Size = new System.Drawing.Size(75, 23);
            this.buttonSelectCube.TabIndex = 3;
            this.buttonSelectCube.Text = "Выбрать";
            this.buttonSelectCube.UseVisualStyleBackColor = true;
            this.buttonSelectCube.Click += new System.EventHandler(this.buttonSelectCube_Click);
            // 
            // SelectCube
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(240, 169);
            this.Controls.Add(this.buttonSelectCube);
            this.Controls.Add(this.comboSelectCube);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelSelectedHost);
            this.Name = "SelectCube";
            this.Text = "SelectCube";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelSelectedHost;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboSelectCube;
        private System.Windows.Forms.Button buttonSelectCube;
    }
}