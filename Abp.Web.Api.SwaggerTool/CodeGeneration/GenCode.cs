using Abp.Web.Api.SwaggerTool.Config;
using NSwag;
using System;
using NSwag.CodeGeneration.TypeScript;

namespace Abp.Web.Api.SwaggerTool.CodeGeneration
{
   public class GenCode
    {
        public string Gen(string type, SwaggerDocument service, SwaggerToolSettings setting)
        {
            switch (type)
            {
                case "CSharp":
                    { return new CSharpGen().Gen(service, setting); }
                case "JQueryCallbacks":
                case "JQueryPromises":
                case "AngularJS":
                case "Angular2":
                    {
                        return new TypeScriptGen().Gen(service, setting,(TypeScriptTemplate)Enum.Parse(typeof(TypeScriptTemplate),type));
                    }
                default:
                    return "Don't support  "+type+" Keyword";
            }
        }
    }
}
