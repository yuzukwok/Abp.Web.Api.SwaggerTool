using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Abp.Web.Api.SwaggerTool.SampleSite.Startup))]

namespace Abp.Web.Api.SwaggerTool.SampleSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
