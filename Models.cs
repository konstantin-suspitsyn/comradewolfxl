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

    

    class OlapCube
    {
        public OlapCube() { }
        public int id { get; set; }
        public string name { get; set; }

        public static explicit operator OlapCube(JObject v)
        {
            throw new NotImplementedException();
        }
    }

    class AvailableOlapCubes
    {
        public AvailableOlapCubes() { }
        public OlapCube[] cubes { get; set; }
}

class OlapFieldsDTO
    {
        public OlapFieldsDTO() { }
        public string data_type { get; set; }
        public string field_type { get; set; }
        public string front_name { get; set; }
        public string field_name { get; set; }
    }

public class OlapFieldsProperty
    {
        public OlapFieldsProperty() { }
        public string data_type { get; set; }
        public string field_type { get; set; }
        public string front_name { get; set; }
    }

    class OlapFields
    {
        public OlapFields() {
            this.fields = new Dictionary<string, OlapFieldsProperty>();
        }

        public Dictionary<string, OlapFieldsProperty> fields { get; set; }

    }


    internal class Models
    {
    }
}
