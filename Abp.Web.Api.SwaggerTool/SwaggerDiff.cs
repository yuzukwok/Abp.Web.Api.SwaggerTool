using Difftaculous;
using Difftaculous.Adapters;
using Difftaculous.Paths;
using Difftaculous.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Abp.Web.Api.SwaggerTool
{
   public class SwaggerDiff
    {
        Regex regexpathparameterreq = new Regex(@"paths\.\['[/\w]*'\]\.[\w]*\.parameters\[\d+\]\.required");
        public Tuple<string,bool> Diff(string newdoc, string olddoc)
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
            IDiffResult result = DiffEngine.Compare(new JsonAdapter(newdoc),
                                                    new JsonAdapter(olddoc));

            StringBuilder builder = new StringBuilder();

            foreach (var item in result.Annotations)
            {
                if (item is MissingPropertyAnnotation)
                {
                    if (item.Path.AsJsonPath == "paths")
                    {
                        //缺少方法
                        compatiable = false;
                        re.MissMethods.Add("del path：" + ((MissingPropertyAnnotation)item).PropertyName);
                        continue;
                    }
                }
                else if (item is DifferingValuesAnnotation)
                {
                    //字段变化
                    //path 中parameter required字段的变化 F->T 非兼容
                    var diffv = item as DifferingValuesAnnotation;
                    if (regexpathparameterreq.Match(diffv.Path.AsJsonPath).Success)
                    {
                        if (Convert.ToBoolean(diffv.ValueA)==true&&Convert.ToBoolean(diffv.ValueB)==false)
                        {
                            compatiable = false;
                            
                        }
                        re.FieldChanges.Add("field required change:"+diffv.Message);
                    }
                    
                }
                builder.Append(item.Path + " " + item.Message);
            }

            return Tuple.Create<string,bool>(builder.ToString(),true);

        }

      


    }


    public class SwaggerDiffResult
    {
        public SwaggerDiffResult()
        {
            MissMethods = new List<string>();
            FieldChanges = new List<string>();
        }
        public List<string> MissMethods { get; set; }
        public List<string> FieldChanges { get; set;  }
    }
}
