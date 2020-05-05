using StoreOrder.WebApplication.Data.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Data
{
    public interface IAuthRepository
    {
        string GenerateToken(User user);
        Task<User> GetUserByUserNameOrEmail(string userNameOrEmail);
        bool VerifyPasswordHash(string enteredPassword, string storedHash, string storedSalt);
    }
}
