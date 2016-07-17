using Abp.Web;
using Abp.Web.Api.SwaggerTool.SampleSite.App_Start;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Abp.Web.Api.SwaggerTool.SampleSite
{
    public class WebApiApplication : AbpWebApplication<AbpWebApiSwaggerToolSampleModule>
    {
        protected override void Application_Start(object sender, EventArgs e)
        {
           base.Application_Start(sender, e);
        }
    }
}
