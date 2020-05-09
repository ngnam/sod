using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StoreOrder.WebApplication.Controllers.ApiBase;
using StoreOrder.WebApplication.Data;
using StoreOrder.WebApplication.Data.DTO;
using StoreOrder.WebApplication.Data.Enums;
using StoreOrder.WebApplication.Data.Models.Stores;
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
                    .Where(c => c.StatusStore == (int)TypeStatusStore.Open)
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

        [HttpGet("tables"), MapToApiVersion("1")]
        public async Task<IActionResult> GetListTables(
            string id = "b4d32aca-665d-4253-83a5-6f6a8b7acade",
            int pageIndex = 0,
            int pageSize = 10,
            string sortColumn = null,
            string sortOrder = null,
            string filterColumn = null,
            string filterQuery = null
            )
        {
            // check user logout
            await CheckIsSignoutedAsync();

            var result = await ApiResult<StoreTable>.CreateAsync(
                    _context.StoreTables
                    .Where(c => c.StoreId == id),
                    pageIndex,
                    pageSize,
                    sortColumn,
                    sortOrder,
                    filterColumn,
                    filterQuery);

            var dataGroup =
                result.Data.GroupBy(c => c.Location)
                    .Select(c => new StoreTableDTOGroupByLocalionDTO
                    {
                        location = c.Key,
                        lstTable = c.DefaultIfEmpty()
                    .Select(table => new StoreTableDTO
                    {
                        Id = table.Id,
                        Location = table.Location,
                        LocationUnit = table.LocationUnit,
                        StoreId = table.StoreId,
                        TableName = table.TableName,
                        TableCode = table.TableCode,
                        TableStatus = table.TableStatus
                    })
                    });


            return Ok(dataGroup);
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