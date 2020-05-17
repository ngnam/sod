using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StoreOrder.WebApplication.Controllers.ApiBase;
using StoreOrder.WebApplication.Data;
using StoreOrder.WebApplication.Data.DTO;
using StoreOrder.WebApplication.Data.Enums;
using StoreOrder.WebApplication.Data.Models.Account;
using StoreOrder.WebApplication.Data.Models.Products;
using StoreOrder.WebApplication.Data.Models.Stores;
using StoreOrder.WebApplication.Data.Wrappers;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Controllers
{
    [Authorize]
    [ApiVersion("1")]
    [ApiVersion("2")]
    public class CustomerController : ApiBaseController<CustomerController>
    {
        private readonly StoreOrderDbContext _context;
        public CustomerController(
            StoreOrderDbContext context,
            IAuthRepository authRepository,
            ILoggerFactory loggerFactory) : base(loggerFactory, authRepository)
        {
            _context = context;
        }

        [HttpPost("login"), MapToApiVersion("1")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAndRegisterCustomer([FromBody] CustomerLoginDTO model)
        {
            var userLogined = new UserLogined();
            if (ModelState.IsValid)
                userLogined = await _authRepository.SignInAndSignUpCustomerAsync(model);
            return Ok(userLogined);
        }

        [HttpPost("login"), MapToApiVersion("2")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAndRegisterCustomerV2([FromForm] CustomerLoginDTO model)
        {
            var userLogined = new UserLogined();
            if (ModelState.IsValid)
                userLogined = await _authRepository.SignInAndSignUpCustomerAsync(model);
            return Ok(userLogined);
        }

        [HttpGet("{storeId}/product/category"), MapToApiVersion("1")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategoryProductsForCustomer(
            string storeId = "b4d32aca-665d-4253-83a5-6f6a8b7acade",
            int pageIndex = 0,
            int pageSize = 10,
            string sortColumn = null,
            string sortOrder = null,
            string filterColumn = null,
            string filterQuery = null)
        {
            var result = await ApiResult<CategoryProductDTO>.CreateAsync(
                _context.CategoryProducts
                    .Where(c => c.StoreId == storeId)
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

        [HttpPost("registor"), MapToApiVersion("2")]
        [AllowAnonymous]
        public async Task<IActionResult> RegistorStoreV2([FromForm] RegistorStoreDTO model)
        {
            string messager = string.Empty;
            if (ModelState.IsValid)
            {
                // init store entity
                Store store = new Store
                {
                    Id = Guid.NewGuid().ToString(),
                    CategoryStoreId = model.CatStoreId,
                    lat = model.lat,
                    lon = model.lon,
                    ProviderId = model.ProviderId,
                    StatusStore = (int)TypeStatusStore.Open,
                    StoreAddress = model.StoreAddress,
                    StoreName = model.StoreName,
                };
                // init user of store
                var hashPass = Helpers.SercurityHelper.GenerateSaltedHash(model.Password);
                User user = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    UserName = model.Email,
                    HashPassword = hashPass.Hash,
                    SaltPassword = hashPass.Salt,
                    StoreId = store.Id,
                    IsActived = (int)TypeVerified.Verified
                };
                store.CreateByUserId = user.Id;
                store.Users.Add(user);
                // save store to database

                try
                {
                    await _context.Stores.AddAsync(store);
                    await _context.SaveChangesAsync();
                    messager = $"Khởi tạo cửa hàng {store.StoreName} thành công, vui lòng check email và đăng nhập để quản trị.";
                }
                catch (Exception ex)
                {
                    _logger.Log(LogLevel.Warning, $"Log when RegistorStore {ex.Message}");
#if DEBUG
                    throw new ApiException(ex);
#else
                    throw new ApiException($"Có lỗi xảy ra khi thêm mới store {store.StoreName}");
#endif
                }
            }

            _logger.Log(LogLevel.Information, messager);
            return Ok(new { data = messager });
        }

        [HttpPost("registor"), MapToApiVersion("1")]
        [AllowAnonymous]
        public async Task<IActionResult> RegistorStore([FromBody] RegistorStoreDTO model)
        {
            string messager = string.Empty;
            if (ModelState.IsValid)
            {
                // init store entity
                Store store = new Store
                {
                    Id = Guid.NewGuid().ToString(),
                    CategoryStoreId = model.CatStoreId,
                    lat = model.lat,
                    lon = model.lon,
                    ProviderId = model.ProviderId,
                    StatusStore = (int)TypeStatusStore.Open,
                    StoreAddress = model.StoreAddress,
                    StoreName = model.StoreName,
                };
                // init user of store
                var hashPass = Helpers.SercurityHelper.GenerateSaltedHash(model.Password);
                User user = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    UserName = model.Email,
                    HashPassword = hashPass.Hash,
                    SaltPassword = hashPass.Salt,
                    StoreId = store.Id,
                    IsActived = (int)TypeVerified.Verified
                };
                store.CreateByUserId = user.Id;
                store.Users.Add(user);
                // save store to database

                try
                {
                    await _context.Stores.AddAsync(store);
                    await _context.SaveChangesAsync();
                    messager = $"Khởi tạo cửa hàng {store.StoreName} thành công, vui lòng check email và đăng nhập để quản trị.";
                }
                catch (Exception ex)
                {
                    _logger.Log(LogLevel.Warning, $"Log when RegistorStore {ex.Message}");
#if DEBUG
                    throw new ApiException(ex);
#else
                    throw new ApiException($"Có lỗi xảy ra khi thêm mới store {store.StoreName}");
#endif
                }
            }

            _logger.Log(LogLevel.Information, messager);
            return Ok(new { data = messager });
        }

        [HttpGet("{storeId}/product/type1"), MapToApiVersion("1")]
        [AllowAnonymous]
        public async Task<IActionResult> GetListProductsOfStoreForCustomer(
            string storeId = "b4d32aca-665d-4253-83a5-6f6a8b7acade",
            int pageIndex = 0,
            int pageSize = 10,
            string sortColumn = null,
            string sortOrder = null,
            string filterColumn = null,
            string filterQuery = null)
        {
            var query = _context.ProductSKUs
                .Where(p => p.Product.StoreId == storeId)
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

        [HttpGet("{storeId}/{categoryIdOrCode}/product/type1"), MapToApiVersion("1")]
        [AllowAnonymous]
        public async Task<IActionResult> GetListProductsOfStoreWithCategoryForCustomer(
            string storeId = "b4d32aca-665d-4253-83a5-6f6a8b7acade",
            string categoryIdOrCode = "ea76e2a8-873f-48bf-8ace-1415ee3758e8",
            int pageIndex = 0,
            int pageSize = 10,
            string sortColumn = null,
            string sortOrder = null,
            string filterColumn = null,
            string filterQuery = null)
        {
            var cat = await _context.CategoryProducts.FirstOrDefaultAsync(c => c.Id == categoryIdOrCode || c.Code == categoryIdOrCode);
            if (cat == null)
            {
                throw new ApiException("Category not found", (int)HttpStatusCode.BadRequest);
            }
            var query = _context.ProductSKUs
                .Where(p => p.Product.StoreId == storeId && p.Product.CategoryId == cat.Id)
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

        [HttpGet("{storeId}/product/type2"), MapToApiVersion("1")]
        [AllowAnonymous]
        public async Task<IActionResult> GetListProductsOfStoreForCustomerV2(
            string storeId = "b4d32aca-665d-4253-83a5-6f6a8b7acade",
            int pageIndex = 0,
            int pageSize = 10,
            string sortColumn = null,
            string sortOrder = null,
            string filterColumn = null,
            string filterQuery = null)
        {
            await CheckIsSignoutedAsync();

            var query = _context.Products
                .Where(p => p.StoreId == storeId)
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

        [HttpGet("{storeId}/{categoryIdOrCode}/product/type2"), MapToApiVersion("1")]
        [AllowAnonymous]
        public async Task<IActionResult> GetListProductsOfStoreWithCategoryForCustomerV2(
            string storeId = "b4d32aca-665d-4253-83a5-6f6a8b7acade",
            string categoryIdOrCode = "ea76e2a8-873f-48bf-8ace-1415ee3758e8",
            int pageIndex = 0,
            int pageSize = 10,
            string sortColumn = null,
            string sortOrder = null,
            string filterColumn = null,
            string filterQuery = null)
        {
            var cat = await _context.CategoryProducts.FirstOrDefaultAsync(c => c.Id == categoryIdOrCode || c.Code == categoryIdOrCode);
            if (cat == null)
            {
                throw new ApiException("Category not found", (int)HttpStatusCode.BadRequest);
            }
            var query = _context.Products
                .Where(p => p.StoreId == storeId && p.CategoryId == cat.Id)
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

    }
}