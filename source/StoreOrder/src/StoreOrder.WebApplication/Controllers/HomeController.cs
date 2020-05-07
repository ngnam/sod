using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StoreOrder.WebApplication.Controllers.ApiBase;
using StoreOrder.WebApplication.Data;
using StoreOrder.WebApplication.Data.Models.Account;
using StoreOrder.WebApplication.Data.Wrappers;

namespace StoreOrder.WebApplication.Controllers
{
    [Authorize]
    [ApiVersion("1")]
    public class HomeController : ApiBaseController
    {
        private readonly IAuthRepository _authRepository;
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<HomeController> _logger;

        public HomeController(
            IAuthRepository authRepository,
            ILogger<HomeController> logger)
        {
            _logger = logger;
            _authRepository = authRepository;
        }

        [HttpGet(""), MapToApiVersion("1")]
        public async Task<IActionResult> Get()
        {
            // check user logout
            await CheckIsSignoutedAsync();
            return Ok(Summaries);
        }

        [HttpGet(""), MapToApiVersion("2")]
        public async Task<IActionResult> GetV2()
        {
            // check user logout
            await CheckIsSignoutedAsync();
            return Ok(HttpContext.GetRequestedApiVersion().ToString());
        }

        private async Task CheckIsSignoutedAsync()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                bool isLogouted = await this._authRepository.CheckUserLogoutedAsync(this.userId, this.currentUserLogin);
                if (isLogouted)
                {
                    throw new ApiException("User đã logout", (int)HttpStatusCode.Unauthorized);
                }
            }
        }
    }
}
