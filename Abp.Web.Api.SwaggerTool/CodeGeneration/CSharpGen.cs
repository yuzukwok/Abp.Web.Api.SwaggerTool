using Abp.Web.Api.SwaggerTool.Config;
using NSwag;
using NSwag.CodeGeneration.CodeGenerators.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.Web.Api.SwaggerTool.CodeGeneration
{
  public  class CSharpGen
    {

        public string Gen(SwaggerService service, SwaggerToolSettings setting)
        {
            var settings = new SwaggerToCSharpClientGeneratorSettings
            {
                ClassName = setting.CSharpGen.ClassName,
                 CSharpGeneratorSettings=new NJsonSchema.CodeGeneration.CSharp.CSharpGeneratorSettings() {  Namespace= setting.CSharpGen.Namespace}
            };

            var generator = new SwaggerToCSharpClientGenerator(service, settings);
            var code = generator.GenerateFile();

            return code;
        }
    }
}
