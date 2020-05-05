using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StoreOrder.WebApplication.Data.Models.Account;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Data
{
    public class AuthRepository: IAuthRepository
    {
        private readonly IConfiguration _configuration;
        private readonly StoreOrderDbContext _context;
        private Boolean Disposed;

        public AuthRepository(
            StoreOrderDbContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
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

        public string GenerateToken(User user)
        {
            return CreateToken(user);
        }

        public async Task<User> GetUserByUserNameOrEmail(string userNameOrEmail)
        {
           return await _context.Users.FirstOrDefaultAsync(user => 
            user.UserName.ToLower().Equals(userNameOrEmail.ToLower()) || 
            user.Email.ToLower().Equals(userNameOrEmail.ToLower()));
        }

        public bool VerifyPasswordHash(string enteredPassword, string storedHash, string storedSalt)
        {
            return Helpers.SercurityHelper.VerifyPasswordHash(enteredPassword, storedHash, storedSalt);
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

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

            return tokenHandler.WriteToken(token);
        }
    }
}
