using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreOrder.WebApplication.Data;
using StoreOrder.WebApplication.Data.DTO;
using StoreOrder.WebApplication.Data.Wrappers;
using StoreOrder.WebApplication.Extensions;
using System.Net;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Controllers
{
    [Produces("application/json", "application/problem+json")]
    [Route("[controller]")]
    [ApiVersion("1")]
    [ApiController]
    public class AccountController : ControllerBase
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
            if (!ModelState.IsValid)
            {
                throw new ApiException(ModelState.AllErrors(), (int)HttpStatusCode.BadRequest);
                //return BadRequest(new ApiError(ResponseMessageEnum.ValidationError.GetDescription(), ModelState.AllErrors()));
            }

            var user = await _authRepository.GetUserByUserNameOrEmail(model.UserNameOrEmail);
            if (user == null)
            {
                throw new ApiException("Wrong username or email.");
                //return BadRequest(new ApiError("User not found."));
            }
            else if (!_authRepository.VerifyPasswordHash(model.Password, user.HashPassword, user.SaltPassword))
            {
                throw new ApiException("Wrong password.");
                //return BadRequest(new ApiError("Wrong password."));
            } else { 
                return Ok(new { data = _authRepository.GenerateToken(user) });
            }
        }

    }
}