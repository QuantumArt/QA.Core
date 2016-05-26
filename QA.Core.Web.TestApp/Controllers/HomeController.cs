using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QA.Core.Web.TestApp.Models;

namespace QA.Core.Web.TestApp.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(TestModel model)
        {
            ViewBag.Message = string.Format("You have submited: {0}, {1}", model.MyProperty1, model.MyProperty2);
            return View();
        }

        #region ajax widgets

        public ActionResult AjaxForm1()
        {
            return PartialView(new TestModel { MyProperty1 = "test1.1", MyProperty2 = "test1.2" });
        }

        public ActionResult AjaxForm2()
        {
            return PartialView(new TestModel { MyProperty1 = "test2.1", MyProperty2 = "test2.2" });
        }

        public ActionResult AjaxForm3()
        {
            return PartialView(new TestModel { MyProperty1 = "test3.1", MyProperty2 = "test3.2" });
        }


        [HttpPost]
        [ValidateAjaxToken()]
        public ActionResult AjaxFormPost1(TestModel model)
        {
            return Json(string.Format("You have submited: {0}, {1}", model.MyProperty1, model.MyProperty2));
        }

        [HttpPost]
        [ValidateAjaxToken(true)]
        public ActionResult AjaxFormPost2(TestModel model)
        {
            return Json(string.Format("You have submited: {0}, {1}", model.MyProperty1, model.MyProperty2));
        }

        [HttpPost]
        [ValidateAjaxToken(HeaderKey = "1234")]
        public ActionResult AjaxFormPost3(TestModel model)
        {
            return Json(string.Format("You have submited: {0}, {1}", model.MyProperty1, model.MyProperty2));
        }

        #endregion

    }
}
