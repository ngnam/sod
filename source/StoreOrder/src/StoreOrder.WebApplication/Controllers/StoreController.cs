using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class StoreController : ApiBaseController
    {
        private readonly StoreOrderDbContext _context;
        private readonly IAuthRepository _authRepository;
        private readonly ILogger<StoreController> _logger;
        public StoreController(
            StoreOrderDbContext context,
            IAuthRepository authRepository,
            ILogger<StoreController> logger)
        {
            _logger = logger;
            _context = context;
            _authRepository = authRepository;
        }

        [HttpGet("employee/product/category"), MapToApiVersion("1")]
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
                        Code =c.Code,
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

        [HttpGet("customer/{storeId}/product/category"), MapToApiVersion("1")]
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

        [HttpGet("employee/product"), MapToApiVersion("1")]
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

        [HttpGet("customer/{storeId}/product"), MapToApiVersion("1")]
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
                
        [HttpGet("employee/{categoryId}/product"), MapToApiVersion("1")]
        public async Task<IActionResult> GetListProductsOfStoreWithCategoryForEmployee(
            string categoryId = "ea76e2a8-873f-48bf-8ace-1415ee3758e8",
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
                .Where(p => p.Product.StoreId == user.StoreId && p.Product.CategoryId == categoryId)
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

        [HttpGet("customer/{storeId}/{categoryId}/product"), MapToApiVersion("1")]
        [AllowAnonymous]
        public async Task<IActionResult> GetListProductsOfStoreWithCategoryForCustomer(
            string storeId = "b4d32aca-665d-4253-83a5-6f6a8b7acade",
            string categoryId = "ea76e2a8-873f-48bf-8ace-1415ee3758e8",
            int pageIndex = 0,
            int pageSize = 10,
            string sortColumn = null,
            string sortOrder = null,
            string filterColumn = null,
            string filterQuery = null)
        {
            var query = _context.ProductSKUs
                .Where(p => p.Product.StoreId == storeId && p.Product.CategoryId == categoryId)
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

        [HttpGet("employee/product/type2"), MapToApiVersion("1")]
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

        [HttpGet("customer/{storeId}/product/type2"), MapToApiVersion("1")]
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

        [HttpGet("employee/{categoryId}/product/type2"), MapToApiVersion("1")]
        public async Task<IActionResult> GetListProductsOfStoreWithCategoryForEmployeeV2(
            string categoryId = "ea76e2a8-873f-48bf-8ace-1415ee3758e8",
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
                .Where(p => p.StoreId == user.StoreId && p.CategoryId == categoryId)
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

        [HttpGet("customer/{storeId}/{categoryId}/product/type2"), MapToApiVersion("1")]
        [AllowAnonymous]
        public async Task<IActionResult> GetListProductsOfStoreWithCategoryForCustomerV2(
            string storeId = "b4d32aca-665d-4253-83a5-6f6a8b7acade",
            string categoryId = "ea76e2a8-873f-48bf-8ace-1415ee3758e8",
            int pageIndex = 0,
            int pageSize = 10,
            string sortColumn = null,
            string sortOrder = null,
            string filterColumn = null,
            string filterQuery = null)
        {
            var query = _context.Products
                .Where(p => p.StoreId == storeId && p.CategoryId == categoryId)
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