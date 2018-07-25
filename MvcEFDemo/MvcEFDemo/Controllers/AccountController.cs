using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MvcEFDemo.DAL;
using MvcEFDemo.Models;
namespace MvcEFDemo.Controllers
{
    public class AccountController : Controller
    {

        public ActionResult Index()
        {
            AccountContext accountDb = new AccountContext();
            return View(accountDb.SysUsers);
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

            AccountContext db = new AccountContext();
            SysUser user = db.SysUsers.Where(u => u.Email == email && u.Password == pwd).FirstOrDefault();
            if (user == null)
            {
                ViewBag.LoginState = email + "用户不存在";
            }
            else
            {
                ViewBag.LoginState = email + " 登录后...";
            }

            return View();
        }

        public ActionResult Register()
        {
            return View();
        }
    }
}
