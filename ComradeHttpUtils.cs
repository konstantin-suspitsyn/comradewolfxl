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

        internal async Task<OlapFields> GetFields(string currentHost, string cube, string token)
        {

            // Returns front fields

            OlapFields frontFields = new OlapFields();

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

        public string GenerateJsonToBackend(List<SelectDTO> selectList, List<CalculationDTO> calculationList, List<WhereDTO> whereList)
        {
            throw new NotImplementedException();

        }


    }
}
