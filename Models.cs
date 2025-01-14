using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace comradewolfxl
{

    class TokenModel
    {
        public TokenModel() { }
        public string access_token { get; set; }
        public string token_type { get; set; }

        public static explicit operator TokenModel(JObject v)
        {
            throw new NotImplementedException();
        }
    }


    internal class Models
    {
    }
}
