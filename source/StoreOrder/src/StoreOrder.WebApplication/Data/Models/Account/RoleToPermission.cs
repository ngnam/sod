using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Data.Models.Account
{
    public class RoleToPermission
    {
        public string RoleId { get; set; }
        public Role Role { get; set; }
        public string PermissionId { get; set; }
        public Permission Permission { get; set; }
    }
}
