using Swashbuckle.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Swashbuckle.Swagger;
using System.Net;
using System.Net.Http.Formatting;
using Newtonsoft.Json;
using System.Web.Http;

namespace Abp.Web.Api.SwaggerTool
{
    public class SwaggerSearchHandler: HttpMessageHandler
    {
        private readonly SwaggerDocsConfig _config;
        public SwaggerSearchHandler(SwaggerDocsConfig config)
        {
            _config = config;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var setting = Its.Configuration.Settings.Get<Config.SwaggerToolSettings>();
            //using refletions to internal
            var swaggerProvider = (ISwaggerProvider)_config.GetType().GetMethod("GetSwaggerProvider", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).Invoke(_config, new object[] { request });

            var rootUrl = (string)_config.GetType().GetMethod("GetRootUrl", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).Invoke(_config, new object[] { request });
            var q = request.GetRouteData().Values["q"].ToString();
            var apiVersion = request.GetRouteData().Values["apiVersion"].ToString();

            try
            {
                var swaggerDoc = swaggerProvider.GetSwagger(rootUrl, apiVersion);

                //筛选条件为path包含q
                IList<string> keytoremove = new List<string>();
                foreach (var item in swaggerDoc.paths)
                {
                    if (!item.Key.ToUpper().Contains(q.ToUpper()))
                    {
                        keytoremove.Add(item.Key);
                    }
                }
                foreach (var item in keytoremove)
                {
                    swaggerDoc.paths.Remove(item);
                }
                var content = ContentFor(request, swaggerDoc);
                return TaskFor(new HttpResponseMessage { Content = content });
            }
            catch (UnknownApiVersion ex)
            {
                return TaskFor(request.CreateErrorResponse(HttpStatusCode.NotFound, ex));
            }
        }

        private HttpContent ContentFor(HttpRequestMessage request, SwaggerDocument swaggerDoc)
        {
            var negotiator = request.GetConfiguration().Services.GetContentNegotiator();
            var result = negotiator.Negotiate(typeof(SwaggerDocument), request, GetSupportedSwaggerFormatters());

            return new ObjectContent(typeof(SwaggerDocument), swaggerDoc, result.Formatter, result.MediaType);
        }

        private IEnumerable<MediaTypeFormatter> GetSupportedSwaggerFormatters()
        {
            var format = (Formatting)_config.GetType().GetMethod("GetFormatting", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).Invoke(_config, null);
            var jsonFormatter = new JsonMediaTypeFormatter
            {
                SerializerSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    Formatting =format,
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
