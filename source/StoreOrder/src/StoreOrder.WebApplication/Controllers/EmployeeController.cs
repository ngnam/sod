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
using StoreOrder.WebApplication.Helpers;
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
        [Authorize(Policy = Permissions.Employee.CreateOrder)]
        public async Task<IActionResult> CreateOrder([FromBody] OrderProductDTO model)
        {
            await CheckIsSignoutedAsync();
            string message = string.Empty;
            if (ModelState.IsValid)
            {
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
                    };
                    if (model.CreatedOn.HasValue)
                    {
                        newOrder.CreatedOn = model.CreatedOn.Value.FromDateTimeOffset();
                    }
                    if (model.UpdatedOn.HasValue)
                    {
                        newOrder.UpdateOn = model.UpdatedOn.Value.FromDateTimeOffset();
                    }

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
                        message = "Tạo mới order thành công!";
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

            return Ok(message);
        }

        [HttpPost("order"), MapToApiVersion("2")]
        [Authorize(Policy = Permissions.Employee.CreateOrder)]
        public async Task<IActionResult> CreateOrderV2([FromForm] OrderProductDTO model)
        {
            await CheckIsSignoutedAsync();
            string message = string.Empty;
            if (ModelState.IsValid)
            {
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
                        message = "Tạo mới order thành công!";
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

            return Ok(message);
        }

        [HttpPut("order/{orderId}"), MapToApiVersion("1")]
        [Authorize(Policy = Permissions.Employee.UpdateOrder)]
        public async Task<IActionResult> UpdateOrder([FromBody] OrderProductDTO model, string orderId, bool isOrder=true)
        {
            await CheckIsSignoutedAsync();
            string message = string.Empty;
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
                        } else
                        {
                            _context.Entry(orderDetail).State = EntityState.Modified;

                            if (isOrder)
                            {
                                orderDetail.Note = order.Note;
                                orderDetail.Amount = order.Amount;
                                orderDetail.Status = order.Status;
                                orderDetail.Price = order.Price;
                            } else
                            {
                                orderDetail.Status = order.Status;
                            }
                        }
                    }

                    // save to db
                    try
                    {
                        await _context.SaveChangesAsync();
                        message = "Cập nhật order thành công!";
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

            return Ok(message);
        }

        [HttpPut("order/{orderId}"), MapToApiVersion("2")]
        [Authorize(Policy = Permissions.Employee.UpdateOrder)]
        public async Task<IActionResult> UpdateOrderV2([FromForm] OrderProductDTO model, string orderId)
        {
            await CheckIsSignoutedAsync();
            string message = string.Empty;
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
                            orderDetail.Note = order.Note;
                            orderDetail.Amount = order.Amount;
                            orderDetail.Status = order.Status;
                            orderDetail.Price = order.Price;
                        }
                    }

                    // save to db
                    try
                    {
                        await _context.SaveChangesAsync();
                        message = "Cập nhật order thành công!";
                    }
                    catch (DbUpdateException ex)
                    {
#if !DEBUG
                        throw new ApiException("An error occurred creating a order.");
#else
                        _logger.LogError("Update db Exception---", ex);
                        throw new ApiException("Error Update Exceptions!", (int)HttpStatusCode.BadRequest);
#endif
                    }

                }
                else
                {
                    throw new ApiException("Api not supports", (int)HttpStatusCode.BadRequest);
                }
            }

            return Ok(message);
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
            var tableOfStore = await _context.StoreTables.FirstOrDefaultAsync(t => t.Id == tableId && t.StoreId == this.UserStoreId);
            if (tableOfStore == null)
            {
                throw new ApiException("table not found.", (int)HttpStatusCode.BadRequest);
            }
            var result = await _context.Orders
                .Include(o => o.OrderDetails)
                .Where(x => x.UserId == this.CurrentUserId && x.TableId == tableId && x.OrderStatus != (int)TypeOrderStatus.Done)
                .Select(o => new OrderProductDTO {
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

            //var result = new OrderProductDTO
            //{
            //    OrderId = Guid.NewGuid().ToString(),
            //    OrderStatus = (int)TypeOrderStatus.NewOrder,
            //    TableId = Guid.NewGuid().ToString(),
            //    TableName = "Bàn 01",
            //    Products = new System.Collections.Generic.List<OrderDetailDTO>()
            //    {
            //        new OrderDetailDTO { ProductId = "1", ProductName = "Trà chanh nhiệt đới", OptionId_OptionValueIds = new string[] {"1_1", "2_1_2_3"}, OptionDescription = "Kích cỡ nhỏ, Toping: Dừa khô, trân châu, chuối khô", Note = "Cốc nhựa", Amount = 1, Status = (int)TypeOrderDetail.NewOrder, Price = 50000  },
            //        new OrderDetailDTO { ProductId = "1", ProductName = "Trà chanh nhiệt đới", OptionId_OptionValueIds = new string[] {"2_1_2"}, OptionDescription = "Toping 80 đá, 20 đường", Note = "Cốc nhựa", Amount = 1, Status = (int)TypeOrderDetail.NewOrder, Price = 50000  },
            //        new OrderDetailDTO { ProductId = "1", ProductName = "Trà chanh nhiệt đới", OptionId_OptionValueIds = new string[] {"2_4_5"}, OptionDescription = "Toping ít đá, nhiều đường", Note = "Cốc thủy tinh, thêm đường", Amount = 1, Status = (int)TypeOrderDetail.NewOrder, Price = 50000  },
            //        new OrderDetailDTO { ProductId = "2", ProductName = "Bạc Sửu", OptionId_OptionValueIds = new string[] {"3_4"}, OptionDescription = "Bạc sửu", Note = "Ít đá", Amount = 1, Status = (int)TypeOrderDetail.NewOrder, Price = 50000  },
            //        new OrderDetailDTO { ProductId = "2", ProductName = "Caffe đen", OptionId_OptionValueIds = new string[] {"3_4"}, OptionDescription = "Caffe đen", Note = "Không đường", Amount = 1, Status = (int)TypeOrderDetail.NewOrder, Price = 50000  },
            //    }
            //};

            return Ok(result);
        }

        [HttpGet("order/{type}"), MapToApiVersion("1")]
        [Authorize(Policy = Permissions.Employee.GetListOrderWithTableOrProductName)]
        public async Task<IActionResult> GetListOrderWithTableOrProductName(int type = 1)
        {
            await CheckIsSignoutedAsync();
            if (type == 1)
            {
                // xem theo món
                var result = new List<OrderWithProductDTO>() {
                    new OrderWithProductDTO { ProductId = "1", ProductName = "Trà chanh nhiệt đới", TotalCount = 5,
                        Products = new List<ViewProductOptionDTO>() {
                            new ViewProductOptionDTO { OptionId_OptionValueIds = new string[] {"1_1"}, OptionDescription = "Bình thường", TableId = "tableId1", TableName = "A1", AmountFood = 1, OrderNote = "Không toping", ProductOrderStatus = (int)TypeOrderDetail.NewOrder },
                            new ViewProductOptionDTO { OptionId_OptionValueIds = new string[] {"2_1"}, OptionDescription = "Toping: Ít đá, nhiều đường", TableId = "tableId1", TableName = "A1", AmountFood = 1, OrderNote = "Cốc nhựa", ProductOrderStatus = (int)TypeOrderDetail.NewOrder },
                            new ViewProductOptionDTO { OptionId_OptionValueIds = new string[] {"2_2"}, OptionDescription = "Toping nhiều đá, ít đường", TableId = "tableId1", TableName = "A1", AmountFood = 1, OrderNote = "Cốc nhựa", ProductOrderStatus = (int)TypeOrderDetail.NewOrder },
                        }
                    },
                    new OrderWithProductDTO { ProductId = "2", ProductName = "Trà chanh nhiệt độ", TotalCount = 5,
                        Products = new List<ViewProductOptionDTO>() {
                            new ViewProductOptionDTO { OptionId_OptionValueIds = new string[] {"1_1"}, OptionDescription = "Bình thường", TableId = "tableId1", TableName = "A1", AmountFood = 1, OrderNote = "Không toping", ProductOrderStatus = (int)TypeOrderDetail.NewOrder },
                            new ViewProductOptionDTO { OptionId_OptionValueIds = new string[] {"2_1"}, OptionDescription = "Toping: Ít đá, nhiều đường", TableId = "tableId1", TableName = "A1", AmountFood = 1, OrderNote = "Cốc nhựa", ProductOrderStatus = (int)TypeOrderDetail.NewOrder },
                            new ViewProductOptionDTO { OptionId_OptionValueIds = new string[] {"2_2"}, OptionDescription = "Toping nhiều đá, ít đường", TableId = "tableId1", TableName = "A1", AmountFood = 1, OrderNote = "Cốc nhựa", ProductOrderStatus = (int)TypeOrderDetail.NewOrder },
                        }
                    },
                    new OrderWithProductDTO { ProductId = "3", ProductName = "Cà phê", TotalCount = 5,
                        Products = new List<ViewProductOptionDTO>() {
                            new ViewProductOptionDTO { OptionId_OptionValueIds = new string[] {"1_1"}, OptionDescription = "Cà phê đen", TableId = "tableId1", TableName = "A1", AmountFood = 1, OrderNote = "Mang về", ProductOrderStatus = (int)TypeOrderDetail.NewOrder },
                            new ViewProductOptionDTO { OptionId_OptionValueIds = new string[] {"2_1"}, OptionDescription = "Bạc sửu", TableId = "tableId1", TableName = "A1", AmountFood = 1, OrderNote = "Cốc trắng", ProductOrderStatus = (int)TypeOrderDetail.NewOrder },
                            new ViewProductOptionDTO { OptionId_OptionValueIds = new string[] {"2_2"}, OptionDescription = "Cà phê sữa", TableId = "tableId1", TableName = "A1", AmountFood = 1, OrderNote = "Cốc đá", ProductOrderStatus = (int)TypeOrderDetail.NewOrder },
                        }
                    }
                };
                return Ok(new { data = result });

            }
            else if (type == 2)
            {
                // xem theo bàn
                var result = new List<OrderWithTableDTO>() {
                    new OrderWithTableDTO { TableId = "1", TableName = "A1", TableStatus = (int)TypeTableStatus.TableStatus1, TotalFoods = 10, TimeOrder = 1 },
                    new OrderWithTableDTO { TableId = "2", TableName = "A2", TableStatus = (int)TypeTableStatus.TableStatus1, TotalFoods = 10, TimeOrder = 1 },
                    new OrderWithTableDTO { TableId = "3", TableName = "A3", TableStatus = (int)TypeTableStatus.TableStatus2, TotalFoods = 10, TimeOrder = 1 },
                    new OrderWithTableDTO { TableId = "4", TableName = "A4", TableStatus = (int)TypeTableStatus.TableStatus2, TotalFoods = 10, TimeOrder = 1 },
                    new OrderWithTableDTO { TableId = "5", TableName = "A5", TableStatus = (int)TypeTableStatus.TableStatus3, TotalFoods = 10, TimeOrder = 1 },
                    new OrderWithTableDTO { TableId = "6", TableName = "A6", TableStatus = (int)TypeTableStatus.TableStatus3, TotalFoods = 10, TimeOrder = 1 },
                    new OrderWithTableDTO { TableId = "7", TableName = "A7", TableStatus = (int)TypeTableStatus.TableStatus3, TotalFoods = 10, TimeOrder = 1 },
                    new OrderWithTableDTO { TableId = "8", TableName = "A8", TableStatus = (int)TypeTableStatus.TableStatus3, TotalFoods = 10, TimeOrder = 1 },
                    new OrderWithTableDTO { TableId = "9", TableName = "A9", TableStatus = (int)TypeTableStatus.TableStatus3, TotalFoods = 10, TimeOrder = 1 },
                    new OrderWithTableDTO { TableId = "10", TableName = "A10", TableStatus = (int)TypeTableStatus.TableStatus3, TotalFoods = 10, TimeOrder = 1 },
                    new OrderWithTableDTO { TableId = "11", TableName = "A11", TableStatus = (int)TypeTableStatus.TableStatus3, TotalFoods = 10, TimeOrder = 1 },
                };
                return Ok(new { data = result });
            }
            return Ok(new { data = new List<OrderWithTableDTO>() { } });
        }

        [HttpGet("order/2/table/{tableId}"), MapToApiVersion("1")]
        [Authorize(Policy = Permissions.Employee.GetOrderWithTableViewProductsWithTableId)]
        public async Task<IActionResult> GetOrderWithTableViewProductsWithTableId(string tableId = "1")
        {
            await CheckIsSignoutedAsync();

            var result = new OrderWithTableViewProductsDTO()
            {
                TableId = tableId,
                TableName = "A1",
                Products = new List<ViewProductDTO>()
                {
                     new ViewProductDTO { ProductId = "1", ProductName = "Trà chanh nhiệt đới", OptionId_OptionValueIds = new string[] {"1_1", "2_1_2_3"}, OptionDescription = "Kích cỡ nhỏ, Toping: Dừa khô, trân châu, chuối khô", OrderNote = "Cốc nhựa", AmountFood = 1, ProductOrderStatus = (int)TypeOrderDetail.NewOrder  },
                     new ViewProductDTO { ProductId = "1", ProductName = "Trà chanh nhiệt đới", OptionId_OptionValueIds = new string[] {"1_1", "2_1_2_3"}, OptionDescription = "Kích cỡ tỏ, Toping: Dừa khô, trân châu, chuối khô", OrderNote = "Cốc nhựa", AmountFood = 1, ProductOrderStatus = (int)TypeOrderDetail.NewOrder  },
                     new ViewProductDTO { ProductId = "2", ProductName = "Trà đào", OptionId_OptionValueIds = new string[] {"1_1", "2_1_2_3"}, OptionDescription = "Kích cỡ nhỏ, Toping: Dừa khô, trân châu, chuối khô", OrderNote = "Cốc nhựa", AmountFood = 1, ProductOrderStatus = (int)TypeOrderDetail.NewOrder  },
                     new ViewProductDTO { ProductId = "2", ProductName = "Trà đào", OptionId_OptionValueIds = new string[] {"1_1", "2_1_2_3"}, OptionDescription = "Kích cỡ to, Toping: Dừa khô, trân châu, chuối khô", OrderNote = "Cốc nhựa", AmountFood = 1, ProductOrderStatus = (int)TypeOrderDetail.NewOrder  },
                     new ViewProductDTO { ProductId = "3", ProductName = "Caffe", OptionId_OptionValueIds = new string[] {"1_1", "2_1_2_3"}, OptionDescription = "Cafe đen", OrderNote = "Cốc thủy tinh", AmountFood = 1, ProductOrderStatus = (int)TypeOrderDetail.NewOrder  },
                     new ViewProductDTO { ProductId = "3", ProductName = "Caffe", OptionId_OptionValueIds = new string[] {"1_1", "2_1_2_3"}, OptionDescription = "Bạc sửu", OrderNote = "Cốc thủy tinh", AmountFood = 1, ProductOrderStatus = (int)TypeOrderDetail.NewOrder  },
                     new ViewProductDTO { ProductId = "3", ProductName = "Caffe", OptionId_OptionValueIds = new string[] {"1_1", "2_1_2_3"}, OptionDescription = "Sữa nâu", OrderNote = "Cốc thủy tinh", AmountFood = 1, ProductOrderStatus = (int)TypeOrderDetail.NewOrder  },
                }
            };

            return Ok(result);
        }

        [HttpGet("order/{orderId}/confirm"), MapToApiVersion("1")]
        [Authorize(Policy = Permissions.Employee.ConfirmOrder)]
        public async Task<IActionResult> ConfirmOrder(string orderId = "1")
        {
            await CheckIsSignoutedAsync();
            var result = new OrderProductDTO
            {
                OrderId = Guid.NewGuid().ToString(),
                OrderStatus = (int)TypeOrderStatus.NewOrder,
                TableId = Guid.NewGuid().ToString(),
                TableName = "Bàn 01",
                Products = new System.Collections.Generic.List<OrderDetailDTO>()
                {
                    new OrderDetailDTO { ProductId = "1", ProductName = "Trà chanh nhiệt đới", OptionId_OptionValueIds = new string[] {"1_1", "2_1_2_3"}, OptionDescription = "Kích cỡ nhỏ, Toping: Dừa khô, trân châu, chuối khô", Note = "Cốc nhựa", Amount = 1, Status = (int)TypeOrderDetail.NewOrder, Price = 50000  },
                    new OrderDetailDTO { ProductId = "1", ProductName = "Trà chanh nhiệt đới", OptionId_OptionValueIds = new string[] {"2_1_2"}, OptionDescription = "Toping 80 đá, 20 đường", Note = "Cốc nhựa", Amount = 1, Status = (int)TypeOrderDetail.NewOrder, Price = 50000  },
                    new OrderDetailDTO { ProductId = "1", ProductName = "Trà chanh nhiệt đới", OptionId_OptionValueIds = new string[] {"2_4_5"}, OptionDescription = "Toping ít đá, nhiều đường", Note = "Cốc thủy tinh, thêm đường", Amount = 1, Status = (int)TypeOrderDetail.NewOrder, Price = 50000  },
                    new OrderDetailDTO { ProductId = "2", ProductName = "Bạc Sửu", OptionId_OptionValueIds = new string[] {"3_4"}, OptionDescription = "Bạc sửu", Note = "Ít đá", Amount = 1, Status = (int)TypeOrderDetail.Done, Price = 50000  },
                    new OrderDetailDTO { ProductId = "2", ProductName = "Caffe đen", OptionId_OptionValueIds = new string[] {"3_4"}, OptionDescription = "Caffe đen", Note = "Không đường", Amount = 1, Status = (int)TypeOrderDetail.Done, Price = 50000  },
                }
            };
            return Ok(result);
        }

        [HttpPost("order/{orderId}/confirm-pay"), MapToApiVersion("1")]
        [Authorize(Policy = Permissions.Employee.ConfirmPayOrder)]
        public async Task<IActionResult> ConfirmPayOrder([FromBody] OrderProductDTO model, string orderId = "1")
        {
            await CheckIsSignoutedAsync();
            if (ModelState.IsValid)
            {
                return Ok(1);
            }
            return Ok(0);
        }
    }
}