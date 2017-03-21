using NJsonSchema;
using System.Collections.Generic;
using System.Text;

namespace Abp.Web.Api.SwaggerTool
{
    public  class Schema4Json
    {
        public string ToSampleJson(JsonSchema4 jsonschemas)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            IList<string> json = new List<string>();
            foreach (var item in jsonschemas.Properties)
            {
                if (!item.Value.IsReadOnly)//readonly is not needed
                {
                    json.Add(GenJsonProperties(item.Value));
                }
                
            }
            builder.Append(string.Join("," , json));
            builder.Append("}");

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
            else if (prop.Type == JsonObjectType.Boolean)
            {
                re += "true";
            }
            else if (prop.Type == JsonObjectType.None)
            {
                re += ToSampleJson(prop.ActualSchema);
            }
            else
            {
                re += "null";
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
