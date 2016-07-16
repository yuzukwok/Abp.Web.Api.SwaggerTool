using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.Web.Api.SwaggerTool.Config;
using Abp.Web.Api.SwaggerTool.Logging;
using Abp.Web.Api.SwaggerTool.SwaggerManager;
using Abp.Web.Api.SwaggerTool.SwaggerManger;
using Swashbuckle.Application;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Abp.Web.Api.SwaggerTool
{
   public class AbpWebApiSwaggerToolModule:AbpModule
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        public override void PreInitialize()
        {
            Logger.Info("AbpWebApiSwaggerToolModule PreInitialize");
            var setting = Its.Configuration.Settings.Get<SwaggerToolSettings>();
            if (setting.enable)
            {
                Logger.Info("SwaggerUi IsEnable");
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
                            c.IncludeXmlComments(HttpRuntime.AppDomainAppPath + "bin\\"+item);
                        }
                    }
                    else if(setting.AutoLoadXmlCommentFiles==true)
                    {
                        //auto load xml
                        foreach (var item in GetXmlCommentsPathDefault())
                        {
                            c.IncludeXmlComments(item);
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

                    //lang file
                    // c.InjectJavaScript(typeof(AbpWebApiSwaggerToolModule).Assembly, "Abp.Web.Api.SwaggerTool.lang.translator.js");
                    //c.InjectJavaScript(typeof(AbpWebApiSwaggerToolModule).Assembly, "Abp.Web.Api.SwaggerTool.lang.zh-CN.js");


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

       

        private static string[] GetXmlCommentsPathDefault()
        {
            return System.AppDomain.CurrentDomain.GetAssemblies()
                .Where(p=>!p.IsDynamic)
                 .Select(p => new FileInfo(p.Location))
                  .Where(p => !p.Name.StartsWith("System") && !p.Name.StartsWith("mscorlib") && !p.Name.StartsWith("Microsoft"))
                  .Select(p => new FileInfo(HttpRuntime.AppDomainAppPath + "bin\\" + p.Name.Replace(p.Extension, "") + ".xml"))
                  .Where(p => p.Exists)
                  .Select(p=>p.FullName)
                  .ToArray();

        }
    }
}
