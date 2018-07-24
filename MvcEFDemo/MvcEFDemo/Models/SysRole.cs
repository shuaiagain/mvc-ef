using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcEFDemo.Models
{
    public class SysRole
    {
        public int ID { get; set; }

        public string RoleName { get; set; }

        public string RoleDesc { get; set; }

        public virtual ICollection<SysUserRole> SysUserRoles { get; set; }
    }
}