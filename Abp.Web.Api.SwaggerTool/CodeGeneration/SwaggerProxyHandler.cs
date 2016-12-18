using Abp.Web.Api.SwaggerTool.Config;
using Newtonsoft.Json;
using Swashbuckle.Application;
using Swashbuckle.Swagger;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;

namespace Abp.Web.Api.SwaggerTool
{
    public class SwaggerProxyHandler : HttpMessageHandler
    {
        private readonly SwaggerDocsConfig _config;
        public SwaggerProxyHandler(SwaggerDocsConfig config)
        {
            _config = config;
        }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var setting = Its.Configuration.Settings.Get<SwaggerToolSettings>();
            //using refletions to internal
            var swaggerProvider= (ISwaggerProvider) _config.GetType().GetMethod("GetSwaggerProvider", System.Reflection.BindingFlags.Instance| System.Reflection.BindingFlags.NonPublic).Invoke(_config,new object[] { request });
          
            var rootUrl = (string)_config.GetType().GetMethod("GetRootUrl", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).Invoke(_config, new object[] { request });
            var type = request.GetRouteData().Values["type"].ToString();

            try
            {
                var swaggerDoc = swaggerProvider.GetSwagger(rootUrl, setting.version);
                var str = JsonConvert.SerializeObject(swaggerDoc, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore,Converters = new[] { new VendorExtensionsConverter() } });
                var service = NSwag.SwaggerDocument.FromJson(str);

                var code = new CodeGeneration.GenCode().Gen(type, service, setting);
                // var content = ContentFor(request, swaggerDoc);
                return TaskFor(new HttpResponseMessage { Content = new StringContent(code) });
            }
            catch (UnknownApiVersion ex)
            {
                return TaskFor(request.CreateErrorResponse(HttpStatusCode.NotFound, ex));
            }
        }

        //private HttpContent ContentFor(HttpRequestMessage request, SwaggerDocument swaggerDoc)
        //{
        //    var negotiator = request.GetConfiguration().Services.GetContentNegotiator();
        //    var result = negotiator.Negotiate(typeof(SwaggerDocument), request, GetSupportedSwaggerFormatters());

        //    return new ObjectContent(typeof(SwaggerDocument), swaggerDoc, result.Formatter, result.MediaType);
        //}

        private IEnumerable<MediaTypeFormatter> GetSupportedSwaggerFormatters()
        {
            var format= (Formatting)_config.GetType().GetMethod("GetFormatting", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).Invoke(_config,null); 
            var jsonFormatter = new JsonMediaTypeFormatter
            {
                SerializerSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    Formatting = format,
                    Converters = new[] { new VendorExtensionsConverter() }
                }
            };
            // NOTE: The custom converter would not be neccessary in Newtonsoft.Json >= 5.0.5 as JsonExtensionData
            // provides similar functionality. But, need to stick with older version for WebApi 5.0.0 compatibility 
            return new[] { jsonFormatter };
        }

        private Task<HttpResponseMessage> TaskFor(HttpResponseMessage response)
        {
            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }
    }
}
