using Abp.Web.Api.SwaggerTool.Config;
using NSwag;
using NSwag.CodeGeneration.TypeScript;

namespace Abp.Web.Api.SwaggerTool.CodeGeneration
{
    public class TypeScriptGen
    {
        public string Gen(SwaggerDocument service, SwaggerToolSettings setting, TypeScriptTemplate Template)
        {
            var settings = new SwaggerToTypeScriptClientGeneratorSettings
            {
                ClassName =setting.TypeScriptGen.ClassName,
                Template = Template,
                PromiseType=PromiseType.Promise,
                TypeScriptGeneratorSettings={ ModuleName= setting.TypeScriptGen.ModuleName }
            };
        

        var generator = new SwaggerToTypeScriptClientGenerator(service, settings);
        var code = generator.GenerateFile();
            return code;
    }
    }
}
