using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace comradewolfxl
{
    public partial class WhereItem : UserControl
    {
        int whereId;
        OlapFields frontFields;
        private WhereTypes whereTypes = new WhereTypes();

        public WhereItem(int id, OlapFields frontFields)
        {
            InitializeComponent();
            this.whereId = id;
            this.frontFields = frontFields;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
