using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StoreOrder.WebApplication.Authorization;
using StoreOrder.WebApplication.Controllers.ApiBase;
using StoreOrder.WebApplication.Data;
using StoreOrder.WebApplication.Data.DTO;
using StoreOrder.WebApplication.Data.Enums;
using StoreOrder.WebApplication.Data.Models.Orders;
using StoreOrder.WebApplication.Data.Models.Products;
using StoreOrder.WebApplication.Data.Models.Stores;
using StoreOrder.WebApplication.Data.Orders;
using StoreOrder.WebApplication.Data.Repositories.Interfaces;
using StoreOrder.WebApplication.Data.Wrappers;
using System;
using System.Collections.Generic;
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

            var user = await _authRepository.GetUserByUserNameOrEmailAsync(model.UserNameOrEmail);
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

            var user = await _authRepository.GetUserByUserNameOrEmailAsync(model.UserNameOrEmail);
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

        [HttpGet("tables"), MapToApiVersion("1")]
        [Authorize(Policy = Permissions.Employee.GetListTables)]
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
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == this.CurrentUserId);
            if (user == null)
            {
                _logger.LogInformation("User not found");
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
        [Authorize(Policy = Permissions.Employee.GetCategoryProductsForEmployee)]
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
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == this.CurrentUserId);
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
        [Authorize(Policy = Permissions.Employee.GetListProductsOfStoreForEmployee)]
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
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == this.CurrentUserId);
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
                    FixedPrice = p.Product.FixedPrice,
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
        [Authorize(Policy = Permissions.Employee.GetListProductsOfStoreWithCategoryForEmployee)]
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
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == this.CurrentUserId);
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
                    FixedPrice = p.Product.FixedPrice,
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
        [Authorize(Policy = Permissions.Employee.GetListProductsOfStoreForEmployeeV2)]
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
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == this.CurrentUserId);
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
                ImageThumb = p.ProductDetail?.ImageThumb,
                ImageWidthThumb = p.ProductDetail?.ImageWidthThumb,
                LongDescription = p.ProductDetail?.LongDescription,
                ShortDescription = p.ProductDetail?.ShortDescription,
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
        [Authorize(Policy = Permissions.Employee.GetListProductsOfStoreWithCategoryForEmployeeV2)]
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
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == this.CurrentUserId);
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
                ImageHeightThumb = p.ProductDetail?.ImageHeightThumb,
                ImageThumb = p.ProductDetail?.ImageThumb,
                ImageWidthThumb = p.ProductDetail?.ImageWidthThumb,
                LongDescription = p.ProductDetail?.LongDescription,
                ShortDescription = p.ProductDetail?.ShortDescription,
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
        [Authorize(Policy = Permissions.Employee.CreateOrder)]
        public async Task<IActionResult> CreateOrder([FromBody] OrderProductDTO model)
        {
            await CheckIsSignoutedAsync();
            int message = 0;
            Order orderCreate = null;
            if (ModelState.IsValid)
            {
                if (model.Products.Count == 0)
                {
                    throw new ApiException("Cần có ít nhất 1 sản phẩm trong đơn hàng.", (int)HttpStatusCode.BadRequest);
                }

                // check if table busy
                var hasTableBusying = await _context.StoreTables.Where(t => t.Id == model.TableId && t.StoreId == this.UserStoreId && t.TableStatus == (int)TypeTableStatus.Busying).FirstOrDefaultAsync();
                if (hasTableBusying != null)
                {
                    throw new ApiException("Bàn này đang bận không thể tạo mới order!", (int)HttpStatusCode.BadRequest);
                }

                // check order is create or update order
                if (string.Equals(model.OrderId, "0"))
                {
                    // create order
                    Order newOrder = new Order
                    {
                        Id = Guid.NewGuid().ToString(),
                        OrderStatus = model.OrderStatus,
                        TableId = model.TableId,
                        TableName = model.TableName,
                        UserId = this.CurrentUserId,
                        CreatedOn = DateTime.UtcNow
                    };

                    // add Order Detail
                    foreach (var order in model.Products)
                    {
                        newOrder.OrderDetails.Add(new Data.Orders.OrderDetail
                        {
                            Id = Guid.NewGuid().ToString(),
                            ProductId = order.ProductId,
                            ProductName = order.ProductName,
                            OptionDescription = order.OptionDescription,
                            OptionId_OptionValueIds = order.OptionId_OptionValueIds,
                            Note = order.Note,
                            Amount = order.Amount,
                            Status = order.Status,
                            Price = order.Price
                        });
                    }

                    // Add or to Database.Orders
                    _context.Orders.Add(newOrder);
                    // save to db
                    try
                    {
                        await _context.SaveChangesAsync();
                        message = 1;
                        orderCreate = newOrder;
                        // update table to state busying
                        await UpdateTableStatus(model.TableId, (int)TypeTableStatus.Busying);

                    }
                    catch (DbUpdateException ex)
                    {
#if !DEBUG
                        throw new ApiException("An error occurred creating a order.");
#else
                        _logger.LogError("Update db---", ex);
                        throw new ApiException("Error Update Exceptions!", (int)HttpStatusCode.BadRequest);
#endif
                    }

                }
                else
                {
                    throw new ApiException("Api not supports", (int)HttpStatusCode.BadRequest);
                }
            }

            return Ok(orderCreate);
        }

        [HttpPost("order"), MapToApiVersion("2")]
        [Authorize(Policy = Permissions.Employee.CreateOrder)]
        public async Task<IActionResult> CreateOrderV2([FromForm] OrderProductDTO model)
        {
            await CheckIsSignoutedAsync();
            int message = 0;
            Order orderCreate = null;
            if (ModelState.IsValid)
            {
                if (model.Products.Count == 0)
                {
                    throw new ApiException("Cần có ít nhất 1 sản phẩm trong đơn hàng.", (int)HttpStatusCode.BadRequest);
                }

                // check if table busy
                var hasTableBusying = await _context.StoreTables.Where(t => t.Id == model.TableId && t.StoreId == this.UserStoreId && t.TableStatus == (int)TypeTableStatus.Busying).FirstOrDefaultAsync();
                if (hasTableBusying != null)
                {
                    throw new ApiException("Bàn này đang bận không thể tạo mới order!", (int)HttpStatusCode.BadRequest);
                }

                // check order is create or update order
                if (string.Equals(model.OrderId, "0"))
                {
                    // create order
                    Order newOrder = new Order
                    {
                        Id = Guid.NewGuid().ToString(),
                        OrderStatus = model.OrderStatus,
                        TableId = model.TableId,
                        TableName = model.TableName,
                        UserId = this.CurrentUserId,
                        CreatedOn = DateTime.UtcNow
                    };

                    // add Order Detail
                    foreach (var order in model.Products)
                    {
                        newOrder.OrderDetails.Add(new Data.Orders.OrderDetail
                        {
                            Id = Guid.NewGuid().ToString(),
                            ProductId = order.ProductId,
                            ProductName = order.ProductName,
                            OptionDescription = order.OptionDescription,
                            OptionId_OptionValueIds = order.OptionId_OptionValueIds,
                            Note = order.Note,
                            Amount = order.Amount,
                            Status = order.Status,
                            Price = order.Price
                        });
                    }

                    // Add or to Database.Orders
                    _context.Orders.Add(newOrder);
                    // save to db
                    try
                    {
                        await _context.SaveChangesAsync();
                        message = 1;
                        orderCreate = newOrder;
                        // update table to state busying
                        await UpdateTableStatus(model.TableId, (int)TypeTableStatus.Busying);

                    }
                    catch (DbUpdateException ex)
                    {
#if !DEBUG
                        throw new ApiException("An error occurred creating a order.");
#else
                        _logger.LogError("Update db---", ex);
                        throw new ApiException("Error Update Exceptions!", (int)HttpStatusCode.BadRequest);
#endif
                    }

                }
                else
                {
                    throw new ApiException("Api not supports", (int)HttpStatusCode.BadRequest);
                }
            }

            return Ok(orderCreate);
        }

        [HttpPut("order/{orderId}"), MapToApiVersion("1")]
        [Authorize(Policy = Permissions.Employee.UpdateOrder)]
        public async Task<IActionResult> UpdateOrder([FromBody] OrderProductDTO model, string orderId, bool isOrder = true)
        {
            await CheckIsSignoutedAsync();
            int message = 0;
            model.OrderId = orderId;
            if (ModelState.IsValid)
            {
                // check products null
                if (model.Products.Count == 0)
                {
                    throw new ApiException("Cần có ít nhất 1 sản phẩm trong đơn hàng.", (int)HttpStatusCode.BadRequest);
                }

                // check order is create or update order
                if (!string.Equals(model.OrderId, "0"))
                {
                    // find order
                    Order orderExist = _context.Orders.Include(o => o.OrderDetails).ThenInclude(o => o.Order).FirstOrDefault(o => o.Id == model.OrderId && o.UserId == this.CurrentUserId && o.TableId == model.TableId);
                    if (orderExist == null)
                    {
                        throw new ApiException("Order không tồn tại.", (int)HttpStatusCode.BadRequest);
                    }

                    // update order
                    _context.Entry(orderExist).State = EntityState.Modified;

                    orderExist.OrderStatus = model.OrderStatus;
                    orderExist.UpdateOn = DateTime.UtcNow;

                    if (!isOrder)
                    {
                        orderExist.UserCookingId = this.CurrentUserId;
                    }

                    // add Order Detail
                    foreach (var order in model.Products)
                    {
                        // find orderDetail
                        // if not exist then create, other update order detail
                        OrderDetail orderDetail = await _context.OrderDetails.FindAsync(order.OrderDetailId);

                        if (orderDetail == null)
                        {
                            orderExist.OrderDetails.Add(new Data.Orders.OrderDetail
                            {
                                Id = Guid.NewGuid().ToString(),
                                ProductId = order.ProductId,
                                ProductName = order.ProductName,
                                OptionDescription = order.OptionDescription,
                                OptionId_OptionValueIds = order.OptionId_OptionValueIds,
                                Note = order.Note,
                                Amount = order.Amount,
                                Status = order.Status,
                                Price = order.Price
                            });
                        }
                        else
                        {
                            _context.Entry(orderDetail).State = EntityState.Modified;

                            if (isOrder)
                            {
                                orderDetail.Note = order.Note;
                                orderDetail.Amount = order.Amount;
                                orderDetail.Status = order.Status;
                                orderDetail.Price = order.Price;
                            }
                            else
                            {
                                orderDetail.Status = order.Status;
                            }
                        }
                    }

                    // save to db
                    try
                    {
                        await _context.SaveChangesAsync();
                        message = 1;
                    }
                    catch (DbUpdateException ex)
                    {
#if !DEBUG
                        throw new ApiException("An error occurred creating a order.");
#else
                        _logger.LogError("Update db---", ex);
                        throw new ApiException("Error Update Exceptions!", (int)HttpStatusCode.BadRequest);
#endif
                    }

                }
                else
                {
                    throw new ApiException("Api not supports", (int)HttpStatusCode.BadRequest);
                }
            }
            // get order
            return Ok(await GetOrders(model.TableId, this.CurrentUserId, this.UserStoreId));
        }

        [HttpPut("order/{orderId}"), MapToApiVersion("2")]
        [Authorize(Policy = Permissions.Employee.UpdateOrder)]
        public async Task<IActionResult> UpdateOrderV2([FromForm] OrderProductDTO model, string orderId, bool isOrder = true)
        {
            await CheckIsSignoutedAsync();
            int message = 0;
            model.OrderId = orderId;

            if (ModelState.IsValid)
            {
                // check order is create or update order
                if (!string.Equals(model.OrderId, "0"))
                {
                    // find order
                    Order orderExist = _context.Orders.Include(o => o.OrderDetails).ThenInclude(o => o.Order).FirstOrDefault(o => o.Id == model.OrderId && o.UserId == this.CurrentUserId && o.TableId == model.TableId);
                    if (orderExist == null)
                    {
                        throw new ApiException("Order không tồn tại.", (int)HttpStatusCode.BadRequest);
                    }

                    // update order
                    _context.Entry(orderExist).State = EntityState.Modified;

                    orderExist.OrderStatus = model.OrderStatus;
                    orderExist.UpdateOn = DateTime.UtcNow;

                    if (!isOrder)
                    {
                        orderExist.UserCookingId = this.CurrentUserId;
                    }

                    // add Order Detail
                    foreach (var order in model.Products)
                    {
                        // find orderDetail
                        // if not exist then create, other update order detail
                        OrderDetail orderDetail = await _context.OrderDetails.FindAsync(order.OrderDetailId);

                        if (orderDetail == null)
                        {
                            orderExist.OrderDetails.Add(new Data.Orders.OrderDetail
                            {
                                Id = Guid.NewGuid().ToString(),
                                ProductId = order.ProductId,
                                ProductName = order.ProductName,
                                OptionDescription = order.OptionDescription,
                                OptionId_OptionValueIds = order.OptionId_OptionValueIds,
                                Note = order.Note,
                                Amount = order.Amount,
                                Status = order.Status,
                                Price = order.Price
                            });
                        }
                        else
                        {
                            _context.Entry(orderDetail).State = EntityState.Modified;

                            if (isOrder)
                            {
                                orderDetail.Note = order.Note;
                                orderDetail.Amount = order.Amount;
                                orderDetail.Status = order.Status;
                                orderDetail.Price = order.Price;
                            }
                            else
                            {
                                orderDetail.Status = order.Status;
                            }
                        }
                    }

                    // save to db
                    try
                    {
                        await _context.SaveChangesAsync();
                        message = 1;
                    }
                    catch (DbUpdateException ex)
                    {
#if !DEBUG
                        throw new ApiException("An error occurred creating a order.");
#else
                        _logger.LogError("Update db---", ex);
                        throw new ApiException("Error Update Exceptions!", (int)HttpStatusCode.BadRequest);
#endif
                    }

                }
                else
                {
                    throw new ApiException("Api not supports", (int)HttpStatusCode.BadRequest);
                }
            }

            return Ok(await GetOrders(model.TableId, this.CurrentUserId, this.UserStoreId));
        }

        [HttpGet("order/table/{tableId}"), MapToApiVersion("1")]
        [Authorize(Policy = Permissions.Employee.GetOrderProductsWithTable)]
        public async Task<IActionResult> GetOrderProductsWithTable(string tableId)
        {
            await CheckIsSignoutedAsync();
            if (string.IsNullOrEmpty(tableId))
            {
                throw new ApiException("table id not found.", (int)HttpStatusCode.BadRequest);
            }
            return Ok(await GetOrders(tableId, this.UserStoreId));
        }

        [HttpGet("order/type1"), MapToApiVersion("1")]
        [Authorize(Policy = Permissions.Employee.GetListOrderWithTableOrProductName)]
        public async Task<IActionResult> GetListOrderWithProductName(
            int pageIndex = 0,
            int pageSize = 10,
            string sortColumn = null,
            string sortOrder = null,
            string filterColumn = null,
            string filterQuery = null
            )
        {
            await CheckIsSignoutedAsync();
            // xem theo món
            //var result = new List<OrderWithProductDTO>() {
            //    new OrderWithProductDTO { ProductId = "1", ProductName = "Trà chanh nhiệt đới", TotalCount = 5,
            //        Products = new List<ViewProductOptionDTO>() {
            //            new ViewProductOptionDTO { OptionId_OptionValueIds = new string[] {"1_1"}, OptionDescription = "Bình thường", TableId = "tableId1", TableName = "A1", Amount = 1, Note = "Không toping", Status = (int)TypeOrderDetail.NewOrder },
            //            new ViewProductOptionDTO { OptionId_OptionValueIds = new string[] {"2_1"}, OptionDescription = "Toping: Ít đá, nhiều đường", TableId = "tableId1", TableName = "A1", Amount = 1, Note = "Cốc nhựa", Status = (int)TypeOrderDetail.NewOrder },
            //            new ViewProductOptionDTO { OptionId_OptionValueIds = new string[] {"2_2"}, OptionDescription = "Toping nhiều đá, ít đường", TableId = "tableId1", TableName = "A1", Amount = 1, Note = "Cốc nhựa", Status = (int)TypeOrderDetail.NewOrder },
            //        }
            //    },
            //    new OrderWithProductDTO { ProductId = "2", ProductName = "Trà chanh nhiệt độ", TotalCount = 5,
            //        Products = new List<ViewProductOptionDTO>() {
            //            new ViewProductOptionDTO { OptionId_OptionValueIds = new string[] {"1_1"}, OptionDescription = "Bình thường", TableId = "tableId1", TableName = "A1", Amount = 1, Note = "Không toping", Status = (int)TypeOrderDetail.NewOrder },
            //            new ViewProductOptionDTO { OptionId_OptionValueIds = new string[] {"2_1"}, OptionDescription = "Toping: Ít đá, nhiều đường", TableId = "tableId1", TableName = "A1", Amount = 1, Note = "Cốc nhựa", Status = (int)TypeOrderDetail.NewOrder },
            //            new ViewProductOptionDTO { OptionId_OptionValueIds = new string[] {"2_2"}, OptionDescription = "Toping nhiều đá, ít đường", TableId = "tableId1", TableName = "A1", Amount = 1, Note = "Cốc nhựa", Status = (int)TypeOrderDetail.NewOrder },
            //        }
            //    },
            //    new OrderWithProductDTO { ProductId = "3", ProductName = "Cà phê", TotalCount = 5,
            //        Products = new List<ViewProductOptionDTO>() {
            //            new ViewProductOptionDTO { OptionId_OptionValueIds = new string[] {"1_1"}, OptionDescription = "Cà phê đen", TableId = "tableId1", TableName = "A1", Amount = 1, Note = "Mang về", Status = (int)TypeOrderDetail.NewOrder },
            //            new ViewProductOptionDTO { OptionId_OptionValueIds = new string[] {"2_1"}, OptionDescription = "Bạc sửu", TableId = "tableId1", TableName = "A1", Amount = 1, Note = "Cốc trắng", Status = (int)TypeOrderDetail.NewOrder },
            //            new ViewProductOptionDTO { OptionId_OptionValueIds = new string[] {"2_2"}, OptionDescription = "Cà phê sữa", TableId = "tableId1", TableName = "A1", Amount = 1, Note = "Cốc đá", Status = (int)TypeOrderDetail.NewOrder },
            //        }
            //    }
            //};

            var tableOfStores = await _context.StoreTables.Where(x => x.StoreId == this.UserStoreId).Select(x => x.Id).ToListAsync();
            if (tableOfStores.Count == 0)
            {
                throw new ApiException("Cửa hàng chưa có bàn.", (int)HttpStatusCode.BadRequest);
            }

            var result = await ApiResult<OrderDetail>.CreateAsync(
                _context.OrderDetails
                    .Include(o => o.Order)
                    .Where(o => tableOfStores.Contains(o.Order.TableId) && o.Order.OrderStatus != (int)TypeOrderStatus.Done && o.Order.Id == o.OrderId),
                pageIndex,
                pageSize,
                sortColumn,
                sortOrder,
                filterColumn,
                filterQuery
                );

            var orderDetails = result.Data.GroupBy(o => new { o.ProductId, o.ProductName })
            .Select(o => new OrderWithProductDTO
            {
                ProductId = o.Key.ProductId,
                ProductName = o.Key.ProductName,
                Products = o.Select(o => new ViewProductOptionDTO
                {
                    TableId = o.Order.TableId,
                    TableName = o.Order.TableName,
                    Note = o.Note,
                    OptionDescription = o.OptionDescription,
                    Amount = o.Amount.Value,
                    Status = o.Status.Value,
                    OptionId_OptionValueIds = o.OptionId_OptionValueIds,
                    OrderDetailId = o.Id
                }).ToList()
            }).ToList();

            return Ok(new
            {
                Data = orderDetails,
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

        [HttpGet("order/type2"), MapToApiVersion("1")]
        [Authorize(Policy = Permissions.Employee.GetListOrderWithTableOrProductName)]
        public async Task<IActionResult> GetListOrderWithTable(
            int pageIndex = 0,
            int pageSize = 10,
            string sortColumn = null,
            string sortOrder = null,
            string filterColumn = null,
            string filterQuery = null
            )
        {
            await CheckIsSignoutedAsync();

            var tableOfStores = await _context.StoreTables.Where(x => x.StoreId == this.UserStoreId).Select(x => x.Id).ToListAsync();
            if (tableOfStores.Count == 0)
            {
                throw new ApiException("Cửa hàng chưa có bàn.", (int)HttpStatusCode.BadRequest);
            }

            var result = await ApiResult<Order>.CreateAsync(
                _context.Orders
                    .Include(o => o.StoreTable)
                    .Include(o => o.OrderDetails)
                    .Where(o => tableOfStores.Contains(o.TableId) && o.OrderStatus != (int)TypeOrderStatus.Done),
                pageIndex,
                pageSize,
                sortColumn,
                sortOrder,
                filterColumn,
                filterQuery
                );

            var tables = result.Data
            .Select(o => new OrderWithTableDTO
            {
                TableId = o.TableId,
                TableName = o.TableName,
                TableStatus = o.StoreTable.TableStatus,
                TotalFoods = o.OrderDetails.Count
            }).ToList();

            return Ok(new
            {
                Data = tables,
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

        [HttpPost("order/update/product-status"), MapToApiVersion("1")]
        [Authorize(Policy = Permissions.Employee.UpdateOrderProductStatus)]
        public async Task<IActionResult> UpdateProductStatus([FromBody] List<ViewProductOptionDTO> listProducts)
        {
            await CheckIsSignoutedAsync();
            int message = 0;
            if (ModelState.IsValid)
            {
                foreach (var model in listProducts)
                {
                    var orderDetail = await _context.OrderDetails.FindAsync(model.OrderDetailId);

                    if (orderDetail == null)
                    {
                        throw new ApiException("Không tìm thấy chi tiết order", (int)HttpStatusCode.BadRequest);
                    }

                    _context.Entry(orderDetail).State = EntityState.Modified;

                    orderDetail.OptionDescription = model.OptionDescription;
                    orderDetail.Note = model.Note;
                    orderDetail.Amount = model.Amount;
                    orderDetail.Status = model.Status;

                    // save to db
                    try
                    {
                        await _context.SaveChangesAsync();
                        message = 1;

                        // update user received order
                        await UpdateUserReceivedOrder(orderDetail.OrderId);

                    }
                    catch (DbUpdateException ex)
                    {
#if !DEBUG
                        throw new ApiException("An error occurred creating a order.");
#else
                        _logger.LogError("Update db---", ex);
                        throw new ApiException("Error Update Exceptions!", (int)HttpStatusCode.BadRequest);
#endif
                    }
                }
            }

            return Ok(message);
        }

        [HttpGet("order/{orderId}/confirm"), MapToApiVersion("1")]
        [Authorize(Policy = Permissions.Employee.ConfirmOrder)]
        public async Task<IActionResult> GetConfirmOrder(string orderId)
        {
            await CheckIsSignoutedAsync();

            if (string.IsNullOrEmpty(orderId))
            {
                throw new ApiException("Order Id is Required", (int)HttpStatusCode.BadRequest);
            }

            var tableOfStores = await _context.StoreTables.Where(x => x.StoreId == this.UserStoreId).Select(x => x.Id).ToListAsync();
            if (tableOfStores.Count == 0)
            {
                throw new ApiException("Cửa hàng chưa có bàn.", (int)HttpStatusCode.BadRequest);
            }

            // find Order by id
            var order = await _context.Orders
                .Include(o => o.OrderDetails).Where(x => x.Id == orderId && x.OrderStatus != (int)TypeOrderStatus.Done && tableOfStores.Contains(x.TableId))
                .FirstOrDefaultAsync();

            if (order == null)
            {
                throw new ApiException("Order không tìm thấy hoặc order đã kết thúc.", (int)HttpStatusCode.BadRequest);
            }

            var result = new OrderProductDTO
            {
                OrderId = order.Id,
                OrderStatus = order.OrderStatus.Value,
                TableId = order.TableId,
                TableName = order.TableName,
                Products = order.OrderDetails.Select(odt => new OrderDetailDTO
                {
                    OrderDetailId = odt.Id,
                    ProductId = odt.ProductId,
                    ProductName = odt.ProductName,
                    Amount = odt.Amount,
                    Note = odt.Note,
                    OptionDescription = odt.OptionDescription,
                    OptionId_OptionValueIds = odt.OptionId_OptionValueIds,
                    Price = odt.Price,
                    Status = odt.Status,
                }).ToList()
            };
            return Ok(result);
        }

        [HttpPost("order/{orderId}/confirm"), MapToApiVersion("1")]
        [Authorize(Policy = Permissions.Employee.ConfirmOrder)]
        public async Task<IActionResult> PostConfirmOrder(string orderId)
        {
            await CheckIsSignoutedAsync();
            int message = 0;
            if (string.IsNullOrEmpty(orderId))
            {
                throw new ApiException("Order Id is Required", (int)HttpStatusCode.BadRequest);
            }

            var tableOfStores = await _context.StoreTables.Where(x => x.StoreId == this.UserStoreId).Select(x => x.Id).ToListAsync();
            if (tableOfStores.Count == 0)
            {
                throw new ApiException("Cửa hàng chưa có bàn.", (int)HttpStatusCode.BadRequest);
            }

            // find Order by id
            var order = await _context.Orders
                .Include(o => o.OrderDetails).Where(x => x.Id == orderId && x.OrderStatus != (int)TypeOrderStatus.Done && tableOfStores.Contains(x.TableId))
                .FirstOrDefaultAsync();

            if (order == null)
            {
                throw new ApiException("Order không tìm thấy hoặc order đã kết thúc.", (int)HttpStatusCode.BadRequest);
            }

            _context.Entry(order).State = EntityState.Modified;
            order.OrderStatus = (int)TypeOrderStatus.Confirmed;
            order.UpdateOn = DateTime.UtcNow;

            // save
            try
            {
                await _context.SaveChangesAsync();
                message = 1;
            }
            catch (DbUpdateException ex)
            {
#if !DEBUG
                throw new ApiException("An error occurred confirm a order.");
#else
                _logger.LogError("Update db---", ex);
                throw new ApiException("Error Update Exceptions!", (int)HttpStatusCode.BadRequest);
#endif
            }

            return Ok(message);
        }

        [HttpPost("order/{orderId}/confirm-pay"), MapToApiVersion("1")]
        [Authorize(Policy = Permissions.Employee.ConfirmPayOrder)]
        public async Task<IActionResult> ConfirmPayOrder([FromBody] OrderProductDTO model, string orderId)
        {
            await CheckIsSignoutedAsync();
            int message = 0;
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(orderId))
                {
                    throw new ApiException("Order Id is Required", (int)HttpStatusCode.BadRequest);
                }

                var tableOfStores = await _context.StoreTables.Where(x => x.StoreId == this.UserStoreId).Select(x => x.Id).ToListAsync();
                if (tableOfStores.Count == 0)
                {
                    throw new ApiException("Cửa hàng chưa có bàn.", (int)HttpStatusCode.BadRequest);
                }

                // find Order confirmed by OrderId
                var order = await _context.Orders
                    .Include(o => o.OrderDetails).Where(x => x.Id == orderId && x.OrderStatus == (int)TypeOrderStatus.Confirmed && tableOfStores.Contains(x.TableId))
                    .FirstOrDefaultAsync();

                if (order == null)
                {
                    throw new ApiException("Order không tìm thấy hoặc order chưa confirm.", (int)HttpStatusCode.BadRequest);
                }

                // update status order
                _context.Entry(order).State = EntityState.Modified;
                order.OrderStatus = (int)TypeOrderStatus.Done;
                order.UpdateOn = DateTime.UtcNow;

                // save
                try
                {
                    await _context.SaveChangesAsync();
                    message = 1;
                }
                catch (DbUpdateException ex)
                {
#if !DEBUG
                throw new ApiException("An error occurred confirm a order.");
#else
                    _logger.LogError("Update db---", ex);
                    throw new ApiException("Error Update Exceptions!", (int)HttpStatusCode.BadRequest);
#endif
                }

                // update table to state can oder
                await UpdateTableStatus(model.TableId, (int)TypeTableStatus.CanOrder);

            }
            return Ok(message);
        }

        private async Task<int> UpdateTableStatus(string tableId, int status)
        {
            int message = 0;
            var tableOrder = await _context.StoreTables.FirstOrDefaultAsync(t => t.Id == tableId && t.StoreId == this.UserStoreId && t.TableStatus != (int)TypeTableStatus.Busying);
            if (tableOrder != null)
            {
                // update table to state can oder
                _context.Entry(tableOrder).State = EntityState.Modified;
                tableOrder.TableStatus = status;
                // save
                try
                {
                    await _context.SaveChangesAsync();
                    message = 1;
                }
                catch (DbUpdateException ex)
                {
#if !DEBUG
                throw new ApiException("An error occurred confirm a order.");
#else
                    _logger.LogError("Update db---", ex);
                    throw new ApiException("Error Update Exceptions!", (int)HttpStatusCode.BadRequest);
#endif
                }
            }
            return message;
        }

        private async Task<OrderProductDTO> GetOrders(string tableId, string storeId)
        {
            var tableOfStore = await _context.StoreTables.FirstOrDefaultAsync(t => t.Id == tableId && t.StoreId == storeId);
            if (tableOfStore == null)
            {
                throw new ApiException("table not found.", (int)HttpStatusCode.BadRequest);
            }
            var result = await _context.Orders
                .Include(o => o.OrderDetails)
                .Where(x => x.TableId == tableOfStore.Id && x.OrderStatus != (int)TypeOrderStatus.Done)
                .OrderByDescending(x => x.CreatedOn)
                .Select(o => new OrderProductDTO
                {
                    OrderId = o.Id,
                    OrderStatus = o.OrderStatus.Value,
                    TableId = o.TableId,
                    TableName = o.TableName,
                    CreatedOn = o.CreatedOn,
                    UpdatedOn = o.UpdateOn,
                    Products = o.OrderDetails.Select(odt => new OrderDetailDTO
                    {
                        OrderDetailId = odt.Id,
                        ProductId = odt.ProductId,
                        ProductName = odt.ProductName,
                        Amount = odt.Amount,
                        Note = odt.Note,
                        OptionDescription = odt.OptionDescription,
                        OptionId_OptionValueIds = odt.OptionId_OptionValueIds,
                        Price = odt.Price,
                        Status = odt.Status,
                    }).ToList()
                }).FirstOrDefaultAsync();
            return result;
        }

        private async Task<int> UpdateUserReceivedOrder(string orderId)
        {
            int message = 0;
            var order = await _context.Orders.FindAsync(orderId);
            if (order != null)
            {
                if (string.IsNullOrEmpty(order.UserCookingId))
                {
                    // update table to state can oder
                    _context.Entry(order).State = EntityState.Modified;
                    order.UserCookingId = this.CurrentUserId;
                    // save
                    try
                    {
                        await _context.SaveChangesAsync();
                        message = 1;
                    }
                    catch (DbUpdateException ex)
                    {
#if !DEBUG
                throw new ApiException("An error occurred confirm a order.");
#else
                        _logger.LogError("Update db---", ex);
                        throw new ApiException("Error Update Exceptions!", (int)HttpStatusCode.BadRequest);
#endif
                    }
                }
            }
            return message;
        }
    }
}