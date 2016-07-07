using Swashbuckle.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Abp.Web.Api.SwaggerTool
{
   public static class HttpConfigurationExtensions
    {
        private static readonly string DefaultRouteTemplate = "swagger/proxy/{type}";
        private static readonly string DefaultRoutePostManTemplate = "swagger/postman";
        public static SwaggerDocsConfig EnableSwaggerProxyGen(this SwaggerDocsConfig swaggerconfig
            )
        {
             
            GlobalConfiguration.Configuration.Routes.MapHttpRoute(
                 name: "swagger_proxy",
                 routeTemplate: DefaultRouteTemplate,
                 defaults: null,  
                 constraints:null,               
                 handler: new SwaggerProxyHandler(swaggerconfig)
             );

            return swaggerconfig;
        }



        public static SwaggerDocsConfig EnableSwaggerPostmanJsonGen(this SwaggerDocsConfig swaggerconfig
            )
        {

            GlobalConfiguration.Configuration.Routes.MapHttpRoute(
                 name: "swagger_postman",
                 routeTemplate: DefaultRoutePostManTemplate,
                 defaults: null,
                 constraints: null,
                 handler: new SwaggerProxyHandler(swaggerconfig)
             );

            return swaggerconfig;
        }
    }
}
