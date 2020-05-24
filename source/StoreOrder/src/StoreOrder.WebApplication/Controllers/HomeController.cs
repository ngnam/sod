using Imgur.API.Authentication.Impl;
using Imgur.API.Endpoints.Impl;
using Imgur.API.Enums;
using Imgur.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StoreOrder.WebApplication.Controllers.ApiBase;
using StoreOrder.WebApplication.Data.Repositories.Interfaces;
using StoreOrder.WebApplication.Settings;
using System.Linq;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Controllers
{
    [Authorize]
    [ApiVersion("1")]
    public class HomeController : ApiBaseController<HomeController>
    {
        protected readonly IConfiguration _configuration;
        public HomeController(
            IConfiguration configuration,
            IAuthRepository authRepository,
            ILoggerFactory loggerFactory) : base(loggerFactory, authRepository)
        {
            _configuration = configuration;
        }

        [HttpGet(""), MapToApiVersion("1")]
        public async Task<IActionResult> Get()
        {
            // check user logout
            await CheckIsSignoutedAsync();
            return Ok(1);
        }

        [HttpGet(""), MapToApiVersion("2")]
        public async Task<IActionResult> GetV2()
        {
            // check user logout
            await CheckIsSignoutedAsync();
            return Ok(1);
        }

        [HttpGet("login-imgur"), MapToApiVersion("1")]
        [AllowAnonymous]
        public IActionResult LoginImgUr()
        {
            var client = new ImgurClient(this.imgUrUploadSettings.ClientID, this.imgUrUploadSettings.ClientSecret);
            var endpoint = new OAuth2Endpoint(client);
            var authorizationUrl = endpoint.GetAuthorizationUrl(OAuth2ResponseType.Token);
            return Ok(new { data = authorizationUrl });
        }

        [HttpGet("callback"), MapToApiVersion("1")]
        [AllowAnonymous]
        public IActionResult CallBack()
        {
            return Ok(1);
        }

        private ImgUrUpload _imgUrUploadSettings;
        protected ImgUrUpload imgUrUploadSettings
        {
            get
            {
                if (_imgUrUploadSettings == null) this._imgUrUploadSettings = _configuration.GetSection("ImgUrUpload").Get<ImgUrUpload>();
                return this._imgUrUploadSettings;
            }
        }
    }
}
