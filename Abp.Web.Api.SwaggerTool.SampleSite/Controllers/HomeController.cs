using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Abp.Web.Api.SwaggerTool.SampleSite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
