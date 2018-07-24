using MvcEFDemo.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MvcEFDemo.DAL
{
    public class AccountInitializer : DropCreateDatabaseIfModelChanges<AccountContext>
    {

        protected override void Seed(AccountContext context)
        {
            base.Seed(context);

            var sysUsers = new List<SysUser>
            {
                new SysUser{UserName="Tom",Password="1",Email="Tom@gmail.com"},
                new SysUser{UserName="Jerry",Password="2",Email="Jerry@gmail.com"}
            };

            sysUsers.ForEach(s => context.SysUsers.Add(s));
            context.SaveChanges();

            var sysRoles = new List<SysRole>
            {
                new SysRole{RoleName="Admin",RoleDesc="1"},
                new SysRole{RoleName="Common",RoleDesc="2"}
            };

            sysRoles.ForEach(s=>context.SysRoles.Add(s));
            context.SaveChanges();
        }

    }
}