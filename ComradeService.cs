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

        public void createSelectDialog()
        {
            Tuple<List<SelectDTO>, List<CalculationDTO>, List<WhereDTO>, string, string, List<string>> currentCubeData = utils.gatherExistingCubeData();
            List<SelectDTO> selectList = currentCubeData.Item1;
            List<CalculationDTO> calculationList = currentCubeData.Item2;
            List<WhereDTO> whereList = currentCubeData.Item3;
            string hostName = currentCubeData.Item4;
            string cubeName = currentCubeData.Item5;

            // TODO: Check host and create token

            //SelectCube selectCube = new SelectAndWhere(selectList, calculationList, whereList, hostName, cubeName);
            //selectCube.ShowDialog();
        }

        public void chectHostAndToken(string hostName)
        {
            // Checks if host exists
        }

    }
}
