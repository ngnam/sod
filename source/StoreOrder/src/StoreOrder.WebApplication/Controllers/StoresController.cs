using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StoreOrder.WebApplication.Controllers.ApiBase;
using StoreOrder.WebApplication.Data;
using StoreOrder.WebApplication.Data.DTO;
using StoreOrder.WebApplication.Data.Enums;
using StoreOrder.WebApplication.Data.Wrappers;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Controllers
{
    [Authorize]
    [ApiVersion("1")]
    public class StoresController : ApiBaseController
    {
        private readonly StoreOrderDbContext _context;
        private readonly IAuthRepository _authRepository;
        private readonly ILogger<StoresController> _logger;
        public StoresController(
            StoreOrderDbContext context,
            IAuthRepository authRepository,
            ILogger<StoresController> logger)
        {
            _logger = logger;
            _context = context;
            _authRepository = authRepository;
        }

        [HttpGet(""), MapToApiVersion("1")]
        public async Task<IActionResult> GetListStores(
                int pageIndex = 0,
                int pageSize = 10,
                string sortColumn = null,
                string sortOrder = null,
                string filterColumn = null,
                string filterQuery = null)
        {
            // check user logout
            await CheckIsSignoutedAsync();

            var result = await ApiResult<StoreItemDTO>.CreateAsync(
                    _context.Stores
                    .Where(c => c.UserId == this.userId && c.StatusStore == (int)TypeStatusStore.Open)
                    .Select(c => new StoreItemDTO()
                    {
                        Id = c.Id,
                        lat = c.lat,
                        lon = c.lon,
                        StatusStore = c.StatusStore,
                        StoreAddress = c.StoreAddress,
                        StoreName = c.StoreName,
                        UpdatedAt = c.UpdatedAt,
                        CreatedAt = c.CreatedAt
                    }),
                    pageIndex,
                    pageSize,
                    sortColumn,
                    sortOrder,
                    filterColumn,
                    filterQuery);

            return Ok(result);
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