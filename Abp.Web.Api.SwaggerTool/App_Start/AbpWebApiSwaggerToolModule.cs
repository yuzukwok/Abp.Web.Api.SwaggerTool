using Abp.Configuration.Startup;
using Abp.Modules;
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

            Configuration.Modules.AbpWebApi().HttpConfiguration
                .EnableSwagger(c =>
                {
                    
                    c.SingleApiVersion("v1", "Werounds.WebApi");
                    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                    c.EnableSwaggerProxyGen();
                    //swagger-ui not support apikey
                    //c.ApiKey("SessionKey").In("header");
                    //c.IncludeXmlComments(GetXmlCommentsPath());
                    //c.DocumentFilter<ApplyDocumentVendorExtensions>();
                })
                .EnableSwaggerUi(c => {
                 
                });
        }

        private static string GetXmlCommentsPath()
        {
            return System.String.Format(@"{0}\bin\WeRounds.Core.XML", System.AppDomain.CurrentDomain.BaseDirectory);
        }
    }
}
