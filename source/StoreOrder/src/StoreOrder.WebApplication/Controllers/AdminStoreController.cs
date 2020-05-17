using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StoreOrder.WebApplication.Controllers.ApiBase;
using StoreOrder.WebApplication.Data;
using StoreOrder.WebApplication.Data.DTO;
using StoreOrder.WebApplication.Data.Enums;
using StoreOrder.WebApplication.Data.Models.Account;
using StoreOrder.WebApplication.Data.Models.Stores;
using StoreOrder.WebApplication.Data.Repositories.Interfaces;
using StoreOrder.WebApplication.Data.Wrappers;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Controllers
{
    [Authorize]
    [ApiVersion("1")]
    [ApiController]
    public class AdminStoreController : ApiBaseController<AdminStoreController>
    {
        private readonly StoreOrderDbContext _context;

        public AdminStoreController(
              StoreOrderDbContext context,
              IAuthRepository authRepository,
              ILoggerFactory loggerFactory) : base(loggerFactory, authRepository)
        {
            _context = context;
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
            _logger.Log(LogLevel.Information, "GetListStores");
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

        [HttpGet("product"), MapToApiVersion("1")]
        [Authorize]
        public async Task<IActionResult> GetMenuProduct(
            int pageIndex = 0,
            int pageSize = 10,
            string sortColumn = null,
            string sortOrder = null,
            string filterColumn = null,
            string filterQuery = null)
        {
            _logger.Log(LogLevel.Information, "get product");

            string messager = string.Empty;
            // get stores of user admin
            User user = await _context.Users.FindAsync(this.userId);
            if (user == null)
            {
                messager = "Không tìm thấy user.";
                _logger.Log(LogLevel.Information, messager);
                throw new ApiException(messager, (int)HttpStatusCode.BadRequest);
            }
            Store store = await _context.Stores.FindAsync(user.StoreId);
            if (store == null)
            {
                messager = "Không tìm thấy store.";
                _logger.Log(LogLevel.Information, messager);
                throw new ApiException(messager, (int)HttpStatusCode.BadRequest);
            }

            var result = await ApiResult<ProductAdminDTO>.CreateAsync(
                _context.Products
                .Where(p => p.StoreId == store.Id)
                .Select(p => new ProductAdminDTO
                {

                    Id = p.Id,
                    CategoryId = p.CategoryId,
                    CreateByUserId = p.CreateByUserId,
                    CreateOn = p.CreateOn,
                    UpdateOn = p.UpdateOn,
                    DeleteOn = p.DeleteOn,
                    Depth = p.Depth,
                    Height = p.Height,
                    NetWeight = p.NetWeight,
                    Weight = p.Weight,
                    UniversalProductCode = p.UniversalProductCode,
                    ProductName = p.ProductName,
                }),
                pageIndex,
                pageSize,
                sortColumn,
                sortOrder,
                filterColumn,
                filterQuery
            );

            return Ok(result);
        }

        [HttpGet("attribute"), MapToApiVersion("1")]
        public async Task<IActionResult> GetListStoreOption(
            int pageIndex = 0,
            int pageSize = 10,
            string sortColumn = null,
            string sortOrder = null,
            string filterColumn = null,
            string filterQuery = null)
        {
            _logger.Log(LogLevel.Information, "get attribute");
            await CheckIsSignoutedAsync();

            string messager = string.Empty;
            // get stores of user admin
            User user = await _context.Users.FindAsync(this.userId);
            if (user == null)
            {
                messager = "Không tìm thấy user.";
                _logger.Log(LogLevel.Information, messager);
                throw new ApiException(messager, (int)HttpStatusCode.BadRequest);
            }
            Store store = await _context.Stores.FindAsync(user.StoreId);
            if (store == null)
            {
                messager = "Không tìm thấy store.";
                _logger.Log(LogLevel.Information, messager);
                throw new ApiException(messager, (int)HttpStatusCode.BadRequest);
            }
            var result = await ApiResult<StoreOptionItemDTO>.CreateAsync(
                _context.StoreOptions.Select(s => new StoreOptionItemDTO
                {
                    OptionId = s.OptionId,
                    Name = s.StoreOptionName,
                    Desc = s.StoreOptionDescription
                }),
                pageIndex,
                pageSize,
                sortColumn,
                sortOrder,
                filterColumn,
                filterQuery
            );

            return Ok(result);
        }

        [HttpGet("category"), MapToApiVersion("1")]
        public async Task<IActionResult> GetListCategoryStore(
           int pageIndex = 0,
           int pageSize = 10,
           string sortColumn = null,
           string sortOrder = null,
           string filterColumn = null,
           string filterQuery = null)
        {
            _logger.LogInformation("get category");

            var result = await ApiResult<CategoryStoreDTO>.CreateAsync(
                _context.CategoryStores.Select(c => new CategoryStoreDTO
                {
                    Id = c.Id,
                    Name = c.Name
                }),
                pageIndex,
                pageSize,
                sortColumn,
                sortOrder,
                filterColumn,
                filterQuery
            );
            return Ok(result);
        }

    }
}