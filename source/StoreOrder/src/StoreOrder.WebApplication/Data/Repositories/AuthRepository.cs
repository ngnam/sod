using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using StoreOrder.WebApplication.Authorization;
using StoreOrder.WebApplication.Data.DTO;
using StoreOrder.WebApplication.Data.Enums;
using StoreOrder.WebApplication.Data.Models.Account;
using StoreOrder.WebApplication.Data.Repositories.Interfaces;
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

namespace StoreOrder.WebApplication.Data.Repositories
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

        public async Task<User> GetUserByUserNameOrEmailAsync(string userNameOrEmail)
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

        public async Task<bool> LogoutAsync(string currentUserId, string userIdentifierId)
        {
            return await LogoutUserAsync(currentUserId, userIdentifierId);
        }

        public async Task<bool> CheckUserLogoutedAsync(string currentUserId, string userIdentifierId)
        {
            return await _context.UserLogins.AnyAsync(u => u.UserId == currentUserId && u.NameIdentifier == userIdentifierId && u.IsLoggedIn == false && u.ExpiresIn == DateTime.MinValue);
        }

        public async Task<UserLogined> SignInAndSignUpCustomerAsync(CustomerLoginDTO model)
        {
            // GET roleCustomerUser 
            var roleCustomerUser = await _context.Roles.FirstOrDefaultAsync(role => role.RoleName.Equals(RoleTypeHelper.RoleCustomerUser));
            User userCreate = new User();

            // CheckUserExist
            var userExist = await _context.Users
                .Include(u => u.UserToRoles)
                .ThenInclude(x => x.Role)
                .Where(u => u.UserToRoles.Any(x => x.RoleId == roleCustomerUser.Id))
                .FirstOrDefaultAsync(u => u.Email.ToLower().Equals(model.Email.ToLower()) &&
                    u.UseExternalSignIns.Count > 0 && u.UserDevices.Count > 0);

            // case login lần sau:
            if (userExist != null)
            {
                // if exist then update user
                // update To UserDevices
                // update To UserExternalSignIns
                userCreate = userExist;
                _context.Entry(userCreate).State = EntityState.Modified;
                userCreate.LastLogin = DateTime.UtcNow;
                // check appId & currentUserId Exist
                if (!_context.UserDevices.Any(uc => uc.CurrentUserId == userCreate.Id && uc.CodeDevice == model.AppId))
                {
                    // Add to UserDevices
                    var userDevice = new UserDevice
                    {
                        Id = Guid.NewGuid().ToString(),
                        IsVerified = (int)TypeVerified.Verified,
                        VerifiedCode = (int)(DateTime.Now.Ticks >> 23),
                        CodeDevice = model.AppId,
                        CurrentUserId = userCreate.Id,
                        LastLogin = DateTime.UtcNow,
                        TimeCode = 20
                    };
                    userCreate.UserDevices.Add(userDevice);
                }
                else
                {
                    // Update to UserDevices
                    var userDevice = _context.UserDevices.FirstOrDefault(uc => uc.CurrentUserId == userExist.Id && uc.CodeDevice == model.AppId);
                    if (userDevice != null)
                    {
                        _context.Entry(userDevice).State = EntityState.Modified;
                        userDevice.LastLogin = DateTime.UtcNow;
                        // save tp db
                        await _context.SaveChangesAsync();
                    }
                }
                // check exist UseExternalSignIns
                if (!_context.ExternalSignIns.Any(ue => ue.UserId == userCreate.Id && ue.TypeLogin == model.TypeLogin))
                {
                    var newUSERExternalSignIn = new ExternalSignIn
                    {
                        Id = Guid.NewGuid().ToString(),
                        IsVerified = (int)TypeVerified.Verified,
                        LastLogin = DateTime.UtcNow,
                        TimeLifeToken = 3600,
                        TokenLogin = model.TokenLogin,
                        TypeLogin = model.TypeLogin,
                        UserId = userExist.Id
                    };
                    userCreate.UseExternalSignIns.Add(newUSERExternalSignIn);
                }
                else
                {
                    // update To UserExternalSignIns
                    var userExternalSignIn = _context.ExternalSignIns.FirstOrDefault(ue => ue.UserId == userExist.Id && ue.TypeLogin == model.TypeLogin);
                    if (userExternalSignIn != null)
                    {
                        _context.Entry(userExternalSignIn).State = EntityState.Modified;
                        userExternalSignIn.LastLogin = DateTime.UtcNow;
                        // save tp db
                        await _context.SaveChangesAsync();
                    }
                }

                // save to db
                await _context.SaveChangesAsync();
            }
            else
            {
                // if not exist then create user
                userCreate.Id = Guid.NewGuid().ToString();
                userCreate.FirstName = model.FirstName;
                userCreate.LastLogin = DateTime.UtcNow;
                userCreate.LastName = model.LastName;
                userCreate.Email = model.Email;
                userCreate.UserName = model.Email;
                userCreate.PhoneNumber = model.PhoneNumber;

                var userDevice = new UserDevice
                {
                    Id = Guid.NewGuid().ToString(),
                    CodeDevice = model.AppId,
                    CurrentUserId = userCreate.Id,
                    IsVerified = (int)TypeVerified.Verified,
                    LastLogin = DateTime.UtcNow,
                    TimeCode = 20,
                    VerifiedCode = (int)(DateTime.Now.Ticks >> 23)
                };
                // Save to UserDevices
                userCreate.UserDevices.Add(userDevice);

                var externalSign = new ExternalSignIn
                {
                    Id = Guid.NewGuid().ToString(),
                    IsVerified = (int)TypeVerified.Verified,
                    LastLogin = DateTime.UtcNow,
                    TimeLifeToken = 3600,
                    TokenLogin = model.TokenLogin,
                    TypeLogin = model.TypeLogin,
                    UserId = userCreate.Id
                };
                // Save to ExternalSignIns
                userCreate.UseExternalSignIns.Add(externalSign);

                // Save to UserToRole
                var userToRole = new UserToRole();
                userToRole.Role = roleCustomerUser;
                userToRole.User = userCreate;
                roleCustomerUser.UserToRoles.Add(userToRole);
                _context.Users.Add(userCreate);

                // Save All To Database
                await _context.SaveChangesAsync();
            }
            // create token
            string currentUserId = Guid.NewGuid().ToString();
            var userLogined = CreateToken(userCreate, currentUserId);
            // save to login
            await SaveToUserLoginAsync(userCreate, userLogined, currentUserId);
            // save to login
            // return
            return userLogined;
        }

        public async Task<User> GetUserAsync(ClaimsPrincipal user)
        {
            string curentUserId = user.Claims.FirstOrDefault(x => x.Type.Equals(UserSignedClaimTypes.CurrentUserId, StringComparison.OrdinalIgnoreCase))?.Value;
            if (string.IsNullOrEmpty(curentUserId))
            {
                return null;
            }
            var result = await _context.Users.FindAsync(curentUserId);
            if (result == null)
            {
                return null;
            }
            return result;
        }
        public async Task<Role[]> GetRolesAsync(User user)
        {
            if (user == null) { return null; }
            var result = await _context.UserToRoles.Where(x => x.UserId == user.Id).Select(x => x.Role).ToArrayAsync();
            return result;

        }
        public async Task<List<Permission>> GetPermissionAsync(Role role)
        {
            return await _context.RoleToPermissions.Where(x => x.RoleId == role.Id).Select(x => x.Permission).ToListAsync();
        }

        public string GetStoreOfCurrentUser(ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(x => x.Type.Equals(UserSignedClaimTypes.StoreIdentifierId, StringComparison.OrdinalIgnoreCase))?.Value;
        }

        #region PRIVATE METHOD
        private UserLogined CreateToken(User user, string userIdentifierId)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(UserSignedClaimTypes.UserIdentifierId, userIdentifierId),
                new Claim(UserSignedClaimTypes.CurrentUserId, user.Id.ToString()),
                new Claim(UserSignedClaimTypes.UserName, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            if (!string.IsNullOrEmpty(user.StoreId))
            {
                claims.Add(new Claim(UserSignedClaimTypes.StoreIdentifierId, user.StoreId));
            }

            var userRole = user.UserToRoles.Select(x => x.Role).ToList();
            
            foreach (var role in userRole)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.RoleName));
                var lstPermission = GetPermissionAsync(role).Result;
                foreach (var item in lstPermission)
                {
                    claims.Add(new Claim(PermissionClaimTypes.Permission, item.PermissionName));
                }
            }

            var subjects = new ClaimsIdentity(claims);

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var timeExpires = Convert.ToDouble(_configuration.GetSection("AppSettings:Expires").Value);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = subjects,
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

            _logger.LogInformation("generate token");
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
                    ExpiresIn = userlogined.ExpiresIn.Value.DateTime,
                    IsLoggedIn = userlogined.IsLoggedIn,
                    TokenType = userlogined.TokenType,
                    UserId = user.Id,
                    NameIdentifier = curentUserId
                }
            };
            try
            {
                _logger.LogInformation("Save user logined");
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ResponseMessageEnum.UpdateDb.GetDescription(), ex);
                throw new ApiException(ResponseMessageEnum.UpdateDb.GetDescription());
            }
        }

        private async Task<bool> LogoutUserAsync(string currentUserId, string userIdentifierId)
        {
            if ((await CheckUserLogoutedAsync(currentUserId, userIdentifierId)))
            {
                _logger.LogInformation("User đã logout");
                throw new ApiException("User đã logout", (int)HttpStatusCode.Unauthorized);
            }
            var userLogin = await _context.UserLogins
                .FirstOrDefaultAsync(p => p.UserId == currentUserId && p.NameIdentifier == userIdentifierId && p.IsLoggedIn == true);
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
                _logger.LogInformation("User đã logout success");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new ApiException(ex.Message);
            }
            return true;
        }
        #endregion
    }
}
