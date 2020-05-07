using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace StoreOrder.WebApplication.Controllers.ApiBase
{
    [Produces("application/json", "application/problem+json")]
    [Route("[controller]")]
    [ApiController]
    public abstract class ApiBaseController : ControllerBase
    {
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
    }
}
