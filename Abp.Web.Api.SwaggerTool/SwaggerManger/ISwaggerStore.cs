using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.Web.Api.SwaggerTool.SwaggerManager
{
    public interface ISwaggerStore:ITransientDependency
    {
        void ImportNewVersion(ApiVersionInfo newswaggerdoc);
        ApiVersionInfo GetLastVersion();
        string GetChangeLogInfo();
    }

    public class ApiVersionInfo
    {
        public string GenDateTime { get; set; }
        public string md5 { get; set; }
        public string SwaggerDoc { get; set; }
        public string ChangeLogs { get; set; }
        public bool BackwardsCompatible { get; set; }

    }
}
