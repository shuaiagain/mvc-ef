using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using MvcEFDemo.Models;

namespace MvcEFDemo.DAL
{
    public class AccountContext : DbContext
    {
        public AccountContext() : base("AccountContext") { }

        public DbSet<Test> Tests { get; set; }

        public DbSet<SysUser> SysUsers { get; set; }

        public DbSet<SysRole> SysRoles { get; set; }

        public DbSet<SysUserRole> SysUserRoles { get; set; }

        public DbSet<SysDepartment> SysDepartments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}