using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Workbook = Microsoft.Office.Interop.Excel.Workbook;
using Worksheet = Microsoft.Office.Interop.Excel.Worksheet;

namespace comradewolfxl
{
    public partial class SelectAndWhere : Form
    {

        private const int heightOfSelectForm = 40;
        private const int heightOfWhereForm = 100;

        private const int HOST_ADDRESS = 1;
        private const int OLAP_CUBE_NAME = 2;

        private const int WHERE_FRONT_ROW_NO = 3;
        private const int WHERE_BACK_ROW_NO = 4;
        private const int WHERE_TYPE_ROW_NO = 5;
        private const int WHERE_CONDITION_1_ROW_NO = 6;
        private const int WHERE_CONDITION_2_ROW_NO = 7;
        private const int SELECT_FRONT_ROW_NO = 8;
        private const int SELECT_BACK_ROW_NO = 9;
        private const int SELECT_CALCULATION_ROW_NO = 10;


        private const int HEADER_ROW_NO = 12;

        // TODO: DELETE AND MOVE IT TO SERVICE
        private const string FIELD_NAME_WITH_CALC = "{0}__{1}";


        private OlapFields frontFields;
        private string cubeName;
        private string currentHost;
        private ComradeHttpUtils comradeHttpUtils;
        private List<SelectItemPiece> selectItems;
        private List<WhereItem> whereItems;
        private int selectIndex = 0;
        private int whereIndex = 0;
        private Calculations calculations;
        private WhereTypes whereTypes;

        private bool isUpdate = false;

        private ComradeService comradeService = new ComradeService();


        public SelectAndWhere(OlapFields frontFields, string cubeName, string currentHost)
        {
            InitializeComponent();

            this.vanillaInit(frontFields, cubeName, currentHost);

            this.isUpdate = false;
            
        }

