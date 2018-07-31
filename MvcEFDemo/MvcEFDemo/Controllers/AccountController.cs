using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MvcEFDemo.DAL;
using MvcEFDemo.Models;
using System.Data.Entity;
using PagedList.Mvc;
using PagedList;
namespace MvcEFDemo.Controllers
{
    public class AccountController : Controller
    {

        #region Index
        public ActionResult Index(string sortOrder, string searchStr, int? page, string currentFilter)
        {
            AccountContext accountDb = new AccountContext();

            ViewBag.SortKey = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.CurrentSort = sortOrder;
            ViewBag.CurrentFilter = currentFilter;
            var users = (from u in accountDb.SysUsers
                         select u);

            if (!string.IsNullOrEmpty(searchStr))
            {
                page = 1;
                users = users.Where(u => u.UserName.Contains(searchStr));
            }
            else
            {
                searchStr = currentFilter;
            }


            switch (sortOrder)
            {
                case "name_desc":
                    users = users.OrderByDescending(u => u.UserName);
                    break;
                default:
                    users = users.OrderBy(u => u.UserName);
                    break;

            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            //return View(users.ToList());
            return View(users.ToPagedList(pageNumber, pageSize));
        }
        #endregion

        #region Login
        public ActionResult Login()
        {
            ViewBag.LoginState = "登录前...";
            return View();
        }
        #endregion

        #region Login
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
        #endregion

        #region Register
        public ActionResult Register()
        {
            return View();
        }
        #endregion

        #region Details
        public ActionResult Details(int id)
        {
            SysUser user = new AccountContext().SysUsers.Find(id);
            return View(user);
        }
        #endregion

        #region Create
        public ActionResult Create()
        {
            return View();
        }
        #endregion

        #region Create
        [HttpPost]
        public ActionResult Create(SysUser sysUser)
        {
            var db = new AccountContext();
            db.SysUsers.Add(sysUser);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        #endregion

        #region Edit
        //修改用户
        public ActionResult Edit(int id)
        {
            SysUser sysUser = new AccountContext().SysUsers.Find(id);
            return View(sysUser);
        }
        #endregion

        #region Edit
        [HttpPost]
        public ActionResult Edit(SysUser sysUser)
        {
            var db = new AccountContext();
            db.Entry(sysUser).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        #endregion

        #region Delete
        //删除用户
        public ActionResult Delete(int id)
        {
            SysUser sysUser = new AccountContext().SysUsers.Find(id);
            return View(sysUser);
        }
        #endregion

        #region DeleteConfirmed
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var db = new AccountContext();
            SysUser sysUser = db.SysUsers.Find(id);
            db.SysUsers.Remove(sysUser);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        #endregion

    }
}
