using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StoreOrder.WebApplication.Authorization;
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
        private string _curentUserId;
        protected string CurrentUserId
        {
            get
            {
                if (string.IsNullOrEmpty(this._curentUserId) && HttpContext.User.Identity.IsAuthenticated)
                {
                    //First get user claims    
                    this._curentUserId = HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals(UserSignedClaimTypes.CurrentUserId, StringComparison.OrdinalIgnoreCase))?.Value;
                }
                return this._curentUserId;
            }
        }

        private string _userIdentifierId;
        protected string UserIdentifierId
        {
            get
            {
                if (string.IsNullOrEmpty(this._userIdentifierId) && HttpContext.User.Identity.IsAuthenticated)
                {
                    //First get user claims    
                    this._userIdentifierId = HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals(UserSignedClaimTypes.UserIdentifierId, StringComparison.OrdinalIgnoreCase))?.Value;
                }
                return this._userIdentifierId;
            }
        }

        protected async Task CheckIsSignoutedAsync()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                bool isLogouted = await this._authRepository.CheckUserLogoutedAsync(this.CurrentUserId, this.UserIdentifierId);
                if (isLogouted)
                {
                    throw new ApiException("User has signout", (int)HttpStatusCode.Unauthorized);
                }
            }
        }

        private string _StoreId; 
        protected string UserStoreId
        {
            get
            {
                if (string.IsNullOrEmpty(this._StoreId) && HttpContext.User.Identity.IsAuthenticated)
                {
                    this._StoreId = HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals(UserSignedClaimTypes.StoreIdentifierId, StringComparison.OrdinalIgnoreCase))?.Value;
                }
                return this._StoreId;
            }
        }
    }
}
