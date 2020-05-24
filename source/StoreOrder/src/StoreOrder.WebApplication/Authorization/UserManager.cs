using StoreOrder.WebApplication.Authorization.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace StoreOrder.WebApplication.Authorization
{
    public class UserManager : IUserManager
    {
        public IList<string> GetPermissionAsync(ClaimsPrincipal user)
        {
            return user.Claims.Where(x => x.Type.Equals(PermissionClaimTypes.Permission, StringComparison.OrdinalIgnoreCase)).Select(x => x.Value).ToList();
        }

        public UserClaim GetUserAsync(ClaimsPrincipal user)
        {
            return new UserClaim
            {
                CurrentUserId = user.Claims.FirstOrDefault(x => x.Type.Equals(UserSignedClaimTypes.CurrentUserId, StringComparison.OrdinalIgnoreCase))?.Value,
                StoreId = user.Claims.FirstOrDefault(x => x.Type.Equals(UserSignedClaimTypes.StoreIdentifierId, StringComparison.OrdinalIgnoreCase))?.Value,
                UserIdentifierId = user.Claims.FirstOrDefault(x => x.Type.Equals(UserSignedClaimTypes.UserIdentifierId, StringComparison.OrdinalIgnoreCase))?.Value,
                UserName = user.Claims.FirstOrDefault(x => x.Type.Equals(UserSignedClaimTypes.UserName, StringComparison.OrdinalIgnoreCase))?.Value,
                Roles = user.Claims.Where(x => x.Type.Equals(ClaimTypes.Role, StringComparison.OrdinalIgnoreCase)).Select(x=>x.Value).ToList(),
            };
        }
    }
}
