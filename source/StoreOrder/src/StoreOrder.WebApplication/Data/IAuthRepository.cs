using StoreOrder.WebApplication.Data.DTO;
using StoreOrder.WebApplication.Data.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Data
{
    public interface IAuthRepository
    {
        UserLogined GenerateToken(User user, string curentUserId);
        Task<User> GetUserByUserNameOrEmail(string userNameOrEmail);
        bool VerifyPasswordHash(string enteredPassword, string storedHash, string storedSalt);
        Task SaveToUserLoginAsync(User user, UserLogined userlogined, string currentUserId);
        Task<bool> LogoutAsync(string userId, string CurrentUserId);
        Task<bool> CheckUserLogoutedAsync(string userId, string currentUserId);
    }
}
