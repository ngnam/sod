using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Data.Models.Account
{
    public class Role
    {
        public Role()
        {
            this.UserToRoles = new HashSet<UserToRole>();
            this.RoleToPermissions = new HashSet<RoleToPermission>();
        }
        public string Id { get; set; }
        public string RoleName { get; set; } // Unique
        public string Desc { get; set; }
        public string Code { get; set; }
        public virtual ICollection<UserToRole> UserToRoles { get; set; }
        public virtual ICollection<RoleToPermission> RoleToPermissions { get; set; }
    }
}
