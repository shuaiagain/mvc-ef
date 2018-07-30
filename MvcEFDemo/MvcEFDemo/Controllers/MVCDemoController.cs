using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcEFDemo.Controllers
{
    public class MVCDemoController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SharedDateDemo()
        {
            ViewBag.DateTime = DateTime.Now;
            return View();
        }

        [ChildActionOnly]
        public ActionResult PartialViewDate()
        {
            ViewBag.DateTime = DateTime.Now.AddMinutes(10);
            return View("PartialPageDateTime");
        }
    }
}
