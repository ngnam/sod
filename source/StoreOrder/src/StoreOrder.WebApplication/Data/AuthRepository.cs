using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using StoreOrder.WebApplication.Data.DTO;
using StoreOrder.WebApplication.Data.Models.Account;
using StoreOrder.WebApplication.Data.Wrappers;
using StoreOrder.WebApplication.Extensions;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IConfiguration _configuration;
        private readonly StoreOrderDbContext _context;
        private Boolean Disposed;
        private readonly ILogger<AuthRepository> _logger;

        public AuthRepository(
            StoreOrderDbContext context, IConfiguration configuration, ILogger<AuthRepository> logger)
        {
            _configuration = configuration;
            _context = context;
            _logger = logger;
        }


        public virtual void Dispose()
        {
            if (!Disposed)
            {
                if (_context != null)
                {
                    _context.Dispose();
                    Disposed = true;
                }
            }
        }

        public UserLogined GenerateToken(User user, string currentUserId)
        {
            return CreateToken(user, currentUserId);
        }

        public async Task<User> GetUserByUserNameOrEmail(string userNameOrEmail)
        {
            return await _context.Users.Include(x => x.UserToRoles).ThenInclude(x => x.Role).FirstOrDefaultAsync(user =>
             user.UserName.ToLower().Equals(userNameOrEmail.ToLower()) ||
             user.Email.ToLower().Equals(userNameOrEmail.ToLower()));
        }

        public bool VerifyPasswordHash(string enteredPassword, string storedHash, string storedSalt)
        {
            return Helpers.SercurityHelper.VerifyPasswordHash(enteredPassword, storedHash, storedSalt);
        }

        public async Task SaveToUserLoginAsync(User user, UserLogined userlogined, string currentUserId)
        {
            await SaveUserLoginedAsync(user, userlogined, currentUserId);
        }

        public async Task<bool> LogoutAsync(string userId, string currentUserId)
        {
            return await LogoutUserAsync(userId, currentUserId);
        }

        public async Task<bool> CheckUserLogoutedAsync(string userId, string currentUserId)
        {
            return await _context.UserLogins.AnyAsync(u => u.UserId == userId && u.NameIdentifier == currentUserId && u.IsLoggedIn == false && u.ExpiresIn == DateTime.MinValue);
        }

        private UserLogined CreateToken(User user, string curentUserId)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, curentUserId),
                new Claim(ClaimTypes.PrimarySid, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
            };

            var userRole = user.UserToRoles.Select(x => x.Role);
            if (userRole.Count() > 0)
            {
                var roleName = string.Join(";", userRole.Select(r => r.RoleName));
                var roleCode = string.Join(";", userRole.Select(r => r.Code));
                var roleIds = string.Join(";", userRole.Select(r => r.Id));
                claims.Add(new Claim(ClaimTypes.Role, roleName));
                claims.Add(new Claim(ClaimTypes.UserData, roleCode));
                claims.Add(new Claim(ClaimTypes.GroupSid, roleIds));
            }

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var timeExpires = Convert.ToDouble(_configuration.GetSection("AppSettings:Expires").Value);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(timeExpires),
                SigningCredentials = creds,
                Audience = _configuration.GetSection("AppSettings:Audience").Value,
                Issuer = _configuration.GetSection("AppSettings:Issuer").Value,
            };
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            var userLogined = new UserLogined
            {
                AccessToken = tokenHandler.WriteToken(token),
                ExpiresIn = token.ValidTo,
                IsLoggedIn = true,
                TokenType = JwtBearerDefaults.AuthenticationScheme
            };

            return userLogined;
        }

        private async Task SaveUserLoginedAsync(User user, UserLogined userlogined, string curentUserId)
        {
            _context.Entry(user).State = EntityState.Modified;
            user.LastLogin = DateTime.UtcNow;
            user.UserLogins = new List<UserLogin>()
            {
                new UserLogin {
                    AccessToken = userlogined.AccessToken,
                    ExpiresIn = userlogined.ExpiresIn,
                    IsLoggedIn = userlogined.IsLoggedIn,
                    TokenType = userlogined.TokenType,
                    UserId = user.Id,
                    NameIdentifier = curentUserId
                }
            };
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ResponseMessageEnum.UpdateDb.GetDescription(), ex);
                throw new ApiException(ResponseMessageEnum.UpdateDb.GetDescription());
            }
        }

        private async Task<bool> LogoutUserAsync(string userId, string currentUserId)
        {
            if ((await CheckUserLogoutedAsync(userId, currentUserId)))
            {
                _logger.LogInformation("User đã logout");
                throw new ApiException("User đã logout", (int)HttpStatusCode.Unauthorized);
            }
            var userLogin = await _context.UserLogins
                .FirstOrDefaultAsync(p => p.UserId == userId && p.NameIdentifier == currentUserId && p.IsLoggedIn == true);
            if (userLogin == null)
            {
                _logger.LogInformation("User không tồn tại!");
                throw new ApiException("User không tồn tại!", (int)HttpStatusCode.BadRequest);
            }
            // Remove currentUser
            //_context.UserLogins.Remove(userLogin);
            _context.Entry(userLogin).State = EntityState.Modified;
            userLogin.IsLoggedIn = false;
            userLogin.ExpiresIn = DateTime.MinValue;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new ApiException(ex.Message);
            }
            return true;
        }
    }
}
