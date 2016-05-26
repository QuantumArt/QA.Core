using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QA.Core.Web;

namespace TestCache.Controllers
{
    public class HomeController : Controller
    {
        [ResultCache(Duration = 8, VaryByParam = "p1;p2;p3")]
        public ActionResult Index(int p1, string p2, string[] p3)
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Действие контроллера c параметрами " + RouteData.Values.Aggregate("", (k, v) => string.Format("{0}-{1}", k, v)) +
                "выполнено: " + DateTime.Now;

            return PartialView();
        }


        public ActionResult News()
        {
            ViewBag.Message = "Действие контроллера c параметрами " + RouteData.Values.Aggregate("", (k, v) => string.Format("{0}-{1}", k, v)) +
                "выполнено: " + DateTime.Now;

            return PartialView();
        }
    }
}
