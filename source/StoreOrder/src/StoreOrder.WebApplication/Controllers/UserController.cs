using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StoreOrder.WebApplication.Controllers.ApiBase;
using StoreOrder.WebApplication.Data;
using StoreOrder.WebApplication.Data.DTO;
using StoreOrder.WebApplication.Data.Enums;
using StoreOrder.WebApplication.Data.Models.Account;
using StoreOrder.WebApplication.Data.Repositories.Interfaces;
using StoreOrder.WebApplication.Data.Wrappers;
using System;
using System.Collections.Generic;
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

            var user = await _context.Users
                .Include(u => u.UserToRoles)
                .ThenInclude(u => u.Role)
                .Include(u => u.Store)
                .Include(u => u.UserDetail)
                .FirstOrDefaultAsync(u => u.Id == this.CurrentUserId && u.IsActived == (int)TypeVerified.Verified);

            if (user == null)
            {
                throw new ApiException("User not found", (int)HttpStatusCode.BadRequest);
            }

            var roleUser = user.UserToRoles.Select(x => x.Role).ToList();

            var result = new UserProfileDTO
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                GroupScreens = roleUser.Select(r => new GroupScreen
                {
                    GroupName = r.Desc,
                    ScreenId = MapScreenId(r.RoleName)
                }).ToList(),
                Gender = user.Gender,
                StoreName = user.Store.StoreName,
                BirthDay = user.BirthDay,
                ProviderId = user.UserDetail != null ? user.UserDetail?.ProvideId : string.Empty,
                Address1 = user.UserDetail != null ? user.UserDetail?.Address1 : string.Empty,
                Address2 = user.UserDetail != null ? user.UserDetail?.Address2 : string.Empty,
                Address3 = user.UserDetail != null ? user.UserDetail?.Address3 : string.Empty,
                GAvartar = user.UserDetail != null ? user.UserDetail.GAvartar : "https://ifg.onecmscdn.com/2019/06/18/screenshot3-1560840221436466431758.jpg"
            };

            return Ok(result);
        }

        private int MapScreenId(string roleName)
        {
            int screenId = 0;
            switch (roleName)
            {
                case RoleTypeHelper.RoleOrderUser:
                    screenId = (int)TypeScreen.USER_ORDER;
                    break;
                case RoleTypeHelper.RoleCookieUser:
                    screenId = (int)TypeScreen.USER_COOKIE;
                    break;
                case RoleTypeHelper.RoleCustomerUser:
                    screenId = (int)TypeScreen.USER_CUSTOMER;
                    break;
                case RoleTypeHelper.RolePayUser:
                    screenId = (int)TypeScreen.USER_PAY;
                    break;
            }
            return screenId;
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
                user.FirstName = model.FirstName ?? "Nhân viên";
                user.LastName = model.LastName ?? "ABC";
                user.Gender = model.Gender ?? 1;
                user.PhoneNumber = model.PhoneNumber ?? string.Empty;
                user.UpdatedAt = DateTime.UtcNow;

                //// Update Birth day
                //if (model.YearOfBirth != 0 && model.MonthOfBirth != 0 && model.DayOfBirth != 0)
                //{
                //    user.BirthDay = new DateTime(model.YearOfBirth, model.MonthOfBirth, model.DayOfBirth);
                //}
                // Update Birth day
                if (model.YearOfBirth.HasValue && model.MonthOfBirth.HasValue && model.DayOfBirth.HasValue)
                {
                    user.BirthDay = new DateTime(model.YearOfBirth.Value, model.MonthOfBirth.Value, model.DayOfBirth.Value);
                }

                if (!string.IsNullOrEmpty(model.NewPassword) && model.NewPassword != "string")
                {
                    var hashPass = Helpers.SercurityHelper.GenerateSaltedHash(model.NewPassword.Trim());
                    user.HashPassword = hashPass.Hash;
                    user.SaltPassword = hashPass.Salt;
                }

                // add User Detail
                if (user.UserDetail != null)
                {
                    // update
                    user.UserDetail.ProvideId = model.ProviderId ?? string.Empty;
                    user.UserDetail.Address1 = model.Address1 ?? string.Empty;
                    user.UserDetail.Address2 = model.Address2 ?? string.Empty;
                    user.UserDetail.Address3 = model.Address3 ?? string.Empty;
                    user.UserDetail.GAvartar = model.GAvartar ?? string.Empty;
                } else
                {
                    UserDetail newUserDetail = new UserDetail();
                    newUserDetail.ProvideId = model.ProviderId ?? string.Empty;
                    newUserDetail.Address1 = model.Address1 ?? string.Empty;
                    newUserDetail.Address2 = model.Address2 ?? string.Empty;
                    newUserDetail.Address3 = model.Address3 ?? string.Empty;
                    newUserDetail.GAvartar = model.GAvartar ?? string.Empty;
                    user.UserDetail = newUserDetail;
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

                // Update Birth day
                if (model.YearOfBirth.HasValue && model.MonthOfBirth.HasValue && model.DayOfBirth.HasValue)
                {
                    user.BirthDay = new DateTime(model.YearOfBirth.Value, model.MonthOfBirth.Value, model.DayOfBirth.Value);
                }

                if (!string.IsNullOrEmpty(model.NewPassword))
                {
                    var hashPass = Helpers.SercurityHelper.GenerateSaltedHash(model.NewPassword);
                    user.HashPassword = hashPass.Hash;
                    user.SaltPassword = hashPass.Salt;
                }

                // add User Detail
                var userDetail = await _context.UserDetails.FindAsync(user.Id);
                if (userDetail != null)
                {
                    // update
                    userDetail.ProvideId = model.ProviderId ?? string.Empty;
                    userDetail.Address1 = model.Address1 ?? string.Empty;
                    userDetail.Address2 = model.Address2 ?? string.Empty;
                    userDetail.Address3 = model.Address3 ?? string.Empty;
                    userDetail.GAvartar = model.GAvartar ?? string.Empty;
                }
                else
                {
                    UserDetail newUserDetail = new UserDetail();
                    newUserDetail.ProvideId = model.ProviderId ?? string.Empty;
                    newUserDetail.Address1 = model.Address1 ?? string.Empty;
                    newUserDetail.Address2 = model.Address2 ?? string.Empty;
                    newUserDetail.Address3 = model.Address3 ?? string.Empty;
                    newUserDetail.GAvartar = model.GAvartar ?? string.Empty;
                    _context.UserDetails.Add(newUserDetail);
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