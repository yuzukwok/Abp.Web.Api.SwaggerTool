using Abp.Web.Api.SwaggerTool.Config;
using NSwag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSwag.CodeGeneration.CodeGenerators.TypeScript;

namespace Abp.Web.Api.SwaggerTool.CodeGeneration
{
    public class TypeScriptGen
    {
        public string Gen(SwaggerService service, SwaggerToolSettings setting, TypeScriptTemplate Template)
        {
            var settings = new SwaggerToTypeScriptClientGeneratorSettings
            {
                ClassName =setting.TypeScriptGen.ClassName,
                Template = Template,
                PromiseType=PromiseType.Promise,
                TypeScriptGeneratorSettings=new NJsonSchema.CodeGeneration.TypeScript.TypeScriptGeneratorSettings() { ModuleName= setting.TypeScriptGen.ModuleName }
            };
        

        var generator = new SwaggerToTypeScriptClientGenerator(service, settings);
        var code = generator.GenerateFile();
            return code;
    }
    }
}
