﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace comradewolfxl
{

    public class TokenModel
    {
        public TokenModel() { }
        public string access_token { get; set; }
        public string token_type { get; set; }

        public static explicit operator TokenModel(JObject v)
        {
            throw new NotImplementedException();
        }
    }



    public class OlapCube
    {
        public OlapCube() { }
        public int id { get; set; }
        public string name { get; set; }

        public static explicit operator OlapCube(JObject v)
        {
            throw new NotImplementedException();
        }
    }

    public class AvailableOlapCubes
    {
        public AvailableOlapCubes() { }
        public OlapCube[] cubes { get; set; }
}

public class OlapFieldsDTO
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

 public class OlapFields
    {
        public OlapFields() {
            this.fields = new Dictionary<string, OlapFieldsProperty>();
        }

        public Dictionary<string, OlapFieldsProperty> fields { get; set; }

        public string getBackendNameByFrontend(string frontendName)
        {
            foreach(KeyValuePair<string, OlapFieldsProperty> property in this.fields)
            {
                if (property.Value.front_name == frontendName)
                {
                    return property.Key;
                }
            }

            throw new Exception("No such field");
            
        }

    }

    public class Calculations
    {
        public Calculations() {
            this.calculations = new Dictionary<string, string>();

            this.calculations.Add("sum", "Сумма");
            this.calculations.Add("count", "Количество");
            this.calculations.Add("count_distinct", "Количество уникальных");
            this.calculations.Add("min", "Минимум");
            this.calculations.Add("max", "Максимум");
            this.calculations.Add("avg", "Среднее");
            this.calculations.Add("none", "Без вычислений");
            this.calculations.Add("distinct", "Уникальные");

        }

        public Dictionary<string, string> calculations { get; private set; }

        public string getCalculationKeyByValue(string value) { 
            foreach(string key in this.calculations.Keys)
            {
                if (this.calculations[key]==value)
                {
                    return key;
                }
            }
            throw new Exception("No such calculation");
        }

    }

    public class FieldDataTypes
    {
        public FieldDataTypes()
        {
            this.fieldTypes = new Dictionary<string, string>();
            this.fieldTypes.Add("DATE", "date");
            this.fieldTypes.Add("NUMBER", "number");
            this.fieldTypes.Add("DATETIME", "datetime");
            this.fieldTypes.Add("BOOLEAN", "boolean");
            this.fieldTypes.Add("TEXT", "text");

    }

        public Dictionary <string, string> fieldTypes { get; private set; }
    }

    public class WhereTypes
    {
        public WhereTypes() { 
            this.where = new Dictionary<string, string>();

            this.where.Add(">", ">");
            this.where.Add(">=", ">=");
            this.where.Add("<", "<");
            this.where.Add("<=", "<=");
            this.where.Add("В списке", "IN");
            this.where.Add("Не в списке", "NOT IN");
            this.where.Add("Meжду", "BETWEEN");
            this.where.Add("Равно", "=");
            this.where.Add("Не равно", "<>");
            this.where.Add("Содержит", "LIKE");
            this.where.Add("Начинается с", "LIKE");
            this.where.Add("Заканчивается на", "LIKE");


        }

        public Dictionary<string, string> where { get; private set; }
    }

    internal class Models
    {
    }
}
