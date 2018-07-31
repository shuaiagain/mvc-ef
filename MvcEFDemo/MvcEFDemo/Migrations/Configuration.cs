namespace MvcEFDemo.Migrations
{
    using System;
    using System.Collections.Generic;

    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using MvcEFDemo.DAL;
    using MvcEFDemo.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<MvcEFDemo.DAL.AccountContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="context"></param>
        protected override void Seed(AccountContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            var sysUsers = new List<SysUser>
            {
                new SysUser{UserName="Tom",Password="1",Email="Tom@gmail.com"},
                new SysUser{UserName="Jerry",Password="2",Email="Jerry@gmail.com"}
            };

            sysUsers.ForEach(s => context.SysUsers.AddOrUpdate(p => p.UserName, s));
            context.SaveChanges();

            var sysRoles = new List<SysRole>
            {
                new SysRole{RoleName="Admin",RoleDesc="1"},
                new SysRole{RoleName="Common",RoleDesc="2"}
            };

            sysRoles.ForEach(s => context.SysRoles.AddOrUpdate(p => p.RoleName, s));
            context.SaveChanges();

            var sysUserRoles = new List<SysUserRole>()
            {
                new SysUserRole{
                    SysUserID=sysUsers.Single(s=>s.UserName=="Tom").ID,
                    SysRoleID=sysRoles.Single(s=>s.RoleName=="Admin").ID
                },
                 new SysUserRole{
                    SysUserID=sysUsers.Single(s=>s.UserName=="Tom").ID,
                    SysRoleID=sysRoles.Single(s=>s.RoleName=="Common").ID
                },
                 new SysUserRole{
                    SysUserID=sysUsers.Single(s=>s.UserName=="Jerry").ID,
                    SysRoleID=sysRoles.Single(s=>s.RoleName=="Common").ID
                }
            };

            foreach (var item in sysUserRoles)
            {
                var sysUserRoleInDataBase = context.SysUserRoles.Where(s => s.SysUser.ID == item.SysUserID && s.SysRole.ID == item.SysRoleID).SingleOrDefault();

                if (sysUserRoleInDataBase == null) context.SysUserRoles.Add(item);
            }

            context.SaveChanges();
        }

    }
}
