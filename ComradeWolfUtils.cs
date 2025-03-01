using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace comradewolfxl
{
    internal class ComradeWolfUtils
    {

        private const string FIELD_NAME_WITH_CALC = "{0}__{1}";

        private const int heightOfSelectForm = 40;
        private const int heightOfWhereForm = 70;

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


        private const string REGISTRY_PATH = @"Comradewolf\Comradewolf\XLaddin";

        // В файле хранится список хостов бэкенда
        private string pathHost = "comradewolf\\hosts.xml";

        public void saveHostInfo(Dictionary<int, string> hostList)
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), this.pathHost);
            Directory.CreateDirectory(Path.GetDirectoryName(path));

            MessageBox.Show(path);


            List<Entry> entries = new List<Entry>();

            foreach (var host in hostList)
            {
                Entry entry = new Entry();
                entry.Key = host.Key;
                entry.Value = host.Value;
                entries.Add(entry);
            }


            XmlSerializer serializer = new XmlSerializer(typeof(List<Entry>));
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                serializer.Serialize(fileStream, entries);
            }
        }

        public void StoreInRegistry(string keyName, string value)
        {
            RegistryKey rootKey = Registry.CurrentUser;
            string registryPath = REGISTRY_PATH;
            using (RegistryKey rk = rootKey.CreateSubKey(registryPath))
            {
                rk.SetValue(keyName, value, RegistryValueKind.String);
            }
        }

        public void DeleteFromRegistry(string keyName)
        {
            RegistryKey rootKey = Registry.CurrentUser;
            string registryPath = REGISTRY_PATH;

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(registryPath, true))
            {
                if (key.GetValue(keyName) != null)
                {
                    key.DeleteValue(keyName);
                }

            }

        }

        public bool fileExistis(string path)
        {
            return File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), path));
        }


        public Dictionary<int, string> getHostInfo()
        {
            // Deserilizes object with host name

            // 0 — selected host
            // 1-... other available options
            Dictionary<int, string> hosts = new Dictionary<int, string>();

            if (this.fileExistis(this.pathHost))
            {
                var serializer = new XmlSerializer(typeof(List<Entry>));

                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), this.pathHost);

                List<Entry> list;

                using (var fileStream = new FileStream(path, FileMode.Open))
                {
                    if (fileStream.Length == 0)
                    {
                        return hosts;
                    }
                    list = (List<Entry>)serializer.Deserialize(fileStream);
                }

                foreach (Entry entry in list)
                {
                    hosts[(int)entry.Key] = (string)entry.Value;
                }

            }

            return hosts;
        }

        public Dictionary<int, string> checkHost(string currentHost)
        {
            // currentHost - host that will have 0 int in dictionary
            /**
             * Creates Dictionary of hosts
             * [0] is oldest link
             */
            Dictionary<int, string> oldHosts = getHostInfo();

            if (oldHosts.ContainsKey(0) && oldHosts[0] == currentHost)
            {
                return oldHosts;
            }

            Dictionary<int, string> newHosts = new Dictionary<int, string>();

            newHosts[0] = currentHost;

            int i = 1;

            foreach (var host in oldHosts)
            {
                if (host.Value != currentHost)
                {
                    newHosts[i] = host.Value;
                    i++;
                }
            }

            return newHosts;
        }

        public string ReadFromRegistry(string keyName, string defaultValue)
        {
            RegistryKey rootKey = Registry.CurrentUser;
            string registryPath = REGISTRY_PATH;
            using (RegistryKey rk = rootKey.OpenSubKey(registryPath, false))
            {
                if (rk == null)
                {
                    return defaultValue;
                }

                var res = rk.GetValue(keyName, defaultValue);
                if (res == null)
                {
                    return defaultValue;
                }

                return res.ToString();
            }
        }

        public int getLastRow(Worksheet ws)
        {
            return ws.Cells.Find("*", SearchOrder: XlSearchOrder.xlByRows, SearchDirection: XlSearchDirection.xlPrevious).Row;
        }

        public void clearOLAPData(int rowsWithOlapData, bool includeHeader)
        {
            int startRow = HEADER_ROW_NO;

            if (includeHeader)
            {
                startRow++;
            }

            Workbook wb = (Workbook)Globals.ThisAddIn.Application.ActiveWorkbook;

            Worksheet activeWs = Globals.ThisAddIn.Application.ActiveSheet;

            var startCell = (Microsoft.Office.Interop.Excel.Range) activeWs.Cells[startRow, 1];
            var endCell = (Microsoft.Office.Interop.Excel.Range) activeWs.Cells[getLastRow(activeWs), rowsWithOlapData];

            activeWs.get_Range(startCell, endCell).ClearContents();
        }

        public string generateCalculationField(string backendName, string backendCalculation)
        {
            return string.Format(FIELD_NAME_WITH_CALC, backendName, backendCalculation);
        }

        public Tuple<List<SelectDTO>, List<CalculationDTO>, List<WhereDTO>, string, string, List<string>> gatherExistingCubeData()
        {

            Workbook wb = (Workbook)Globals.ThisAddIn.Application.ActiveWorkbook;

            Worksheet activeWs = Globals.ThisAddIn.Application.ActiveSheet;

            string olapCubeName;
            string hostName;

            List<SelectDTO> selectList = new List<SelectDTO>();
            List<CalculationDTO> calculationList = new List<CalculationDTO>();
            List<WhereDTO> whereList = new List<WhereDTO>();
            // Ordered list of all selected and calculated objects with types
            List<string> selectAndCalcNames = new List<string>();

            if (activeWs.Cells[1, 1].Value==null)
            {
                MessageBox.Show("Хост отсутствует");
                throw new Exception("Хост отсутствует");
            }

            hostName = activeWs.Cells[1, 1].Value;


            if (activeWs.Cells[2, 1].Value == null)
            {
                MessageBox.Show("Куб отсутствует");
                throw new Exception("Куб отсутствует");
            }

            olapCubeName = activeWs.Cells[2, 1].Value;


            // TODO: все, что формирует списки в отдельный метод

            // SELECT AND CALCULATION
            for (int i=1; i<1000; i++)
            {
                if (activeWs.Cells[SELECT_BACK_ROW_NO, i].Value == null)
                {
                    break;
                }
                string backendName = activeWs.Cells[SELECT_BACK_ROW_NO, i].Value;

                if (activeWs.Cells[SELECT_CALCULATION_ROW_NO, i].Value == "none")
                {
                    selectList.Add(new SelectDTO(backendName));
                    selectAndCalcNames.Add(backendName);
                } else
                {
                    string calculation = activeWs.Cells[SELECT_CALCULATION_ROW_NO, i].Value;
                    calculationList.Add(new CalculationDTO(backendName, calculation));

                    selectAndCalcNames.Add(generateCalculationField(backendName, calculation));

                }

            }

            // WHERE
            for (int i = 1; i < 1000; i++)
            {
                if (activeWs.Cells[WHERE_BACK_ROW_NO, i].Value == null)
                {
                    break;
                }

                if (activeWs.Cells[WHERE_TYPE_ROW_NO].Value == "between")
                {
                    List<string> listWhereCondTemp = new List<string>();
                    if (activeWs.Cells[WHERE_CONDITION_1_ROW_NO, i].Value == null | activeWs.Cells[WHERE_CONDITION_2_ROW_NO, i].Value == null)
                    {
                        MessageBox.Show("Проблема с условиями WHERE");
                        throw new Exception("Проблема с условиями WHERE");
                    }

                    listWhereCondTemp.Add(activeWs.Cells[WHERE_CONDITION_1_ROW_NO, i].Value);
                    listWhereCondTemp.Add(activeWs.Cells[WHERE_CONDITION_2_ROW_NO, i].Value);

                    whereList.Add(new WhereDTO(activeWs.Cells[WHERE_BACK_ROW_NO, i].Value, activeWs.Cells[WHERE_TYPE_ROW_NO, i].Value, listWhereCondTemp));

                }

                string whereBackTemp = activeWs.Cells[WHERE_BACK_ROW_NO, i].Value;
                string whereTypeTemp = activeWs.Cells[WHERE_TYPE_ROW_NO, i].Value;
                string whereCondTemp = activeWs.Cells[WHERE_CONDITION_1_ROW_NO, i].Value;

                whereList.Add(new WhereDTO(whereBackTemp, whereTypeTemp, whereCondTemp));

                //TODO: ADD WHERE

            }
            return new Tuple<List<SelectDTO>, List<CalculationDTO>, List<WhereDTO>, string, string, List<string>>(selectList, calculationList, whereList, hostName, olapCubeName, selectAndCalcNames);
        }

        internal void writeDataPieceToSheet(List<Dictionary<string, object>> dataFromOLAP, int pageNo, int itemsPerPage, int rowNoToInsert, int colNoToInsert, List<string> selectAndCalculations)
        {

            Workbook wb = (Workbook)Globals.ThisAddIn.Application.ActiveWorkbook;

            Worksheet activeWs = Globals.ThisAddIn.Application.ActiveSheet;

            int rowNo = pageNo * itemsPerPage + HEADER_ROW_NO;

            var startCell = (Microsoft.Office.Interop.Excel.Range)activeWs.Cells[rowNo + 1, 1];

            // BulkInsert https://brandewinder.com/2010/10/17/Write-data-to-an-Excel-worksheet-with-C-fast/
            var bulkData = new object[rowNoToInsert, colNoToInsert];
            foreach (Dictionary<string, object> row in dataFromOLAP)
            {
                int columnNo = 0;

                foreach (string item in selectAndCalculations)
                {
                    bulkData[rowNo - (HEADER_ROW_NO) - pageNo * itemsPerPage, columnNo] = row[item];

                    columnNo++;
                }
                rowNo++;
            }

            var endCell = (Microsoft.Office.Interop.Excel.Range)activeWs.Cells[rowNo, selectAndCalculations.Count];
            var writeRange = activeWs.get_Range(startCell, endCell);

            writeRange.Value = bulkData;
        }
    }

    public class Entry
    {
        public object Key;
        public object Value;
        public Entry()
        {
        }

        public Entry(object key, object value)
        {
            Key = key;
            Value = value;
        }
    }


}
