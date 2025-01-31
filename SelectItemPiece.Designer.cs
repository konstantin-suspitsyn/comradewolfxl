namespace comradewolfxl
{
    partial class SelectItemPiece
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
            this.selectItemBox = new System.Windows.Forms.ComboBox();
            this.calculationType = new System.Windows.Forms.ComboBox();
            this.deleteButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // selectItemBox
            // 
            this.selectItemBox.FormattingEnabled = true;
            this.selectItemBox.Location = new System.Drawing.Point(0, 3);
            this.selectItemBox.Name = "selectItemBox";
            this.selectItemBox.Size = new System.Drawing.Size(196, 21);
            this.selectItemBox.TabIndex = 0;
            this.selectItemBox.SelectedIndexChanged += new System.EventHandler(this.selectItemBox_SelectedIndexChanged);
            // 
            // calculationType
            // 
            this.calculationType.FormattingEnabled = true;
            this.calculationType.Location = new System.Drawing.Point(202, 3);
            this.calculationType.Name = "calculationType";
            this.calculationType.Size = new System.Drawing.Size(217, 21);
            this.calculationType.TabIndex = 1;
            // 
            // deleteButton
            // 
            this.deleteButton.Location = new System.Drawing.Point(425, 1);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(75, 23);
            this.deleteButton.TabIndex = 2;
            this.deleteButton.Text = "delete";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // SelectItemPiece
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.calculationType);
            this.Controls.Add(this.selectItemBox);
            this.Name = "SelectItemPiece";
            this.Size = new System.Drawing.Size(504, 26);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox selectItemBox;
        private System.Windows.Forms.ComboBox calculationType;
        private System.Windows.Forms.Button deleteButton;
    }
}
