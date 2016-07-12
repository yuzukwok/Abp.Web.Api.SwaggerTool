using Difftaculous;
using Difftaculous.Adapters;
using Difftaculous.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.Web.Api.SwaggerTool
{
   public class SwaggerDiff
    {
        public Tuple<string,bool> Diff(string newdoc, string olddoc)
        {
            
            IDiffResult result = DiffEngine.Compare(new JsonAdapter(newdoc),
                                                    new JsonAdapter(olddoc));

            foreach (var item in result.Annotations)
            {
                Console.WriteLine(item.Path + " " + item.Message);
            }

            return Tuple.Create<string,bool>("",true);

        }
    }
}
