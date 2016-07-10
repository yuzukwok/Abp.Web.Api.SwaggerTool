using Abp.Web.Api.SwaggerTool.Config;
using Newtonsoft.Json;
using NSwag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.Web.Api.SwaggerTool.Postman
{
  public  class PostManGen
    {
        public string Gen(SwaggerService service,string root,SwaggerToolSettings setting)
        {
            var collectionId = PostMan.GetId();
            var apis = service.Operations;
            var requests = GetPostmanRequests(apis, collectionId, root,setting);
            var collection = new PostmanCollection
            {
                id = collectionId,
                name = setting.PostmanGen.name,
                description = "",
                order = requests.Select(x => x.id).ToList(),
                timestamp = DateTime.Now.DateTimeToStamp(),
                requests = requests
            };

            //按照tag分组
            //TODO folder
            List<Postfolder> folders = new List<Postfolder>();
                var groups = requests.GroupBy(s => s.tagname);
                foreach (var item in groups)
                {
                    Postfolder floder = new Postfolder();
                    floder.id = PostMan.GetId();
                if (service.Tags != null)
                {
                    floder.name = service.Tags.Where(p => p.Name == item.Key).FirstOrDefault().Description;
                }
                else
                {
                    floder.name = item.Key;
                }
                floder.order = requests.Where(p => p.tagname == item.Key).Select(s => s.id).ToList();

                foreach (var req in requests.Where(p => p.tagname == item.Key).ToList())
                {
                    req.folder = floder.id;
                }
                folders.Add(floder);
            }
            collection.folders = folders;
            return JsonConvert.SerializeObject(collection);
        }


        private List<PostmanRequest> GetPostmanRequests(IEnumerable<SwaggerOperationDescription> apis, string collectionId, string root,SwaggerToolSettings setting)
        {
         
            List<PostmanRequest> re = new List<PostmanRequest>();
            foreach (var item in apis)
            {
                var p = new PostmanRequest
                {
                    
                    id = PostMan.GetId(),
                    name = item.Operation.Summary,
                    description = item.Operation.Summary,
                    descriptionFormat = "html",
                    headers = setting.PostmanGen.headers,
                    method = item.Method.ToString(),
                    pathVariables = new Dictionary<string, string>(),
                    url =root+ item.Path,
                    collectionId = collectionId,
                    tagname = item.Operation.Tags.FirstOrDefault()
                };
                var datalist = item.Operation.Parameters
                    .Where(s => s.Kind == SwaggerParameterKind.Query)
                    .Select(s => new PostmanData() { key = s.Name })
                    .ToList();

                if (datalist.Count>0)
                {
                    p.data = datalist;

                }
                var rawdata = item.Operation.Parameters
                     .Where(s => s.Kind == SwaggerParameterKind.Body)
                     .SingleOrDefault();
                if (rawdata!=null)
                {
                   
                    p.dataMode = "raw";
                    p.rawModeData =new Schema4Json().ToSampleJson(rawdata.ActualSchema);
                }
                
               
                re.Add(p);
            }
            return re;
        }

     
    }
}
