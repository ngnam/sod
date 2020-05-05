using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Data.Models.Account
{
    public class Permission
    {
        public Permission()
        {
        }
        public string Id { get; set; }
        public string PermissionName { get; set; } // unique // the tabs, screens, actions
        public virtual ICollection<RoleToPermission> RoleToPermissions { get; set; }
    }
}
