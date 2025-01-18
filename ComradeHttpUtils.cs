using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http.Json;

namespace comradewolfxl
{
    internal class ComradeHttpUtils
    {

        static readonly HttpClient client = new HttpClient();
        private const string HEALTH_CHECK_LINK = "health_check";
        private const string TOKEN_LINK = "v1/user/authenticate";

        ComradeWolfUtils comradeWolfUtils;

        public ComradeHttpUtils()
        {
            comradeWolfUtils = new ComradeWolfUtils();
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




    }
}
