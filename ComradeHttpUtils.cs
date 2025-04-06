using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Text.Json.Nodes;

namespace comradewolfxl
{
    internal class ComradeHttpUtils
    {

        static readonly HttpClient client = new HttpClient();
        private const string HEALTH_CHECK_LINK = "health_check";
        private const string TOKEN_LINK = "v1/user/authenticate";
        private const string AUTH_TOKEN_POSTFIX = "AUTH_TOKEN";
        private const string LIST_OF_ALLOWED_TABLES = "v1/cube/available";
        private const string GET_OLAP_FIELDS = "v1/cube/{0}/front-fields";
        private const string GET_QUERY_DATA = "v1/cube/{0}/query_info";
        private const string GET_DISINCT_FOR_WHERE = "v1/cube/{0}/filter_data";
        private const string GET_ONE_PAGE = "v1/cube/{0}/query_id/{1}?page={2}";

        ComradeWolfUtils comradeWolfUtils;

        public ComradeHttpUtils()
        {
            this.comradeWolfUtils = new ComradeWolfUtils();
        }

        public bool IsTokenValid(string token)
        {
            JwtSecurityToken jwtSecurityToken;
            try
            {
                jwtSecurityToken = new JwtSecurityToken(token);
            }
            catch (Exception)
            {
                return false;
            }

            return jwtSecurityToken.ValidTo > DateTime.UtcNow;
        }

        internal async Task<Dictionary<int, string>> GetCubesAsync(string currentHost, string token)
        {
            Dictionary<int, string> cubes = new Dictionary<int, string>();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync(currentHost + LIST_OF_ALLOWED_TABLES);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            AvailableOlapCubes olapCubes = JsonConvert.DeserializeObject<AvailableOlapCubes>(responseBody);

            foreach (OlapCube olapName in olapCubes.cubes)
            {
                cubes.Add(olapName.id, olapName.name);
            }


            return cubes;
        }

        public string GetAuthPostfix()
        {
            return AUTH_TOKEN_POSTFIX;
        }

        public async Task<bool> healthCheckAsync(string url)
        {

            // Checks if host exists 
            // If host returns 200 Ok, than it's fine

            bool health = false;

            HttpResponseMessage result;

            try
            {
                result = await client.GetAsync(url + HEALTH_CHECK_LINK);

            }
            catch (System.UriFormatException)
            {
                MessageBox.Show("Вы ввели неправильный адрес");
                return health;
            }
            catch (System.Net.Http.HttpRequestException)
            {
                MessageBox.Show("Хост недоступен. Или вы ввели неправильный адрес");
                return health;
            }
            if (result.StatusCode == HttpStatusCode.OK)
            {
                health = true;
            }

            return health;
        }

        public async Task<string> getToken(string url, string username, string password)
        {
            Dictionary<string, string> loginPayload = new Dictionary<string, string>() {
                { "user", username },
                {"password", password }
            };

            var myHttpClient = new HttpClient();
            JsonContent content = JsonContent.Create(loginPayload);
            var response = await myHttpClient.PostAsync(url + TOKEN_LINK, content);

            try
            {
                response.EnsureSuccessStatusCode();
                // Handle success
            }
            catch (HttpRequestException ex) {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedAccessException();
                }  
                throw ex;
            }
            

            string responseBody = await response.Content.ReadAsStringAsync();
            TokenModel tokenFromBackend
                = JsonConvert.DeserializeObject<TokenModel>(responseBody);

            return tokenFromBackend.access_token;
        }

        internal async Task<OlapFields> GetFields(string currentHost, string cube)
        {

            // Returns front fields

            OlapFields frontFields = new OlapFields();

            string token = GetTokenForHost(currentHost);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync(currentHost + string.Format(GET_OLAP_FIELDS, cube));
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();


            //JObject jsonObjects = (JObject)JsonConvert.DeserializeObject(responseBody);

            Dictionary<string, List<OlapFieldsDTO>> values = JsonConvert.DeserializeObject<Dictionary<string, List<OlapFieldsDTO>>>(responseBody);


            // Parse all fielsds
            foreach (OlapFieldsDTO field in values["fields"])
            {
                OlapFieldsProperty olapFieldsProperty = new OlapFieldsProperty();
                olapFieldsProperty.front_name = field.front_name;
                olapFieldsProperty.data_type = field.data_type;
                olapFieldsProperty.field_type = field.field_type;
                frontFields.fields.Add(field.field_name, olapFieldsProperty);
            }

            return frontFields;
        }