        public void vanillaInit(OlapFields frontFields, string cubeName, string currentHost)
        {
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

        public SelectAndWhere(OlapFields frontFields, List<SelectDTO> selectList, List<CalculationDTO> calculationList, List<WhereDTO> whereList, string hostName, string cubeName, List<string> selectAndCalculations)
        {
            InitializeComponent();

            this.vanillaInit(frontFields, cubeName, hostName);

            this.isUpdate = true;

            foreach (string selectCalcItem in selectAndCalculations)
            {
                Tuple<string, string, bool> typeOfSelectOrCalculation = comradeService.getTypeOfSelectOrCalculationItem(selectCalcItem);

                string backendName = typeOfSelectOrCalculation.Item1;
                string calculationType = typeOfSelectOrCalculation.Item2;
                bool isCalculation = typeOfSelectOrCalculation.Item3;

                SelectItemPiece selectItemPiece = addSelectItem();
                selectItemPiece.setSelectItem(this.frontFields.fields[backendName].front_name);

                if (isCalculation)
                {
                    // Add calculation
                    selectItemPiece.setCalculationType(this.calculations.calculations[calculationType]);
                }
            }

            foreach(WhereDTO where in whereList)         
            {
                WhereItem whereItem = this.addWhereItem();
                string backendName = where.field_name;
                string frontName = this.frontFields.fields[backendName].front_name;
                string whereTypeBack = where.where;
                string whereTypeFront = whereTypes.getWhereFrontByBack(whereTypeBack);

                string condition1 = "";
                string condition2 = "";

                if (whereTypeBack.Equals("BETWEEN"))
                {
                    if (where.condition.GetType().GetGenericTypeDefinition() == typeof(List<>)) {

                        List<string> whereCond = (List<string>) where.condition;
                        condition1 = whereCond[0];
                        condition2 = whereCond[1];
                    }
                } else
                {
                    condition1 = (string) where.condition;
                }


                whereItem.addItem(frontName, whereTypeFront, condition1, condition2);
            }

        }

        private SelectItemPiece addSelectItem()
        {
            SelectItemPiece selectItemPiece = new SelectItemPiece(selectIndex, frontFields);
            this.selectPanel.Controls.Add(selectItemPiece);

            selectItemPiece.Location = new System.Drawing.Point(0, selectIndex * heightOfSelectForm);
            this.selectIndex++;

            return selectItemPiece;
        }


        private void addSelect_Click(object sender, EventArgs e)
        {

            addSelectItem();


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
            addWhereItem();
        }

        private WhereItem addWhereItem()
        {
            WhereItem whereItem = new WhereItem(whereIndex, frontFields, cubeName, currentHost); ;
            this.panelWhere.Controls.Add(whereItem);

            whereItem.Location = new System.Drawing.Point(0, whereIndex * heightOfWhereForm);
            whereIndex++;

            return whereItem;
        }



        private async void createCube_ClickAsync(object sender, EventArgs e)
        {
            this.createCube.Enabled = false;
            // TODO: Make form to use current worksheet or create new
            // Now we create new worksheet
            Workbook wb = (Workbook)Globals.ThisAddIn.Application.ActiveWorkbook;
            if (!isUpdate) {
                wb.Worksheets.Add();
            }
            

            // Ordered list of all selected and calculated objects with types
            List<KeyValuePair<string, string>> itemsToBeConverted = new List<KeyValuePair<string, string>>();

            Worksheet activeWs = Globals.ThisAddIn.Application.ActiveSheet;
            MessageBox.Show(activeWs.Name);

            // It will completely clear sheet on cube change
            if (isUpdate)
            {
                activeWs.Cells.Clear();
            }

            activeWs.Cells[HOST_ADDRESS, 1].Value = this.currentHost;
            activeWs.Cells[OLAP_CUBE_NAME, 1].Value = this.cubeName;


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
                string backendType = this.frontFields.getFieldTypeByFrontend(tempFrontendName);

                activeWs.Cells[SELECT_FRONT_ROW_NO, startingSelectVal].Value = tempFrontendName;
                activeWs.Cells[SELECT_BACK_ROW_NO, startingSelectVal].Value = backendName;
                activeWs.Cells[SELECT_CALCULATION_ROW_NO, startingSelectVal].Value = backendCalculation;

                // Header of pivot
                activeWs.Cells[HEADER_ROW_NO, startingSelectVal].Value = tempFrontendName;
                activeWs.Cells[HEADER_ROW_NO, startingSelectVal].Font.Color = XlRgbColor.rgbWhite;
                activeWs.Cells[HEADER_ROW_NO, startingSelectVal].Interior.Color = XlRgbColor.rgbBlue;


                if (backendCalculation == "none")
                {
                    selectList.Add(new SelectDTO(backendName));
                    itemsToBeConverted.Add(new KeyValuePair<string, string> (backendName, backendType ));

                }
                else
                {
                    calculationList.Add(new CalculationDTO(backendName, backendCalculation));
                    itemsToBeConverted.Add(new KeyValuePair<string, string> (string.Format(FIELD_NAME_WITH_CALC, backendName, backendCalculation), "number" ));

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
                if (whereType == "BETWEEN")
                {
                    List<string> betweenCond = new List<string>();
                    betweenCond.Add(tempCond1);
                    betweenCond.Add(tempCond2);
                    whereList.Add(new WhereDTO(backendNameTemp, whereType, betweenCond));
                } else if (whereType == "IN")
                {
                    List<string> inCond = System.Text.RegularExpressions.Regex.Split(tempCond1, @";(?<!\\;)").ToList();
                    whereList.Add(new WhereDTO(backendNameTemp, whereType, inCond));

                } 
                
                else
                {
                    whereList.Add(new WhereDTO(backendNameTemp, whereType, tempCond1));
                }
                
            }
            try
            {
                await this.getDataFromOLAPAsync(selectList, calculationList, whereList, itemsToBeConverted);

            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.createCube.Enabled = true;
            }

        }

        public async Task getDataFromOLAPAsync(List<SelectDTO> selectList, List<CalculationDTO> calculationList, List<WhereDTO> whereList, List<KeyValuePair<string, string>> itemsToBeConverted)
        {

            Worksheet activeWs = Globals.ThisAddIn.Application.ActiveSheet;

            QueryInfoDTO queryInfoDTO = await comradeHttpUtils.GetQueryInfo(selectList, calculationList, whereList, this.currentHost, this.cubeName);
            int pages = queryInfoDTO.pages;
            int itemsPerPage = queryInfoDTO.items_per_page;
            long queryId = queryInfoDTO.id;

            int rowNo = HEADER_ROW_NO;

            for (int pageNo = 0; pageNo < pages; pageNo++)
            {

                List<Dictionary<string, object>> dataFromOLAP = await comradeHttpUtils.GetPageOfDataFromOlap(currentHost, cubeName, queryId, pageNo);
                
                int bulkRows = dataFromOLAP.Count;
                int bulkColumns = itemsToBeConverted.Count;

                var startCell = (Microsoft.Office.Interop.Excel.Range)activeWs.Cells[rowNo + 1, 1];

                // BulkInsert https://brandewinder.com/2010/10/17/Write-data-to-an-Excel-worksheet-with-C-fast/
                var bulkData = new object[bulkRows, bulkColumns];
                foreach (Dictionary<string, object> row in dataFromOLAP)
                {
                    int columnNo = 0;

                    foreach(KeyValuePair<string, string> item in itemsToBeConverted)
                    {
                        bulkData[rowNo - (HEADER_ROW_NO) - pageNo * itemsPerPage, columnNo] = row[item.Key];

                        columnNo++;
                    }
                    rowNo++;
                }

                var endCell = (Microsoft.Office.Interop.Excel.Range)activeWs.Cells[rowNo, itemsToBeConverted.Count];
                var writeRange = activeWs.get_Range(startCell, endCell);

                writeRange.Value = bulkData;
            }

            MessageBox.Show("Done");
            this.Close();
        }
    }
}
