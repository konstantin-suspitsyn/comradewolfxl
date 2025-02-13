using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;

namespace comradewolfxl
{
    public partial class SelectAndWhere : Form
    {

        private const int heightOfSelectForm = 40;
        private const int heightOfWhereForm = 70;

        private const int WHERE_FRONT_ROW_NO = 1;
        private const int WHERE_BACK_ROW_NO = 2;
        private const int WHERE_TYPE_ROW_NO = 3;
        private const int WHERE_CONDITION_1_ROW_NO = 4;
        private const int WHERE_CONDITION_2_ROW_NO = 5;
        private const int SELECT_FRONT_ROW_NO = 6;
        private const int SELECT_BACK_ROW_NO = 7;
        private const int SELECT_CALCULATION_ROW_NO = 8;


        OlapFields frontFields;
        string cubeName;
        string currentHost;
        ComradeHttpUtils comradeHttpUtils;
        List<SelectItemPiece> selectItems;
        List<WhereItem> whereItems;
        int selectIndex = 0;
        int whereIndex = 0;
        Calculations calculations;
        WhereTypes whereTypes;


        public SelectAndWhere(OlapFields frontFields, string cubeName, string currentHost)
        {
            InitializeComponent();
            this.frontFields = frontFields;
            this.cubeName = cubeName;
            this.currentHost = currentHost;
            this.comradeHttpUtils = new ComradeHttpUtils();
            this.whereItems = new List<WhereItem>();
            this.calculations = new Calculations();
            whereTypes = new WhereTypes();

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

        public void DeleteSelectAndUpdateAll(int id)
        {
            this.selectPanel.Controls.RemoveAt(id);
            this.selectIndex--;

            int i = 0;

            foreach (SelectItemPiece selectItemPiece in this.selectPanel.Controls)
            {
                if (i >= id)
                {
                    selectItemPiece.UpdateId(i);
                    selectItemPiece.Location = new System.Drawing.Point(0, (i) * heightOfSelectForm);
                }
                i++;
            }

        }

        public void DeleteWhereAndUpdateAll(int id)
        {
            this.panelWhere.Controls.RemoveAt(id);
            this.whereIndex--;

            int i = 0;

            foreach (WhereItem whereItem in this.panelWhere.Controls)
            {
                if (i >= id)
                {
                    whereItem.UpdateId(i);
                    whereItem.Location = new System.Drawing.Point(0, (i) * heightOfWhereForm);
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

        private void createCube_Click(object sender, EventArgs e)
        {
            // TODO: Make form to use current worksheet or create new
            // Now we create new worksheet
            Workbook wb = (Workbook)Globals.ThisAddIn.Application.ActiveWorkbook;
            var worksheet = (Worksheet)wb.Worksheets.Add();

            Worksheet activeWs = Globals.ThisAddIn.Application.ActiveSheet;
            MessageBox.Show(activeWs.Name);

            List<SelectDTO> selectList = new List<SelectDTO>();
            List<CalculationDTO > calculationList = new List<CalculationDTO>();
            List<WhereDTO> whereList = new List<WhereDTO>();

            int startingSelectVal = 1;

            foreach (SelectItemPiece selectItemPiece in this.selectPanel.Controls)
            {
                MessageBox.Show(selectItemPiece.getSelectedItem());
                // DO SMTH
                string tempFrontendName = selectItemPiece.getSelectedItem();
                string tempCalculation = selectItemPiece.getCalculation();

                string backendName = this.frontFields.getBackendNameByFrontend(tempFrontendName);
                string backendCalculation = this.calculations.getCalculationKeyByValue(tempCalculation);

                activeWs.Cells[SELECT_FRONT_ROW_NO, startingSelectVal].Value = tempFrontendName;
                activeWs.Cells[SELECT_BACK_ROW_NO, startingSelectVal].Value = backendName;
                activeWs.Cells[SELECT_CALCULATION_ROW_NO, startingSelectVal].Value = backendCalculation;

                if (backendCalculation == "None")
                {
                    selectList.Add(new SelectDTO(backendName));
                } else
                {
                    calculationList.Add(new CalculationDTO(backendName, backendCalculation));
                }

                startingSelectVal++;
            }

            int startingWhereVal = 1;

            foreach (WhereItem whereItem in this.panelWhere.Controls)
            {
                
                string tempFrontWhere = whereItem.getWhereItem();
                string tempWhereType = whereItem.getWhereType();
                string tempCond1 = whereItem.getWhereCondition1();
                string tempCond2 = whereItem.getWhereCondition2();
                string backendNameTemp = this.frontFields.getBackendNameByFrontend(tempFrontWhere);

                string whereType = this.whereTypes.where[tempWhereType];

                activeWs.Cells[WHERE_FRONT_ROW_NO, startingWhereVal].Value = tempFrontWhere;
                activeWs.Cells[WHERE_BACK_ROW_NO, startingWhereVal].Value = backendNameTemp;
                activeWs.Cells[WHERE_TYPE_ROW_NO, startingWhereVal].Value = whereType;
                activeWs.Cells[WHERE_CONDITION_2_ROW_NO, startingWhereVal].Value = tempCond2;
                activeWs.Cells[WHERE_CONDITION_1_ROW_NO, startingWhereVal].Value = tempCond1;

                startingWhereVal++;
                if (whereType == "Between")
                {
                    List<string> betweenCond = new List<string>();
                    betweenCond.Add(tempCond1);
                    betweenCond.Add(tempCond2);
                    whereList.Add(new WhereDTO(backendNameTemp, whereType, betweenCond));
                } else
                {
                    whereList.Add(new WhereDTO(backendNameTemp, whereType, tempCond1));
                }
                
            }
            this.getDataFromOLAP(selectList, calculationList, whereList);
        }

        public void getDataFromOLAP(List<SelectDTO> selectList, List<CalculationDTO> calculationList, List<WhereDTO> whereList)
        {
            throw new NotImplementedException();
        }
    }
}
