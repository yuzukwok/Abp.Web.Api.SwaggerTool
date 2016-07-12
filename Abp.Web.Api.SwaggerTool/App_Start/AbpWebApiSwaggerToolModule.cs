using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.Web.Api.SwaggerTool.Config;
using Abp.Web.Api.SwaggerTool.SwaggerManager;
using Abp.Web.Api.SwaggerTool.SwaggerManger;
using Swashbuckle.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Abp.Web.Api.SwaggerTool
{
   public class AbpWebApiSwaggerToolModule:AbpModule
    {

        public override void PreInitialize()
        {
            var setting = Its.Configuration.Settings.Get<SwaggerToolSettings>();
            if (setting.enable)
            {
                ConfigureSwaggerUi();
            }
           
            base.PreInitialize();
        }
        public override void Initialize()
        {
            if (!IocManager.IsRegistered<ISwaggerStore>())
            {
                IocManager.Register<ISwaggerStore, TextSwaggerStore>();
            }
          

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
                    if (setting.XmlCommentFiles != null)
                    {
                        foreach (var item in setting.XmlCommentFiles)
                        {
                            c.IncludeXmlComments(System.AppDomain.CurrentDomain.BaseDirectory+"//"+item);
                        }
                    }


                    c.DocumentFilter<ApplyDocumentVendorExtensions>();

                    //userful!!!
                    c.EnableSwaggerChangeLog();
                    c.EnableSwaggerProxyGen();
                    c.EnableSwaggerPostmanJsonGen();
                    c.EnableSwaggerSearch();
                  
                })
                .EnableSwaggerUi(c => {

                    //theme
                    if (!string.IsNullOrEmpty(setting.theme))
                    {
                        c.InjectStylesheet(typeof(AbpWebApiSwaggerToolModule).Assembly, "Abp.Web.Api.SwaggerTool.Theme.theme-"+setting.theme+".css");
                    }
                    //customassert
                    if (setting.CustomAssets!=null)
                    {
                        foreach (var item in setting.CustomAssets)
                        {
                            c.CustomAsset(item.name, Assembly.Load(item.assambly), item.resourcename);
                        }
                    }
                    
                    
                });
        }

       

        private static string[] GetXmlCommentsPath()
        {
          return  System.IO.Directory.GetFiles(System.AppDomain.CurrentDomain.BaseDirectory, "*.XML");
           
        }
    }
}
