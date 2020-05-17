using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StoreOrder.WebApplication.Controllers.ApiBase;
using StoreOrder.WebApplication.Data.Repositories.Interfaces;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Controllers
{
    [Authorize]
    [ApiVersion("1")]
    public class HomeController : ApiBaseController<HomeController>
    {
        public HomeController(
            IAuthRepository authRepository,
            ILoggerFactory loggerFactory) : base(loggerFactory, authRepository)
        {
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
    }
}
