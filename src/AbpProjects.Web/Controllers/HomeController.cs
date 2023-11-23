using System;
using System.Web.Mvc;
using Abp.Timing;
using Abp.Web.Mvc.Authorization;


namespace AbpProjects.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HomeController : AbpProjectsControllerBase
    {
        public ActionResult Index()
        {
            var data = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            //var currenttime = Clock.Now;
            return View("~/App/Main/views/layout/layout.cshtml"); //Layout of the angular application.
        }
    }
}