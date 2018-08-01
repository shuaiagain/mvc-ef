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
using System.Net;

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

        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SysUser user = new AccountContext().SysUsers.Include(u => u.SysDepartment)
                                                      .Include(s => s.SysUserRoles)
                                                      .Where(a => a.ID == id).Single();

            if (user == null)
            {
                return HttpNotFound();
            }

            //将用户所在的部门选出
            PopulateDepartmentsDropDownList(user.SysDepartmentID);
            //将某个用户下的所有角色取出
            PopulateAssignedRoleData(user);

            return View(user);
        }

        [HttpPost]
        public ActionResult Edit(int? id, string[] selectedRoles)
        {
            if (!id.HasValue)
            {
                return HttpNotFound();
            }

            var accountDB = new AccountContext();
            var userToUpdate = accountDB.SysUsers.Include(a => a.SysUserRoles).Where(a => a.ID == id).Single();
            if (TryUpdateModel(userToUpdate, "", new string[] { "UserName", "Password", "Email", "CreateDate", "SysDepartmentID" }))
            {
                try
                {
                    UpdateUserRoles(selectedRoles, userToUpdate);

                    accountDB.Entry(userToUpdate).State = EntityState.Modified;
                    accountDB.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    throw;
                }
            }

            //如果失败,重新绑定视图
            PopulateDepartmentsDropDownList(userToUpdate.SysDepartmentID);//将用户所在的部门选出
            PopulateAssignedRoleData(userToUpdate);//将某个用户下的所有角色取出
            return View(userToUpdate);
        }

        private void UpdateUserRoles(string[] selectedRoles, SysUser userToUpdate)
        {
            using (AccountContext db2 = new AccountContext())
            {
                //没有选择，全部清空
                if (selectedRoles == null)
                {
                    var sysUserRoles = db2.SysUserRoles.Where(u => u.SysUserID == userToUpdate.ID).ToList();
                    foreach (var item in sysUserRoles)
                    {
                        db2.SysUserRoles.Remove(item);
                    }
                    db2.SaveChanges();
                    return;
                }
                //编辑后的角色
                var selectedRolesHS = new HashSet<string>(selectedRoles);
                //原来的角色
                var userRoles = new HashSet<int>
                (userToUpdate.SysUserRoles.Select(r => r.SysRoleID));

                foreach (var item in db2.SysRoles)
                {
                    //如果被选中,原来没有的要添加
                    if (selectedRolesHS.Contains(item.ID.ToString()))
                    {
                        if (!userRoles.Contains(item.ID))
                        {
                            userToUpdate.SysUserRoles.Add(new SysUserRole { SysUserID = userToUpdate.ID, SysRoleID = item.ID });
                        }
                    }
                    else
                    {
                        if (userRoles.Contains(item.ID))//如果没被选中,原来有的要去除
                        {
                            SysUserRole sysUserRole = db2.SysUserRoles
                                .FirstOrDefault(ur => ur.SysRoleID == item.ID && ur.SysUserID == userToUpdate.ID);
                            db2.SysUserRoles.Remove(sysUserRole);
                            db2.SaveChanges();
                        }
                    }
                }
            }
        }
        public void PopulateAssignedRoleData(SysUser user)
        {

            var allRoles = new AccountContext().SysRoles.ToList();
            var userRoles = new HashSet<int>(user.SysUserRoles.Select(a => a.SysRoleID));
            var vm = new List<AssignedRoleData>();
            foreach (var item in allRoles)
            {
                vm.Add(new AssignedRoleData()
                {
                    RoleId = item.ID,
                    RoleName = item.RoleName,
                    Assigned = userRoles.Contains(item.ID)
                });
            }

            ViewBag.Roles = vm;
        }

        public void PopulateDepartmentsDropDownList(object selectedId = null)
        {
            var departQuery = (from q in new AccountContext().SysDepartments
                               orderby q.DepartmentName
                               select q);

            ViewBag.SysDepartmentID = new SelectList(departQuery, "ID", "DepartmentName", selectedId);
        }
    }
}
