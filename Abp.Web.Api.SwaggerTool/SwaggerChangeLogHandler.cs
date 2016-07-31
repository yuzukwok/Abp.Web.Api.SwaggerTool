using Abp.Dependency;
using Abp.Web.Api.SwaggerTool.SwaggerManager;
using Newtonsoft.Json;
using Swashbuckle.Application;
using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Abp.Web.Api.SwaggerTool
{
   public class SwaggerChangeLogHandler: HttpMessageHandler
    {
        private readonly SwaggerDocsConfig _config;
       
        public SwaggerChangeLogHandler(SwaggerDocsConfig config)
        {
            _config = config;
          
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var _swaggerstore = IocManager.Instance.Resolve<ISwaggerStore>();
            var setting = Its.Configuration.Settings.Get<Config.SwaggerToolSettings>();
            //using refletions to internal
            var swaggerProvider = (ISwaggerProvider)_config.GetType().GetMethod("GetSwaggerProvider", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).Invoke(_config, new object[] { request });

            var rootUrl = (string)_config.GetType().GetMethod("GetRootUrl", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).Invoke(_config, new object[] { request });
       
            var apiVersion = request.GetRouteData().Values["apiVersion"].ToString();

            try
            {
                var swaggerDoc = swaggerProvider.GetSwagger(rootUrl, apiVersion);
                var last = _swaggerstore.GetLastVersion();
                ApiVersionInfo info = new ApiVersionInfo();
                info.GenDateTime = DateTime.Now.ToString();
                info.SwaggerDoc = JsonConvert.SerializeObject(swaggerDoc, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore, Converters = new[] { new VendorExtensionsConverter() } });
                var md5 = MD5.Create();
                using (var stream =new MemoryStream(Encoding.UTF8.GetBytes(info.SwaggerDoc)))
                {
                    info.md5 = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "");
                }
                if (last!=null)
                {
                    SwaggerDiff diff = new SwaggerDiff();
                   var re= diff.Diff(info.SwaggerDoc, last.SwaggerDoc);
                    //info.BackwardsCompatible = re.Item2;
                    info.ChangeLogs = FormatChangelogDesc(re);
                }

                _swaggerstore.ImportNewVersion(info);              
              
                var content =new StringContent(_swaggerstore.GetChangeLogInfo());
                return TaskFor(new HttpResponseMessage { Content = content });
            }
            catch (UnknownApiVersion ex)
            {
                return TaskFor(request.CreateErrorResponse(HttpStatusCode.NotFound, ex));
            }
        }

        private string FormatChangelogDesc(SwaggerDiffResult re)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var item in re.AddMethods)
            {
                builder.AppendLine(item);
            }
            foreach (var item in re.MissMethods)
            {
                builder.AppendLine(item);
            }
            return builder.ToString();
        }

        private Task<HttpResponseMessage> TaskFor(HttpResponseMessage response)
        {
            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }
    
}
}
