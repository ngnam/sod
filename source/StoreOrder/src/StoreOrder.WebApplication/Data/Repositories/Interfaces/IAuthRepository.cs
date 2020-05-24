using StoreOrder.WebApplication.Data.DTO;
using StoreOrder.WebApplication.Data.Models.Account;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Data.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        UserLogined GenerateToken(User user, string curentUserId);
        Task<User> GetUserByUserNameOrEmailAsync(string userNameOrEmail);
        bool VerifyPasswordHash(string enteredPassword, string storedHash, string storedSalt);
        Task SaveToUserLoginAsync(User user, UserLogined userlogined, string userIdentifierId);
        Task<bool> LogoutAsync(string currentUserId, string userIdentifierId);
        Task<bool> CheckUserLogoutedAsync(string currentUserId, string userIdentifierId);
        Task<UserLogined> SignInAndSignUpCustomerAsync(CustomerLoginDTO model);
        Task<User> GetUserAsync(ClaimsPrincipal user);
        string GetStoreOfCurrentUser(ClaimsPrincipal user);
        Task<Role[]> GetRolesAsync(User user);
        Task<List<Permission>> GetPermissionAsync(Role role);
    }
}
