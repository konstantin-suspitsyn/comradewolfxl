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
    public partial class SelectAndWhere : Form
    {

        private const int heightOfSelectForm = 40;
        private const int heightOfWhereForm = 150;

        OlapFields frontFields;
        string cubeName;
        string currentHost;
        ComradeHttpUtils comradeHttpUtils;
        List<SelectItemPiece> selectItems;
        List<WhereItem> whereItems;
        int selectIndex = 0;
        int whereIndex = 0;


        public SelectAndWhere(OlapFields frontFields, string cubeName, string currentHost)
        {
            InitializeComponent();
            this.frontFields = frontFields;
            this.cubeName = cubeName;
            this.currentHost = currentHost;
            this.comradeHttpUtils = new ComradeHttpUtils();
            this.whereItems = new List<WhereItem>();

            // Добавляем скролл по вертикали
            selectPanel.AutoScroll = false;
            selectPanel.HorizontalScroll.Enabled = false;
            selectPanel.HorizontalScroll.Visible = false;
            selectPanel.HorizontalScroll.Maximum = 0;
            selectPanel.AutoScroll = true;

            // Добавляем скролл по вертикали
            panelWhere.AutoScroll = false;
            panelWhere.HorizontalScroll.Enabled = false;
            panelWhere.HorizontalScroll.Visible = false;
            panelWhere.HorizontalScroll.Maximum = 0;
            panelWhere.AutoScroll = true;
        }

        private void addSelect_Click(object sender, EventArgs e)
        {
            
            SelectItemPiece selectItemPiece = new SelectItemPiece(selectIndex, frontFields);
            this.selectPanel.Controls.Add(selectItemPiece);
            
            selectItemPiece.Location = new System.Drawing.Point(0, selectIndex * heightOfSelectForm);
            this.selectIndex++;


        }

        public void DeleteButtonAndUpdateAll(int id)
        {
            this.selectPanel.Controls.RemoveAt(id);
            this.selectIndex--;

            int i = 0;

            foreach (SelectItemPiece selectItemPiece in this.selectPanel.Controls)
            {
                if (i >= id)
                {
                    selectItemPiece.UpdateId(i);
                    selectItemPiece.Location = new System.Drawing.Point(0, (i+1) * heightOfSelectForm);
                }
                i++;
            }

        }

        private void addWhere_Click(object sender, EventArgs e)
        {
            WhereItem whereItem = new WhereItem(whereIndex, frontFields);
            this.panelWhere.Controls.Add(whereItem);
            
            whereItem.Location = new System.Drawing.Point(0, whereIndex * heightOfWhereForm);
            whereIndex++;
        }
    }
}
