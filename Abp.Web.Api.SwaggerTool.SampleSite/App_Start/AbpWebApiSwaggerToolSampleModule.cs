using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.Web.Api.SwaggerTool.SampleSite.Service;
using Abp.WebApi.Controllers.Dynamic.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Abp.Web.Api.SwaggerTool.SampleSite.App_Start
{
    [DependsOn(typeof(AbpWebApiSwaggerToolModule))]
    public class AbpWebApiSwaggerToolSampleModule:AbpModule
    {
        public override void Initialize()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            Configuration.Modules.AbpWebApi().DynamicApiControllerBuilder.For<ISampleAppService>("app/sample").Build();
            base.Initialize();

        }
    }
}