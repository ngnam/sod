using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using StoreOrder.WebApplication.Authorization;

namespace StoreOrder.WebApplication.Helpers
{
    public static class StartupHelpers
    {
        public static void AddAuthorizationPolicies(this IServiceCollection services)
        {
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy(Permissions.Account.View, builder =>
            //    {
            //        builder.AddRequirements(new PermissionRequirement(Permissions.Account.View));
            //    });
            //    options.AddPolicy(Permissions.Account.Create, builder =>
            //    {
            //        builder.AddRequirements(new PermissionRequirement(Permissions.Account.Create));
            //    });
            //});
            services.AddAuthorization();
            // register the scope authorization handler
            services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
            services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        }
    }
}
