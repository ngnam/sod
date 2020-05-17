using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StoreOrder.WebApplication.Data.Repositories.Interfaces;
using StoreOrder.WebApplication.Data.Wrappers;
using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Controllers.ApiBase
{
    [Produces("application/json", "application/problem+json")]
    [Route("[controller]")]
    [ApiController]
    public abstract class ApiBaseController<T> : ControllerBase
    {
        protected readonly ILogger<T> _logger;
        protected readonly IAuthRepository _authRepository;
        public ApiBaseController(
            ILoggerFactory loggerFactory,
            IAuthRepository authRepository)
        {
            _logger = loggerFactory.CreateLogger<T>();
            _authRepository = authRepository;
        }
        private string _userId;
        protected string userId
        {
            get
            {
                if (string.IsNullOrEmpty(this._userId) && HttpContext.User.Identity.IsAuthenticated)
                {
                    //First get user claims    
                    var claims = HttpContext.User.Claims;
                    this._userId = claims?.FirstOrDefault(x => x.Type.Equals(ClaimTypes.PrimarySid, StringComparison.OrdinalIgnoreCase))?.Value;
                }
                return this._userId;
            }
        }

        private string _currentUserLogin;
        protected string currentUserLogin
        {
            get
            {
                if (string.IsNullOrEmpty(this._currentUserLogin) && HttpContext.User.Identity.IsAuthenticated)
                {
                    //First get user claims    
                    var claims = HttpContext.User.Claims;
                    this._currentUserLogin = claims?.FirstOrDefault(x => x.Type.Equals(ClaimTypes.NameIdentifier, StringComparison.OrdinalIgnoreCase))?.Value;
                }
                return this._currentUserLogin;
            }
        }

        protected async Task CheckIsSignoutedAsync()
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
