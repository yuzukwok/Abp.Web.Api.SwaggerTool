using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.Web.Api.SwaggerTool.Config;
using Swashbuckle.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.Web.Api.SwaggerTool
{
   public class AbpWebApiSwaggerToolModule:AbpModule
    {

        public override void PreInitialize()
        {
            ConfigureSwaggerUi();
            base.PreInitialize();
        }
        public override void Initialize()
        {
            

            base.Initialize();

        }
        private void ConfigureSwaggerUi()
        {
            var setting = Its.Configuration.Settings.Get<SwaggerToolSettings>();

            Configuration.Modules.AbpWebApi().HttpConfiguration
                .EnableSwagger(c =>
                {
                    
                    c.SingleApiVersion(setting.version, setting.title);
                    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                    var files = GetXmlCommentsPath();
                    foreach (var item in files)
                    {
                        c.IncludeXmlComments(item);
                    }
                  
                    c.DocumentFilter<ApplyDocumentVendorExtensions>();


                    c.EnableSwaggerProxyGen();
                    c.EnableSwaggerPostmanJsonGen();
                  
                })
                .EnableSwaggerUi(c => {
                 
                });
        }

        private static string[] GetXmlCommentsPath()
        {
          return  System.IO.Directory.GetFiles(System.AppDomain.CurrentDomain.BaseDirectory, "*.XML");
           
        }
    }
}
