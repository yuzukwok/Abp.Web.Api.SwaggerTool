using Abp.Web.Api.SwaggerTool.SwaggerManager;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace Abp.Web.Api.SwaggerTool.SwaggerManger
{
    public class TextSwaggerStore : ISwaggerStore
    {
        private readonly string basepath;
        public TextSwaggerStore()
        {
            basepath = System.AppDomain.CurrentDomain.BaseDirectory;

        }
        public string filefolder = "/swaggerfiles";
        public string GetChangeLogInfo()
        {
           return File.ReadAllText(basepath+"/"+filefolder + "/changelogs.json");
        }

        public ApiVersionInfo GetLastVersion()
        {
            var dictory = new System.IO.DirectoryInfo(basepath + "\\" + filefolder);
            if (!dictory.Exists)
            {
                dictory.Create();
            }
           var file= dictory.GetFiles()
                .Where(p=>p.Name.Contains("swagger"))
                .OrderByDescending(p => p.LastWriteTime)
                .FirstOrDefault();
            if (file ==null)
            {
                return null;
            }
            else
            {
                var txt = File.ReadAllText(file.FullName);
                var api = JsonConvert.DeserializeObject<ApiVersionInfo>(txt);
                return api;
            }
           
        }

        public void ImportNewVersion(ApiVersionInfo newswaggerdoc)
        {
            var last = GetLastVersion();
            if (last==null||last.md5!=newswaggerdoc.md5)
            {
                File.WriteAllText(basepath + "/"+filefolder + "/swagger" + newswaggerdoc.md5 + ".json", JsonConvert.SerializeObject(newswaggerdoc));

                //Append new changelogs
                 string changelog =newswaggerdoc.GenDateTime.ToString()+"\r\n"+ newswaggerdoc.ChangeLogs + "\r\n";
                File.AppendAllText(basepath + "/"+filefolder + "/changelogs.json", changelog);
            }
        }
    }
}
