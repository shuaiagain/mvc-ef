using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Web.Mvc;
using MvcEFDemo.Models;
using MvcEFDemo.ViewModels;
using MvcEFDemo.DAL;
using System.Data.Entity;

namespace MvcEFDemo.Controllers
{
    public class UserRoleController : Controller
    {

        public ActionResult Index(int? id)
        {

            var vm = new UserRoleIndexData();
            var accountDB = new AccountContext();
            //获取所有用户及用户的角色和部门
            vm.SysUsers = accountDB.SysUsers.Include(u => u.SysDepartment)
                                          .Include(u => u.SysUserRoles.Select(s => s.SysRole))
                                          .OrderBy(u => u.UserName);

            if (id.HasValue)
            {
                ViewBag.UserID = id;
                vm.SysUserRoles = vm.SysUsers.Where(u => u.ID == id).Single().SysUserRoles;
                vm.SysRoles = vm.SysUserRoles.Where(u => u.SysUserID == id).Select(s => s.SysRole);
            }

            return View(vm);
        }

    }
}
