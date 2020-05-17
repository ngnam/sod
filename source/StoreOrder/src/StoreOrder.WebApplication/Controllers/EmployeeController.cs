using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StoreOrder.WebApplication.Controllers.ApiBase;
using StoreOrder.WebApplication.Data;
using StoreOrder.WebApplication.Data.DTO;
using StoreOrder.WebApplication.Data.Models.Products;
using StoreOrder.WebApplication.Data.Models.Stores;
using StoreOrder.WebApplication.Data.Wrappers;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Controllers
{
    [Authorize]
    [ApiVersion("1")]
    [ApiVersion("2")]
    public class EmployeeController : ApiBaseController<EmployeeController>
    {
        private readonly StoreOrderDbContext _context;
        public EmployeeController(
            StoreOrderDbContext context,
            IAuthRepository authRepository,
            ILoggerFactory loggerFactory) : base(loggerFactory, authRepository)
        {
            _context = context;
        }

        [HttpPost("login"), MapToApiVersion("1")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserLoginAuthenticate model)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return Ok(new { isAuthenicated = HttpContext.User.Identity.IsAuthenticated });
            }

            var user = await _authRepository.GetUserByUserNameOrEmail(model.UserNameOrEmail);
            if (user == null)
            {
                _logger.LogInformation("Wrong username or email.");
                throw new ApiException("Wrong username or email.", (int)HttpStatusCode.BadRequest);
            }
            else if (!_authRepository.VerifyPasswordHash(model.Password, user.HashPassword, user.SaltPassword))
            {
                _logger.LogInformation("Wrong password.");
                throw new ApiException("Wrong password.", (int)HttpStatusCode.BadRequest);
            }
            else
            {
                string currentUserId = Guid.NewGuid().ToString();
                var userLogined = _authRepository.GenerateToken(user, currentUserId);
                // save to login
                await _authRepository.SaveToUserLoginAsync(user, userLogined, currentUserId);
                return Ok(userLogined);
            }
        }

        [HttpPost("login"), MapToApiVersion("2")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginV2([FromForm] UserLoginAuthenticate model)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return Ok(new { isAuthenicated = HttpContext.User.Identity.IsAuthenticated });
            }

            var user = await _authRepository.GetUserByUserNameOrEmail(model.UserNameOrEmail);
            if (user == null)
            {
                throw new ApiException("Wrong username or email.", (int)HttpStatusCode.BadRequest);
            }
            else if (!_authRepository.VerifyPasswordHash(model.Password, user.HashPassword, user.SaltPassword))
            {
                throw new ApiException("Wrong password.", (int)HttpStatusCode.BadRequest);
            }
            else
            {
                string currentUserId = Guid.NewGuid().ToString();
                var userLogined = _authRepository.GenerateToken(user, currentUserId);
                // save to login
                await _authRepository.SaveToUserLoginAsync(user, userLogined, currentUserId);
                return Ok(userLogined);
            }
        }

        [HttpGet("tables"), MapToApiVersion("1")]
        public async Task<IActionResult> GetListTables(
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

            // get Store for user
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == this.userId);
            if (user == null)
            {
                throw new ApiException("User not found");
            }

            var result = await ApiResult<StoreTable>.CreateAsync(
                    _context.StoreTables
                    .Where(c => c.StoreId == user.StoreId),
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


            return Ok(new
            {
                Data = dataGroup,
                FilterColumn = result.FilterColumn,
                FilterQuery = result.FilterQuery,
                PageIndex = result.PageIndex,
                PageSize = result.PageSize,
                TotalPages = result.TotalPages,
                TotalCount = result.TotalCount,
                SortColumn = result.SortColumn,
                SortOrder = result.SortOrder,
                HasNextPage = result.HasNextPage,
                HasPreviousPage = result.HasPreviousPage
            });
        }

        [HttpGet("product/category"), MapToApiVersion("1")]
        public async Task<IActionResult> GetCategoryProductsForEmployee(
            int pageIndex = 0,
            int pageSize = 10,
            string sortColumn = null,
            string sortOrder = null,
            string filterColumn = null,
            string filterQuery = null)
        {
            await CheckIsSignoutedAsync();
            // get Store for user
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == this.userId);
            if (user == null)
            {
                throw new ApiException("User not found");
            }
            var result = await ApiResult<CategoryProductDTO>.CreateAsync(
                _context.CategoryProducts
                    .Where(c => c.StoreId == user.StoreId)
                    .Select(c => new CategoryProductDTO
                    {
                        CategoryName = c.CategoryName,
                        Code = c.Code,
                        Slug = c.Slug
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

        [HttpGet("product"), MapToApiVersion("1")]
        public async Task<IActionResult> GetListProductsOfStoreForEmployee(
           int pageIndex = 0,
           int pageSize = 10,
           string sortColumn = null,
           string sortOrder = null,
           string filterColumn = null,
           string filterQuery = null)
        {
            await CheckIsSignoutedAsync();

            // get Store for user
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == this.userId);
            if (user == null)
            {
                throw new ApiException("User not found");
            }

            var query = _context.ProductSKUs
                .Where(p => p.Product.StoreId == user.StoreId)
                .Include(p => p.ProductSKUValues)
                .ThenInclude(p => p.ProductOptionValue)
                .ThenInclude(p => p.ProductOption)
                .ThenInclude(p => p.Product)
                .ThenInclude(p => p.ProductDetail)
                .Select(p => new ProductItemDTO
                {
                    ProductId = p.ProductId,
                    Price = p.Price,
                    ProductName = p.Product.ProductName,
                    Weight = p.Product.Weight,
                    Depth = p.Product.Depth,
                    Height = p.Product.Height,
                    UniversalProductCode = p.Product.UniversalProductCode,
                    ImageHeightThumb = p.Product.ProductDetail.ImageHeightThumb,
                    ImageOrigin = p.Product.ProductDetail.ImageOrigin,
                    ImageThumb = p.Product.ProductDetail.ImageThumb,
                    ImageWidthThumb = p.Product.ProductDetail.ImageWidthThumb,
                    LongDescription = p.Product.ProductDetail.LongDescription,
                    NetWeight = p.Product.NetWeight,
                    Sku = p.Sku,
                    SkuId = p.SkuId,
                    ShortDescription = p.Product.ProductDetail.ShortDescription,
                    OptionId = p.ProductSKUValues.FirstOrDefault(x => x.SkuId == p.SkuId).OptionId,
                    OptionName = p.ProductSKUValues.FirstOrDefault(x => x.SkuId == p.SkuId).ProductOption.OptionName,
                    ValueId = p.ProductSKUValues.FirstOrDefault(x => x.SkuId == p.SkuId).ProductOptionValue.ValueId,
                    ValueName = p.ProductSKUValues.FirstOrDefault(x => x.SkuId == p.SkuId).ProductOptionValue.ValueName,
                });

            var result = await ApiResult<ProductItemDTO>.CreateAsync(
                query,
                pageIndex,
                pageSize,
                sortColumn,
                sortOrder,
                filterColumn,
                filterQuery
            );


            return Ok(result);
        }

        [HttpGet("{categoryIdOrCode}/product"), MapToApiVersion("1")]
        public async Task<IActionResult> GetListProductsOfStoreWithCategoryForEmployee(
            string categoryIdOrCode = "ea76e2a8-873f-48bf-8ace-1415ee3758e8",
            int pageIndex = 0,
            int pageSize = 10,
            string sortColumn = null,
            string sortOrder = null,
            string filterColumn = null,
            string filterQuery = null)
        {
            await CheckIsSignoutedAsync();

            // get Store for user
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == this.userId);
            if (user == null)
            {
                throw new ApiException("User not found");
            }
            var cat = await _context.CategoryProducts.FirstOrDefaultAsync(c => c.Id == categoryIdOrCode || c.Code == categoryIdOrCode);
            if (cat == null)
            {
                throw new ApiException("Category not found", (int)HttpStatusCode.BadRequest);
            }

            var query = _context.ProductSKUs
                .Where(p => p.Product.StoreId == user.StoreId && p.Product.CategoryId == cat.Id)
                .Include(p => p.ProductSKUValues)
                .ThenInclude(p => p.ProductOptionValue)
                .ThenInclude(p => p.ProductOption)
                .ThenInclude(p => p.Product)
                .ThenInclude(p => p.ProductDetail)
                .Select(p => new ProductItemDTO
                {
                    ProductId = p.ProductId,
                    Price = p.Price,
                    ProductName = p.Product.ProductName,
                    Weight = p.Product.Weight,
                    Depth = p.Product.Depth,
                    Height = p.Product.Height,
                    UniversalProductCode = p.Product.UniversalProductCode,
                    ImageHeightThumb = p.Product.ProductDetail.ImageHeightThumb,
                    ImageOrigin = p.Product.ProductDetail.ImageOrigin,
                    ImageThumb = p.Product.ProductDetail.ImageThumb,
                    ImageWidthThumb = p.Product.ProductDetail.ImageWidthThumb,
                    LongDescription = p.Product.ProductDetail.LongDescription,
                    NetWeight = p.Product.NetWeight,
                    Sku = p.Sku,
                    SkuId = p.SkuId,
                    ShortDescription = p.Product.ProductDetail.ShortDescription,
                    OptionId = p.ProductSKUValues.FirstOrDefault(x => x.SkuId == p.SkuId).OptionId,
                    OptionName = p.ProductSKUValues.FirstOrDefault(x => x.SkuId == p.SkuId).ProductOption.OptionName,
                    ValueId = p.ProductSKUValues.FirstOrDefault(x => x.SkuId == p.SkuId).ProductOptionValue.ValueId,
                    ValueName = p.ProductSKUValues.FirstOrDefault(x => x.SkuId == p.SkuId).ProductOptionValue.ValueName,
                });

            var result = await ApiResult<ProductItemDTO>.CreateAsync(
                query,
                pageIndex,
                pageSize,
                sortColumn,
                sortOrder,
                filterColumn,
                filterQuery
            );


            return Ok(result);
        }

        [HttpGet("product/type2"), MapToApiVersion("1")]
        public async Task<IActionResult> GetListProductsOfStoreForEmployeeV2(
           int pageIndex = 0,
           int pageSize = 10,
           string sortColumn = null,
           string sortOrder = null,
           string filterColumn = null,
           string filterQuery = null)
        {
            await CheckIsSignoutedAsync();
            // get Store for user
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == this.userId);
            if (user == null)
            {
                throw new ApiException("User not found");
            }
            var query = _context.Products
                .Where(p => p.StoreId == user.StoreId)
                .Include(p => p.ProductDetail)
                .Include(p => p.ProductSKUs)
                .ThenInclude(p => p.ProductSKUValues)
                .ThenInclude(p => p.ProductOption)
                .ThenInclude(p => p.ProductOptionValues)
                .Select(p => p);

            var result = await ApiResult<Product>.CreateAsync(
                query,
                pageIndex,
                pageSize,
                sortColumn,
                sortOrder,
                filterColumn,
                filterQuery
            );

            var DataGroup = result.Data.Select(p => new ProductItemGroupByNameDTO
            {
                ProductId = p.Id,
                ProductName = p.ProductName,
                FixedPrice = p.FixedPrice,
                UniversalProductCode = p.UniversalProductCode,
                ImageHeightThumb = p.ProductDetail.ImageHeightThumb,
                ImageThumb = p.ProductDetail.ImageThumb,
                ImageWidthThumb = p.ProductDetail.ImageWidthThumb,
                LongDescription = p.ProductDetail.LongDescription,
                ShortDescription = p.ProductDetail.ShortDescription,
                ProductOptionDTOs = p.ProductOptions.Select(po => new ProductOptionDTO
                {
                    OptionId = po.OptionId,
                    OptionName = po.OptionName,
                    ProductOptionValueDTOs = po.ProductOptionValues.Select(pov => new ProductOptionValueDTO
                    {
                        ValueId = pov.ValueId,
                        ValueName = pov.ValueName,
                        SkuId = pov.ProductSKUValues.FirstOrDefault().SkuId,
                        Price = p.ProductSKUs.FirstOrDefault(x => x.SkuId == pov.ProductSKUValues.FirstOrDefault().SkuId).Price,
                        Sku = p.ProductSKUs.FirstOrDefault(x => x.SkuId == pov.ProductSKUValues.FirstOrDefault().SkuId).Sku,
                    })
                }),
            });

            return Ok(new
            {
                Data = DataGroup,
                FilterColumn = result.FilterColumn,
                FilterQuery = result.FilterQuery,
                PageIndex = result.PageIndex,
                PageSize = result.PageSize,
                TotalPages = result.TotalPages,
                TotalCount = result.TotalCount,
                SortColumn = result.SortColumn,
                SortOrder = result.SortOrder,
                HasNextPage = result.HasNextPage,
                HasPreviousPage = result.HasPreviousPage
            });
        }


        [HttpGet("{categoryIdOrCode}/product/type2"), MapToApiVersion("1")]
        public async Task<IActionResult> GetListProductsOfStoreWithCategoryForEmployeeV2(
            string categoryIdOrCode = "ea76e2a8-873f-48bf-8ace-1415ee3758e8",
            int pageIndex = 0,
            int pageSize = 10,
            string sortColumn = null,
            string sortOrder = null,
            string filterColumn = null,
            string filterQuery = null)
        {
            await CheckIsSignoutedAsync();
            // get Store for user
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == this.userId);
            if (user == null)
            {
                throw new ApiException("User not found", (int)HttpStatusCode.BadRequest);
            }
            var cat = await _context.CategoryProducts.FirstOrDefaultAsync(c => c.Id == categoryIdOrCode || c.Code == categoryIdOrCode);
            if (cat == null)
            {
                throw new ApiException("Category not found", (int)HttpStatusCode.BadRequest);
            }
            var query = _context.Products
                .Where(p => p.StoreId == user.StoreId && p.CategoryId == cat.Id)
                .Include(p => p.ProductDetail)
                .Include(p => p.ProductSKUs)
                .ThenInclude(p => p.ProductSKUValues)
                .ThenInclude(p => p.ProductOption)
                .ThenInclude(p => p.ProductOptionValues)
                .Select(p => p);

            var result = await ApiResult<Product>.CreateAsync(
                query,
                pageIndex,
                pageSize,
                sortColumn,
                sortOrder,
                filterColumn,
                filterQuery
            );

            var DataGroup = result.Data.Select(p => new ProductItemGroupByNameDTO
            {
                ProductId = p.Id,
                ProductName = p.ProductName,
                FixedPrice = p.FixedPrice,
                UniversalProductCode = p.UniversalProductCode,
                ImageHeightThumb = p.ProductDetail.ImageHeightThumb,
                ImageThumb = p.ProductDetail.ImageThumb,
                ImageWidthThumb = p.ProductDetail.ImageWidthThumb,
                LongDescription = p.ProductDetail.LongDescription,
                ShortDescription = p.ProductDetail.ShortDescription,
                ProductOptionDTOs = p.ProductOptions.Select(po => new ProductOptionDTO
                {
                    OptionId = po.OptionId,
                    OptionName = po.OptionName,
                    ProductOptionValueDTOs = po.ProductOptionValues.Select(pov => new ProductOptionValueDTO
                    {
                        ValueId = pov.ValueId,
                        ValueName = pov.ValueName,
                        SkuId = pov.ProductSKUValues.FirstOrDefault().SkuId,
                        Price = p.ProductSKUs.FirstOrDefault(x => x.SkuId == pov.ProductSKUValues.FirstOrDefault().SkuId).Price,
                        Sku = p.ProductSKUs.FirstOrDefault(x => x.SkuId == pov.ProductSKUValues.FirstOrDefault().SkuId).Sku,
                    })
                }),
            });

            return Ok(new
            {
                Data = DataGroup,
                FilterColumn = result.FilterColumn,
                FilterQuery = result.FilterQuery,
                PageIndex = result.PageIndex,
                PageSize = result.PageSize,
                TotalPages = result.TotalPages,
                TotalCount = result.TotalCount,
                SortColumn = result.SortColumn,
                SortOrder = result.SortOrder,
                HasNextPage = result.HasNextPage,
                HasPreviousPage = result.HasPreviousPage
            });
        }

        [HttpPost("order"), MapToApiVersion("1")]
        public async Task<IActionResult> StoreOrder([FromBody] OrderProductDTO model)
        {
            await CheckIsSignoutedAsync();
            if (ModelState.IsValid)
            {
                return Ok();
            }
            return Ok(1);
        }

        [HttpPost("order"), MapToApiVersion("2")]
        public async Task<IActionResult> StoreOrderV2([FromForm] OrderProductDTO model)
        {
            await CheckIsSignoutedAsync();
            if (ModelState.IsValid)
            {
                return Ok();
            }
            return Ok(1);
        }

    }
}