using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Abp.Web.Api.SwaggerTool.SampleSite.Controllers
{
    [Authorize]
    public class ValuesController : ApiController
    {
        [DisplayName("A")]
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        //public string Api1()
        //{
        //    return "api1";
        //}
        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]ClassDto value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }

    public class ClassDto
    {
        public List<ClassDto2> Dtos { get; set; }
    }
    /// <summary>
    /// dto class 2 comment
    /// </summary>
    public class ClassDto2
    {
        /// <summary>
        /// A comment
        /// </summary>
        public string A { get; set; }
        public int B { get; set; }
        public Enum1 Enum { get; set; }
    }

    public enum Enum1
    {
        [Display(Name ="这是C")]
        C,
        D
    }
}
