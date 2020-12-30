using Abp.Web.Api.SwaggerTool.Config;
using NSwag;
using NSwag.CodeGeneration.CSharp;

namespace Abp.Web.Api.SwaggerTool.CodeGeneration {
    public class CSharpGen {

        public string Gen(SwaggerDocument service, SwaggerToolSettings setting) {
            var settings = new SwaggerToCSharpClientGeneratorSettings() {
                ClassName = setting.CSharpGen.ClassName,
                CSharpGeneratorSettings = {Namespace = setting.CSharpGen.Namespace}
            };
            var generator = new SwaggerToCSharpClientGenerator(service, settings);
            var code = generator.GenerateFile();

            return code;
        }
    }
}