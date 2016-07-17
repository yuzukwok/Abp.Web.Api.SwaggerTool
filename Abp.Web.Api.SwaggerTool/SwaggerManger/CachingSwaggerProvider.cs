using Swashbuckle.Swagger;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.Web.Api.SwaggerTool.SwaggerManger
{
    public class CachingSwaggerProvider : ISwaggerProvider
    {
        private static ConcurrentDictionary<string, SwaggerDocument> _cache =
            new ConcurrentDictionary<string, SwaggerDocument>();

        private readonly ISwaggerProvider _swaggerProvider;

        public CachingSwaggerProvider(ISwaggerProvider swaggerProvider)
        {
            _swaggerProvider = swaggerProvider;
        }

        public SwaggerDocument GetSwagger(string rootUrl, string apiVersion)
        {
            var cacheKey = string.Format("{0}_{1}", rootUrl, apiVersion);
            return _cache.GetOrAdd(cacheKey, (key) => _swaggerProvider.GetSwagger(rootUrl, apiVersion));
        }
    }
}
