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
        public string Gen(SwaggerService service,SwaggerToolSettings setting)
        {
            var collectionId = PostMan.GetId();
            var apis = service.Operations;
            var requests = GetPostmanRequests(apis, collectionId);
            var collection = new PostmanCollection
            {
                id = collectionId,
                name = "WebAPI2PostMan",
                description = "",
                order = requests.Select(x => x.id).ToList(),
                timestamp = 0,
                requests = requests
            };
            return JsonConvert.SerializeObject(collection);
        }


        private List<PostmanRequest> GetPostmanRequests(IEnumerable<SwaggerOperationDescription> apis, string collectionId)
        {
            return apis.Select(api => new PostmanRequest
            {
                collection = collectionId,
                id = PostMan.GetId(),
                name = api.Operation.Summary,               
               //data = GetPostmanDatas(api),
                description = "",
                descriptionFormat = "html",
                headers = "",
                method = api.Method.ToString(),
                pathVariables = new Dictionary<string, string>(),
                url = api.Path,
                version = 2,
                collectionId = collectionId
            }).ToList();
        }

     
    }
}
