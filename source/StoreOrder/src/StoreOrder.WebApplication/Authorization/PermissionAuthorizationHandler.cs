using Microsoft.AspNetCore.Authorization;
using StoreOrder.WebApplication.Data;
using System.Linq;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Authorization
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IUserManager _userManager;
        public PermissionAuthorizationHandler(IUserManager userManager)
        {
            _userManager = userManager;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            await Task.Run(() =>
            {
                if (context.User == null)
                {
                    return;
                }

                // Get all the roles the user belongs to and check if any of the roles has the permission required
                // for the authorization to succeed.

                var user = _userManager.GetUserAsync(context.User);
                if (user == null)
                {
                    return;
                }

                // check user.StoreId !== currentUser.StoreId or User login is Customer
                if (!(user.Roles.Contains(RoleTypeHelper.RoleSysAdmin)) && string.IsNullOrEmpty(user.StoreId))
                {
                    context.Fail();
                    return;
                }

                var permissions = _userManager.GetPermissionAsync(context.User);

                if (permissions.Any(p => p == requirement.Permission))
                {
                    context.Succeed(requirement);
                    return;
                }
                else
                {
                    context.Fail();
                    return;
                }
            });
        }
    }
}
