using Microsoft.Office.Tools.Ribbon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace comradewolfxl
{
    public partial class ComradewolfRibbon
    {
        private void ComradewolfRibbon_Load(object sender, RibbonUIEventArgs e)
        {

        }

        private void button1_Click(object sender, RibbonControlEventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.ShowDialog();
        }

        private void buttonCreateCube_Click(object sender, RibbonControlEventArgs e)
        {
            SelectCube selectCube = new SelectCube();
            selectCube.ShowDialog();
        }

        private async void buttonUpdateCube_Click(object sender, RibbonControlEventArgs e)
        {
            ComradeService comradeService = new ComradeService();
            try { 
                await comradeService.updateOlapDataOnSheet();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonChangeCube_Click(object sender, RibbonControlEventArgs e)
        {
            ComradeService comradeService = new ComradeService();
            comradeService.createSelectDialog();
        }
    }
}
