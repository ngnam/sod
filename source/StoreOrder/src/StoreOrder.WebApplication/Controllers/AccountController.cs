using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StoreOrder.WebApplication.Controllers.ApiBase;
using StoreOrder.WebApplication.Data;
using StoreOrder.WebApplication.Data.DTO;
using StoreOrder.WebApplication.Data.Enums;
using StoreOrder.WebApplication.Data.Models.Account;
using StoreOrder.WebApplication.Data.Models.Stores;
using StoreOrder.WebApplication.Data.Repositories.Interfaces;
using StoreOrder.WebApplication.Data.Wrappers;
using System;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Controllers
{
    [Authorize]
    [ApiVersion("1")]
    [ApiVersion("2")]
    public class AccountController : ApiBaseController<AccountController>
    {
        private readonly StoreOrderDbContext _context;

        public AccountController(
            IAuthRepository authRepository,
            ILoggerFactory loggerFactory,
            StoreOrderDbContext context) : base(loggerFactory, authRepository)
        {
            _context = context;
        }

        [HttpGet(""), MapToApiVersion("1")]
        public IActionResult LoginTest()
        {
            _logger.LogInformation("LoginTest test");
            throw new ApiException("Đi chỗ khác chơi đi, vào đây phá tao báo công an :)");
        }

        [HttpGet("logout"), MapToApiVersion("1")]
        public async Task<IActionResult> Logout()
        {
            _logger.LogInformation("Logout");
            var isLogout = await _authRepository.LogoutAsync(this.userId, this.currentUserLogin);
            return Ok(new { isLogout = isLogout });
        }

        [HttpPost("registration/{storeId}"), MapToApiVersion("1")]
        public async Task<IActionResult> RegistrationUser([FromBody] RegistrationUserDTO model, string storeId)
        {
            _logger.Log(LogLevel.Information, "call registration/{@storeId}", storeId);
            await CheckIsSignoutedAsync();
            string messager = string.Empty;
            if (ModelState.IsValid)
            {
                // check store exist and open
                Store store = await _context.Stores.FindAsync(storeId);
                if (store == null)
                {
                    messager = "Không tìm thấy cửa hàng hoặc cửa hàng đã đóng.";
                    _logger.Log(LogLevel.Information, messager);
                    throw new ApiException(messager);
                }

                // get role
                Role role = await _context.Roles.FindAsync(storeId);
                if (role == null)
                {
                    messager = "Không tìm thấy role.";
                    _logger.Log(LogLevel.Information, messager);
                    throw new ApiException(messager);
                }

                // init user
                var hashPass = Helpers.SercurityHelper.GenerateSaltedHash(model.Password);
                User user = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    Age = model.Age,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    UserName = model.Username,
                    StoreId = store.Id,
                    Gender = model.Gender,
                    IsActived = (int)TypeVerified.Verified,
                    HashPassword = hashPass.Hash,
                    SaltPassword = hashPass.Salt
                };

                // Add user to Role
                UserToRole userToRole = new UserToRole();

                userToRole.Role = role;
                userToRole.User = user;

                role.UserToRoles.Add(userToRole);

                // save user create to database
                try
                {
                    _context.Roles.Add(role);
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                    messager = $"Đã khởi tạo nhân viên {role.Desc}: {user.FirstName} {user.LastName} cho cửa hàng của bạn.";
                }
                catch (Exception ex)
                {
                    _logger.Log(LogLevel.Warning, $"Log when RegistrationUser {ex.Message}");
#if !DEBUG
                    throw new ApiException($"Có lỗi xảy ra khi Đăng ký {user.FirstName}");
#else
                    throw new ApiException(ex);
#endif
                }

            }

            _logger.Log(LogLevel.Information, messager);
            return Ok(new { data = messager });
        }

        [HttpPost("registration/{storeId}"), MapToApiVersion("2")]
        public async Task<IActionResult> RegistrationUserV2([FromForm] RegistrationUserDTO model, string storeId)
        {
            _logger.Log(LogLevel.Information, "call registration/{@storeId}", storeId);
            await CheckIsSignoutedAsync();
            string messager = string.Empty;
            if (ModelState.IsValid)
            {
                // check store exist and open
                Store store = await _context.Stores.FindAsync(storeId);
                if (store == null)
                {
                    messager = "Không tìm thấy cửa hàng hoặc cửa hàng đã đóng.";
                    _logger.Log(LogLevel.Information, messager);
                    throw new ApiException(messager);
                }

                // get role
                Role role = await _context.Roles.FindAsync(storeId);
                if (role == null)
                {
                    messager = "Không tìm thấy role.";
                    _logger.Log(LogLevel.Information, messager);
                    throw new ApiException(messager);
                }

                // init user
                var hashPass = Helpers.SercurityHelper.GenerateSaltedHash(model.Password);
                User user = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    Age = model.Age,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    UserName = model.Username,
                    StoreId = store.Id,
                    Gender = model.Gender,
                    IsActived = (int)TypeVerified.Verified,
                    HashPassword = hashPass.Hash,
                    SaltPassword = hashPass.Salt
                };

                // Add user to Role
                UserToRole userToRole = new UserToRole();

                userToRole.Role = role;
                userToRole.User = user;

                role.UserToRoles.Add(userToRole);

                // save user create to database
                try
                {
                    _context.Roles.Add(role);
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                    messager = $"Đã khởi tạo nhân viên {role.Desc}: {user.FirstName} {user.LastName} cho cửa hàng của bạn.";
                }
                catch (Exception ex)
                {
                    _logger.Log(LogLevel.Warning, $"Log when RegistrationUser {ex.Message}");
#if !DEBUG
                    throw new ApiException($"Có lỗi xảy ra khi Đăng ký {user.FirstName}");
#else
                    throw new ApiException(ex);
#endif
                }

            }

            _logger.Log(LogLevel.Information, messager);
            return Ok(new { data = messager });
        }
    }
}