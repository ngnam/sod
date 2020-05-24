using StoreOrder.WebApplication.Authorization.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace StoreOrder.WebApplication.Authorization
{
    public interface IUserManager
    {
        UserClaim GetUserAsync(ClaimsPrincipal user);
        IList<string> GetPermissionAsync(ClaimsPrincipal user);
    }
}