        public async Task<DistinctValuesDTO> GetDistinctValues(string fieldName, string host, string cube)
        {

            Dictionary<string, string> whatToSelect = new Dictionary<string, string>{ { "field_name", fieldName }, { "type", "all" } };

            Dictionary<string, Dictionary<string, string>> preJson = new Dictionary<string, Dictionary<string, string>> { { "SELECT_DISTINCT", whatToSelect } };

            string jsonPayload = JsonConvert.SerializeObject(preJson);

            DistinctValuesDTO result = await this.GetDistinctWhereValues(host, jsonPayload, cube);

            return result;
        }

        private async Task<DistinctValuesDTO> GetDistinctWhereValues(string host, string jsonPayload, string cube)
        {
            string token = GetTokenForHost(host);

            var jsonBody = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.PostAsync(host + string.Format(GET_DISINCT_FOR_WHERE, cube), jsonBody);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            DistinctValuesDTO values
                = JsonConvert.DeserializeObject<DistinctValuesDTO>(responseBody);

            return values;
        }

        public async Task<QueryInfoDTO> GetQueryInfo(List<SelectDTO> selectList, List<CalculationDTO> calculationList, List<WhereDTO> whereList, string host, string cube)
        {

            string jsonPayload = createJSONPayloadForOLAP(selectList, calculationList, whereList);
            QueryInfoDTO queryInfoDTO = await this.getQueryInfo(host, jsonPayload, cube);

            return queryInfoDTO;


        }

        public async Task<List<Dictionary<string, object>>> GetPageOfDataFromOlap(string host, string cubeName, long queryId, int pageNo)
        {
            string token = GetTokenForHost(host);
            string completeUrl = host + string.Format(GET_ONE_PAGE, cubeName, queryId, pageNo);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync(completeUrl);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            List<Dictionary<string, object>> responsePreConvert = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(responseBody);

            return responsePreConvert;
        }

        public async Task<QueryInfoDTO> getQueryInfo(string host, string postJson, string cube)
        {
            string token = GetTokenForHost(host);

            var jsonBody = new StringContent(postJson, Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.PostAsync(host + string.Format(GET_QUERY_DATA, cube), jsonBody);
            if (response.StatusCode == HttpStatusCode.NotAcceptable)
            {
                throw new Exception("Слишком много значений в выдаче. Попробуйте добавить фильтры");
            }

            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            QueryInfoDTO tokenFromBackend
                = JsonConvert.DeserializeObject<QueryInfoDTO>(responseBody);

            return tokenFromBackend;


        }

        public string createJSONPayloadForOLAP(List<SelectDTO> selectList, List<CalculationDTO> calculationList, List<WhereDTO> whereList)
        {
            Dictionary<string, List<SelectDTO>> selectPayload = this.createSelectPayloadDictionary(selectList);
            Dictionary<string, List<CalculationDTO>> calculationPayload = this.createCalculationPayload(calculationList);
            Dictionary<string, List<WhereDTO>> wherePayload = this.createWherePayload(whereList);

            string s1 = JsonConvert.SerializeObject(selectPayload);
            s1 = s1.Remove(s1.Length - 1);
            string s2 = JsonConvert.SerializeObject(calculationPayload).Remove(0, 1);
            s2 = s2.Remove(s2.Length - 1);
            string s3 = JsonConvert.SerializeObject(wherePayload);
            s3 = s3.Remove(0, 1);

            string result = s1 + ", " + s2 + ", " + s3;
            return result;

        }

        public Dictionary<string, List<SelectDTO>> createSelectPayloadDictionary(List<SelectDTO> selectList)
        {
            Dictionary<string, List<SelectDTO>> selectPayload = new Dictionary<string, List<SelectDTO>>();
            selectPayload.Add("SELECT", selectList);
            return selectPayload;
        }

        public Dictionary<string, List<CalculationDTO>> createCalculationPayload(List<CalculationDTO> calculationList)
        {
            Dictionary<string, List<CalculationDTO>> calculationPayload = new Dictionary<string, List<CalculationDTO>>();
            calculationPayload.Add("CALCULATION", calculationList);
            return calculationPayload;
        }

        public Dictionary<string, List<WhereDTO>> createWherePayload(List<WhereDTO> whereList)
        {
            Dictionary<string, List<WhereDTO>> where = new Dictionary<string, List<WhereDTO>>();
            where.Add("WHERE", whereList);
            return where;
        }

        public string GetTokenForHost(string currentHost)
        {


            // check for credentials
            string currentToken = comradeWolfUtils.ReadFromRegistry(this.GetAuthPostfix() + currentHost, null);


            if ((currentToken == null) || (!this.IsTokenValid(currentToken)))
            {
                // if not then open login form
                MessageBox.Show("Необходимо залогиниться");
                LoginForm loginForm = new LoginForm();
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show("Тута");

                }

            }


            return currentToken;
        }

    }
}
