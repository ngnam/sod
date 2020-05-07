using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreOrder.WebApplication.Controllers.ApiBase;
using StoreOrder.WebApplication.Data;
using StoreOrder.WebApplication.Data.DTO;
using StoreOrder.WebApplication.Data.Wrappers;
using System;
using System.Net;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Controllers
{
    [ApiVersion("1")]
    public class AccountController : ApiBaseController
    {
        private readonly IAuthRepository _authRepository;
        public AccountController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpGet(""), MapToApiVersion("1")]
        [AllowAnonymous]
        public IActionResult LoginTest()
        {
            throw new ApiException("Wrong username or email.");
        }

        [HttpPost("login/staff"), MapToApiVersion("1")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserLoginAuthenticate model)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return Ok(new { isAuthenicated = HttpContext.User.Identity.IsAuthenticated });
            }

            var user = await _authRepository.GetUserByUserNameOrEmail(model.UserNameOrEmail);
            if (user == null)
            {
                throw new ApiException("Wrong username or email.", (int)HttpStatusCode.BadRequest);
            }
            else if (!_authRepository.VerifyPasswordHash(model.Password, user.HashPassword, user.SaltPassword))
            {
                throw new ApiException("Wrong password.", (int)HttpStatusCode.BadRequest);
            }
            else
            {
                string currentUserId = Guid.NewGuid().ToString();
                var userLogined = _authRepository.GenerateToken(user, currentUserId);
                // save to login
                await _authRepository.SaveToUserLoginAsync(user, userLogined, currentUserId);
                return Ok(userLogined);
            }
        }

        [HttpGet("logout"), MapToApiVersion("1")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var isLogout = await _authRepository.LogoutAsync(this.userId, this.currentUserLogin);
            return Ok(new { isLogout = isLogout });
        }

        [HttpPost("login/customer"), MapToApiVersion("1")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAndRegisterCustomer([FromBody] CustomerLoginDTO model)
        {
            var userLogined = new UserLogined();
            if (ModelState.IsValid)
                userLogined = await _authRepository.SignInAndSignUpCustomerAsync(model);
            return Ok(userLogined);
        }
    }
}