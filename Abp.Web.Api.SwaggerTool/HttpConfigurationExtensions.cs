using Abp.Web.Api.SwaggerTool.Postman;
using Swashbuckle.Application;
using System.Web.Http;

namespace Abp.Web.Api.SwaggerTool
{
   public static class HttpConfigurationExtensions
    {
        private static readonly string DefaultRouteTemplate = "swagger/proxy/{type}";
        private static readonly string DefaultRoutePostManTemplate = "swagger/postman";
        private static readonly string DefaultRouteSearchTemplate = "swagger/docs/{apiVersion}/{q}";
        private static readonly string DefaultRouteChangeLogTemplate = "swagger/changelogs/{apiVersion}";

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
        public static SwaggerDocsConfig EnableSwaggerSearch(this SwaggerDocsConfig swaggerconfig
            )
        {

            GlobalConfiguration.Configuration.Routes.MapHttpRoute(
                 name: "swagger_search",
                 routeTemplate: DefaultRouteSearchTemplate,
                  defaults: null,
                constraints: new { apiVersion = @".+",q=".+" },
                 handler: new SwaggerSearchHandler(swaggerconfig)
             );

            return swaggerconfig;
        }


        public static SwaggerDocsConfig EnableSwaggerChangeLog(this SwaggerDocsConfig swaggerconfig
          )
        {

            GlobalConfiguration.Configuration.Routes.MapHttpRoute(
                 name: "swagger_changelog",
                 routeTemplate: DefaultRouteChangeLogTemplate,
                  defaults: null,
                constraints: new { apiVersion = @".+" },
                 handler: new SwaggerChangeLogHandler(swaggerconfig)
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
                 handler: new SwaggerPostmanHandler(swaggerconfig)
             );

            return swaggerconfig;
        }


    }
}
