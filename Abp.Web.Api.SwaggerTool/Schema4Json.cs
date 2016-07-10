using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.Web.Api.SwaggerTool
{
    public  class Schema4Json
    {
        public string ToSampleJson(JsonSchema4 jsonschemas)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{\r\n");
            IList<string> json = new List<string>();
            foreach (var item in jsonschemas.Properties)
            {
                json.Add(GenJsonProperties(item.Value));
            }
            builder.Append(string.Join(",\r\n" , json));
            builder.Append("}\r\n");

            return builder.ToString();
        }

        private string GenJsonProperties(JsonProperty prop)
        {
            string re = "\"" + prop.Name + "\":";
            if (prop.Type == JsonObjectType.String)
            {
                re += "\"" + prop.Type + "\"";
            }
            else if (prop.Type == JsonObjectType.Number || prop.Type == JsonObjectType.Integer)
            {
                re += "0";
            }
            else if (prop.Type == JsonObjectType.Array)
            {
                re += "[" + GenJsonArrayProp(prop.Item) + "]";
            }
            return re;
        }

        private string GenJsonArrayProp(JsonSchema4 schema)
        {
            if (schema.Type== JsonObjectType.String)
            {
                return "\"string\"";
            }
            else if(schema.Type== JsonObjectType.None)
            {
                return ToSampleJson(schema.ActualSchema);
            }
            else
            {
                return "";
            }
        }
    }
}
