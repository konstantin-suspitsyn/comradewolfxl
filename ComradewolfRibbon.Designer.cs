namespace comradewolfxl
{
    partial class ComradewolfRibbon : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public ComradewolfRibbon()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

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
            this.tab1 = this.Factory.CreateRibbonTab();
            this.userGroup = this.Factory.CreateRibbonGroup();
            this.logInButton = this.Factory.CreateRibbonButton();
            this.tab1.SuspendLayout();
            this.userGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tab1.Groups.Add(this.userGroup);
            this.tab1.Label = "ComradewolfOLAP";
            this.tab1.Name = "tab1";
            // 
            // userGroup
            // 
            this.userGroup.Items.Add(this.logInButton);
            this.userGroup.Label = "Пользователь";
            this.userGroup.Name = "userGroup";
            // 
            // logInButton
            // 
            this.logInButton.Label = "Войти";
            this.logInButton.Name = "logInButton";
            this.logInButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.button1_Click);
            // 
            // ComradewolfRibbon
            // 
            this.Name = "ComradewolfRibbon";
            this.RibbonType = "Microsoft.Excel.Workbook";
            this.Tabs.Add(this.tab1);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.ComradewolfRibbon_Load);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.userGroup.ResumeLayout(false);
            this.userGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup userGroup;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton logInButton;
    }

    partial class ThisRibbonCollection
    {
        internal ComradewolfRibbon ComradewolfRibbon
        {
            get { return this.GetRibbon<ComradewolfRibbon>(); }
        }
    }
}
