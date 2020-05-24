using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StoreOrder.WebApplication.Controllers.ApiBase;
using StoreOrder.WebApplication.Data;
using StoreOrder.WebApplication.Data.DTO;
using StoreOrder.WebApplication.Data.Enums;
using StoreOrder.WebApplication.Data.Repositories.Interfaces;
using StoreOrder.WebApplication.Data.Wrappers;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Controllers
{
    [Authorize]
    [ApiVersion("1")]
    [ApiVersion("2")]
    public class UserController : ApiBaseController<UserController>
    {
        private readonly StoreOrderDbContext _context;
        public UserController(
            StoreOrderDbContext context,
            ILoggerFactory loggerFactory,
            IAuthRepository authRepository) : base(loggerFactory, authRepository)
        {
            _context = context;
        }

        [HttpGet("profile"), MapToApiVersion("1")]
        public async Task<IActionResult> GetUserProfile()
        {
            await CheckIsSignoutedAsync();

            var user = await _context.Users.Include(u => u.UserToRoles).ThenInclude(u => u.Role).FirstOrDefaultAsync(u => u.Id == this.CurrentUserId);

            if (user == null)
            {
                throw new ApiException("User not found", (int)HttpStatusCode.BadRequest);
            }

            int ScreenId = 0;
            var roleName = user.UserToRoles.FirstOrDefault().Role;

            switch (roleName.RoleName)
            {
                case RoleTypeHelper.RoleOrderUser:
                    ScreenId = (int)TypeScreen.USER_ORDER;
                    break;
                case RoleTypeHelper.RoleCookieUser:
                    ScreenId = (int)TypeScreen.USER_COOKIE;
                    break;
                case RoleTypeHelper.RolePayUser:
                    ScreenId = (int)TypeScreen.USER_PAY;
                    break;
                case RoleTypeHelper.RoleCustomerUser:
                    ScreenId = (int)TypeScreen.USER_CUSTOMER;
                    break;
            }

            var result = new UserProfileDTO
            {
                FirstName = user.FirstName,
                GAvartar = "https://ragus.vn/wp-content/uploads/2019/10/Yua-Mikami-vlog-1.jpg",
                LastName = user.LastName,
                GroupName = roleName.Desc,
                ScreenId = ScreenId,
            };

            return Ok(result);
        }

        [HttpPost("profile"), MapToApiVersion("1")]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UserProfileUpdateDTO model)
        {
            await CheckIsSignoutedAsync();

            if (ModelState.IsValid)
            {
                // check user exist
                var user = await _context.Users.FindAsync(this.CurrentUserId);
                if (user == null)
                {
                    throw new ApiException("User not found", (int)HttpStatusCode.BadRequest);
                }

                // check password
                if (!_authRepository.VerifyPasswordHash(model.CurrentPassword, user.HashPassword, user.SaltPassword))
                {
                    throw new ApiException("Wrong password.", (int)HttpStatusCode.BadRequest);
                }
                // update user
                _context.Entry(user).State = EntityState.Modified;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Gender = model.Gender;
                user.PhoneNumber = model.PhoneNumber;
                user.UpdatedAt = DateTime.UtcNow;

                if (!string.IsNullOrEmpty(model.NewPassword))
                {
                    var hashPass = Helpers.SercurityHelper.GenerateSaltedHash(model.NewPassword);
                    user.HashPassword = hashPass.Hash;
                    user.SaltPassword = hashPass.Salt;
                }
                // save to database
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (System.Exception ex)
                {
                    {
                        _logger.Log(LogLevel.Warning, "Có lỗi xảy ra khi update user profile", ex.Message);
#if DEBUG
                        throw new ApiException(ex);
#else
                    throw new ApiException("Có lỗi xảy ra khi update database");
#endif
                    }
                }
            }

            return Ok(1);
        }

        [HttpPost("profile"), MapToApiVersion("2")]
        public async Task<IActionResult> UpdateUserProfileV2([FromForm] UserProfileUpdateDTO model)
        {
            await CheckIsSignoutedAsync();

            if (ModelState.IsValid)
            {
                // check user exist
                var user = await _context.Users.FindAsync(this.CurrentUserId);
                if (user == null)
                {
                    throw new ApiException("User not found", (int)HttpStatusCode.BadRequest);
                }

                // check password
                if (!_authRepository.VerifyPasswordHash(model.CurrentPassword, user.HashPassword, user.SaltPassword))
                {
                    throw new ApiException("Wrong password.", (int)HttpStatusCode.BadRequest);
                }
                // update user
                _context.Entry(user).State = EntityState.Modified;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Gender = model.Gender;
                user.PhoneNumber = model.PhoneNumber;
                user.UpdatedAt = DateTime.UtcNow;

                if (!string.IsNullOrEmpty(model.NewPassword))
                {
                    var hashPass = Helpers.SercurityHelper.GenerateSaltedHash(model.NewPassword);
                    user.HashPassword = hashPass.Hash;
                    user.SaltPassword = hashPass.Salt;
                }
                // save to database
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (System.Exception ex)
                {
                    {
                        _logger.Log(LogLevel.Warning, "Có lỗi xảy ra khi update user profile", ex.Message);
#if DEBUG
                        throw new ApiException(ex);
#else
                    throw new ApiException("Có lỗi xảy ra khi update database");
#endif
                    }
                }
            }

            return Ok(1);
        }

    }
}