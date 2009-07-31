using System.Web.Mvc;

namespace Quickstart.Controllers
{
    using System;

    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewData["Message"] = "Welcome to ASP.NET MVC!";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult ErrorResultFactory()
        {
            return View();
        }
    }
}
