using NSwag;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abp.Web.Api.SwaggerTool
{
    public class SwaggerDiff
    {
        
        public async Task<SwaggerDiffResult> Diff(string newdoc, string olddoc)
        {
            //兼容性
            //            all path and verb combinations in the old specification are present in the new one
            //no request parameters are required in the new specification that were not required in the old one
            //all request parameters in the old specification are present in the new one
            //all request parameters in the old specification have the same type in the new one
            //all response attributes in the old specification are present in the new one
            //all response attributes in the old specification have the same type in the new one
            SwaggerDiffResult re = new SwaggerDiffResult();
            bool compatiable = true;

            var snew = await SwaggerDocument.FromJsonAsync(newdoc);
            var sold = await SwaggerDocument.FromJsonAsync(olddoc);

            //StringBuilder builder = new StringBuilder();

            //foreach (var item in result.Annotations)
            //{
            //    if (item is MissingPropertyAnnotation)
            //    {
            //        if (item.Path.AsJsonPath == "paths")
            //        {
            //            //缺少方法
            //            compatiable = false;
            //            re.MissMethods.Add("del path：" + ((MissingPropertyAnnotation)item).PropertyName);
            //            continue;
            //        }
            //        else if (regexpathmethod.Match(item.Path.AsJsonPath).Success)
            //        {
            //            re.MissMethods.Add("del path:"+ regexpathmethod.Match(item.Path.AsJsonPath).Value.Replace("paths.","") + " http method:" + ((MissingPropertyAnnotation)item).PropertyName);
            //            continue;
            //        }
            //    }

            //   // builder.Append(item.Path + " " + item.Message);
            //}


            return re;

        }

      


    }


    public class SwaggerDiffResult
    {
        public SwaggerDiffResult()
        {
            AddMethods = new List<string>();
            MissMethods = new List<string>();
            FieldChanges = new List<string>();
        }
        public List<string> MissMethods { get; set; }
        public List<string> AddMethods { get; set; }
        public List<string> FieldChanges { get; set;  }
    }
}
