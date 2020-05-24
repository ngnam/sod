using System.Collections.Generic;

namespace StoreOrder.WebApplication.Authorization.Models
{
    public class UserClaim
    {
        public UserClaim()
        {
            Roles = new HashSet<string>();
        }
        public string CurrentUserId { get; set; }
        public string UserIdentifierId { get; set; }
        public string UserName { get; set; }
        public string StoreId { get; set; }
        public ICollection<string> Roles { get; set; }
    }
}
