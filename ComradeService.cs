using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace comradewolfxl
{
    // Combines ComradeWolfUtils and ComradeHttpUtils
    internal class ComradeService
    {
        ComradeHttpUtils httpUtils = new ComradeHttpUtils();
        ComradeWolfUtils utils = new ComradeWolfUtils();

        public string EscapeSeparator(string theWord)
        {
            return utils.escapeSeparator(theWord);
        }

        public async Task<DistinctValuesDTO> GetWhereHelp(string fieldName, string host, string cube)
        {
            return await httpUtils.GetDistinctValues(fieldName, host, cube);
        }

        public async Task<Tuple<int, int, long>> GetQueryInfo(List<SelectDTO> selectList, List<CalculationDTO> calculationList, List<WhereDTO> whereList, string currentHost, string cubeName)
        {
            QueryInfoDTO queryInfoDTO = await httpUtils.GetQueryInfo(selectList, calculationList, whereList, currentHost, cubeName);
            int pages = queryInfoDTO.pages;
            int itemsPerPage = queryInfoDTO.items_per_page;
            long queryId = queryInfoDTO.id;

            return new Tuple<int, int, long>  (pages, itemsPerPage, queryId) ;
        }

        public async Task updateOlapDataOnSheet()
        {

            Tuple<List<SelectDTO>, List<CalculationDTO>, List<WhereDTO>, string, string, List<string>> currentCubeData = utils.gatherExistingCubeData();
            List<SelectDTO> selectList = currentCubeData.Item1;
            List<CalculationDTO> calculationList = currentCubeData.Item2;
            List<WhereDTO> whereList = currentCubeData.Item3;
            string hostName = currentCubeData.Item4;
            string cubeName = currentCubeData.Item5;
            List<string> selectAndCalculations = currentCubeData.Item6;

            Tuple<int, int, long> queryInfo = await this.GetQueryInfo(selectList, calculationList, whereList, hostName, cubeName);
            int pages = queryInfo.Item1;
            int itemsPerPage = queryInfo.Item2;
            long queryId = queryInfo.Item3;

            utils.clearOLAPData(selectList.Count + calculationList.Count, true);

            await this.insertDataToSheet(pages, itemsPerPage, queryId, hostName, cubeName, selectAndCalculations);

        }

        public async Task insertDataToSheet(int pages, int itemsPerPage, long queryId, string currentHost, string cubeName, List<string>selectAndCalculations)
        {
            for (int pageNo = 0; pageNo < pages; pageNo++)
            {

                List<Dictionary<string, object>> dataFromOLAP = await httpUtils.GetPageOfDataFromOlap(currentHost, cubeName, queryId, pageNo);

                int bulkRows = dataFromOLAP.Count;
                int bulkColumns = selectAndCalculations.Count;

                utils.writeDataPieceToSheet(dataFromOLAP, pageNo, itemsPerPage, bulkRows, bulkColumns, selectAndCalculations);

                
            }
        }

        public async void createSelectDialog()
        {
            Tuple<List<SelectDTO>, List<CalculationDTO>, List<WhereDTO>, string, string, List<string>> currentCubeData = utils.gatherExistingCubeData();
            List<SelectDTO> selectList = currentCubeData.Item1;
            List<CalculationDTO> calculationList = currentCubeData.Item2;
            List<WhereDTO> whereList = currentCubeData.Item3;
            string hostName = currentCubeData.Item4;
            string cubeName = currentCubeData.Item5;
            List<string> selectAndCalculations = currentCubeData.Item6;

            bool isLogged = chectHostAndToken(hostName);
            
            if (isLogged)
            {
                OlapFields olapFields = await httpUtils.GetFields(hostName, cubeName);
                SelectAndWhere selectAndWhere = new SelectAndWhere(olapFields, selectList, calculationList, whereList, hostName, cubeName, selectAndCalculations);
                selectAndWhere.ShowDialog();
            }


        }

        public bool chectHostAndToken(string hostName)
        {
            // Checks if host exists
            utils.checkHost(hostName);

            string currentToken = utils.ReadFromRegistry(httpUtils.GetAuthPostfix() + hostName, null);


            if ((currentToken == null) || (!httpUtils.IsTokenValid(currentToken)))
            {
                // if not then open login form
                MessageBox.Show("Необходимо залогиниться");
                LoginForm loginForm = new LoginForm(hostName);

                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    chectHostAndToken(hostName);
                } else
                {
                    MessageBox.Show("Процедура авторизации не пройдена");
                    return false;
                }
            }

            return true; 
 
        }

        public Tuple<string, string, bool> getTypeOfSelectOrCalculationItem(string selectAndCalculations)
        {
            string fieldName;
            string calculation = "none";
            bool isCalculation = false;

            if (selectAndCalculations.Contains("__")) {
                string[] subs = selectAndCalculations.Split(new string[] { "__" }, StringSplitOptions.None);
                if (subs.Length != 2) { throw new Exception("Название поля некорректное"); }
                fieldName = subs[0];
                calculation = subs[1];
                isCalculation = true; 
            } else
            {
                fieldName = selectAndCalculations;
            }

            return new Tuple<string, string, bool>(fieldName, calculation, isCalculation);

        }
    }
}
