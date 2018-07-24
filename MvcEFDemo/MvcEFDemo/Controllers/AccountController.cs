using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcEFDemo.Controllers
{
    public class AccountController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            ViewBag.LoginState = "登录前...";
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection fc)
        {

            string email = fc["exampleInputEmail"];
            string pwd = fc["exampleInputPassword"];

            ViewBag.LoginState = "登录后...";
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }
    }
}
