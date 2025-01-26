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
            Microsoft.Office.Tools.Ribbon.RibbonGroup cubeGroup;
            this.buttonCreateCube = this.Factory.CreateRibbonButton();
            this.buttonUpdateCube = this.Factory.CreateRibbonButton();
            this.buttonChangeCube = this.Factory.CreateRibbonButton();
            this.tab1 = this.Factory.CreateRibbonTab();
            this.userGroup = this.Factory.CreateRibbonGroup();
            this.logInButton = this.Factory.CreateRibbonButton();
            cubeGroup = this.Factory.CreateRibbonGroup();
            cubeGroup.SuspendLayout();
            this.tab1.SuspendLayout();
            this.userGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // cubeGroup
            // 
            cubeGroup.Items.Add(this.buttonCreateCube);
            cubeGroup.Items.Add(this.buttonUpdateCube);
            cubeGroup.Items.Add(this.buttonChangeCube);
            cubeGroup.Label = "Работа с данными";
            cubeGroup.Name = "cubeGroup";
            // 
            // buttonCreateCube
            // 
            this.buttonCreateCube.Label = "Создать куб";
            this.buttonCreateCube.Name = "buttonCreateCube";
            this.buttonCreateCube.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.buttonCreateCube_Click);
            // 
            // buttonUpdateCube
            // 
            this.buttonUpdateCube.Label = "Обновить куб";
            this.buttonUpdateCube.Name = "buttonUpdateCube";
            this.buttonUpdateCube.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.buttonUpdateCube_Click);
            // 
            // buttonChangeCube
            // 
            this.buttonChangeCube.Label = "Изменить куб";
            this.buttonChangeCube.Name = "buttonChangeCube";
            this.buttonChangeCube.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.buttonChangeCube_Click);
            // 
            // tab1
            // 
            this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tab1.Groups.Add(this.userGroup);
            this.tab1.Groups.Add(cubeGroup);
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
            this.logInButton.Label = "Вход на хост";
            this.logInButton.Name = "logInButton";
            this.logInButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.button1_Click);
            // 
            // ComradewolfRibbon
            // 
            this.Name = "ComradewolfRibbon";
            this.RibbonType = "Microsoft.Excel.Workbook";
            this.Tabs.Add(this.tab1);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.ComradewolfRibbon_Load);
            cubeGroup.ResumeLayout(false);
            cubeGroup.PerformLayout();
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
        internal Microsoft.Office.Tools.Ribbon.RibbonButton buttonCreateCube;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton buttonUpdateCube;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton buttonChangeCube;
    }

    partial class ThisRibbonCollection
    {
        internal ComradewolfRibbon ComradewolfRibbon
        {
            get { return this.GetRibbon<ComradewolfRibbon>(); }
        }
    }
}
