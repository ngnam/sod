﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StoreOrder.WebApplication.Authorization;
using StoreOrder.WebApplication.Data;
using StoreOrder.WebApplication.Data.Models.Account;
using StoreOrder.WebApplication.Data.Models.Products;
using StoreOrder.WebApplication.Data.Models.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            var databaseDbContext = services.GetRequiredService<StoreOrderDbContext>();
            var logger = services.GetRequiredService<ILogger<StoreOrderDbContext>>();

            // migration db
            databaseDbContext.Database.EnsureCreated();

            if (!databaseDbContext.Roles.Any())
            {
                string password = "string@1234";
                var hashPass = Helpers.SercurityHelper.GenerateSaltedHash(password);

                //// I will add 1 user to one Role
                //var book1 = new Book();
                //var book2 = new Book();
                var roleAdmin = new Role { Id = Guid.NewGuid().ToString(), Code = Guid.NewGuid().ToString(), RoleName = RoleTypeHelper.RoleAdmin, Desc = "Admin of Store" };
                var roleOrderUser = new Role { Id = Guid.NewGuid().ToString(), Code = Guid.NewGuid().ToString(), RoleName = RoleTypeHelper.RoleOrderUser, Desc = "NV Order of Store" };
                var roleCookieUser = new Role { Id = Guid.NewGuid().ToString(), Code = Guid.NewGuid().ToString(), RoleName = RoleTypeHelper.RoleCookieUser, Desc = "NV phụ bếp of Store" };
                var rolePayUser = new Role { Id = Guid.NewGuid().ToString(), Code = Guid.NewGuid().ToString(), RoleName = RoleTypeHelper.RolePayUser, Desc = "Nv Thanh toán of Store" };
                var roleCustomerUser = new Role { Id = Guid.NewGuid().ToString(), Code = Guid.NewGuid().ToString(), RoleName = RoleTypeHelper.RoleCustomerUser, Desc = "Role Account of Customer" };

                //// I create the User 
                //var lib = new Library();
                var user1 = new User { Id = Guid.NewGuid().ToString(), Age = 29, BirthDay = new DateTime(1991, 1, 1).ToUniversalTime(), CountLoginFailed = 0, FirstName = "admin", LastName = "admin", IsActived = 1, Email = "admin01@gmail.com", UserName = "admin01", HashPassword = hashPass.Hash, SaltPassword = hashPass.Salt, Gender = 1, OldPassword = hashPass.Hash + ";" + hashPass.Salt, PhoneNumber = "1234567" };
                var nv1 = new User { Id = Guid.NewGuid().ToString(), Age = 29, BirthDay = new DateTime(1991, 1, 1).ToUniversalTime(), CountLoginFailed = 0, FirstName = "nv1", LastName = "nv1", IsActived = 1, Email = "nv01@gmail.com", UserName = "nv01", HashPassword = hashPass.Hash, SaltPassword = hashPass.Salt, Gender = 1, OldPassword = hashPass.Hash + ";" + hashPass.Salt, PhoneNumber = "1234567" };
                var nv2 = new User { Id = Guid.NewGuid().ToString(), Age = 29, BirthDay = new DateTime(1991, 1, 1).ToUniversalTime(), CountLoginFailed = 0, FirstName = "nv2", LastName = "nv2", IsActived = 1, Email = "nv02@gmail.com", UserName = "nv02", HashPassword = hashPass.Hash, SaltPassword = hashPass.Salt, Gender = 1, OldPassword = hashPass.Hash + ";" + hashPass.Salt, PhoneNumber = "1234567" };
                var nv3 = new User { Id = Guid.NewGuid().ToString(), Age = 29, BirthDay = new DateTime(1991, 1, 1).ToUniversalTime(), CountLoginFailed = 0, FirstName = "nv3", LastName = "nv3", IsActived = 1, Email = "nv03@gmail.com", UserName = "nv03", HashPassword = hashPass.Hash, SaltPassword = hashPass.Salt, Gender = 1, OldPassword = hashPass.Hash + ";" + hashPass.Salt, PhoneNumber = "1234567" };

                //// I create two UserToRole which I need them 
                //// To map between the users and the role
                //var b2lib1 = new Library2Book();
                //var b2lib2 = new Library2Book();
                var userToRole1 = new UserToRole();
                var userToRole2 = new UserToRole();
                var userToRole3 = new UserToRole();
                var userToRole4 = new UserToRole();
                //// Mapping the first book to the library.
                //// Changed userToRole1.Role to userToRole2.Role
                //// Changed b2lib2.Library to b2lib1.Library
                //b2lib1.Book = book1;
                //b2lib1.Library = lib;
                userToRole1.Role = roleAdmin;
                userToRole1.User = user1;

                //// I map the second book to the library.
                //b2lib2.Book = book2;
                //b2lib2.Library = lib;
                userToRole2.Role = roleOrderUser;
                userToRole2.User = nv1;

                userToRole3.Role = roleCookieUser;
                userToRole3.User = nv2;

                userToRole4.Role = rolePayUser;
                userToRole4.User = nv3;

                //// Linking the books (Library2Book table) to the library
                //lib.Library2Books.Add(b2lib1);
                //lib.Library2Books.Add(b2lib2);
                roleAdmin.UserToRoles.Add(userToRole1);
                roleOrderUser.UserToRoles.Add(userToRole2);
                roleCookieUser.UserToRoles.Add(userToRole3);
                rolePayUser.UserToRoles.Add(userToRole4);

                //// Adding the data to the DbContext.
                //myDb.Libraries.Add(lib);
                databaseDbContext.Roles.Add(roleAdmin);
                databaseDbContext.Users.Add(user1);
                databaseDbContext.SaveChanges();

                databaseDbContext.Roles.Add(roleOrderUser);
                databaseDbContext.Users.Add(nv1);
                databaseDbContext.SaveChanges();

                databaseDbContext.Roles.Add(roleCookieUser);
                databaseDbContext.Users.Add(nv2);
                databaseDbContext.SaveChanges();

                databaseDbContext.Roles.Add(rolePayUser);
                databaseDbContext.Users.Add(nv3);
                databaseDbContext.SaveChanges();

                //myDb.Books.Add(book1);
                //myDb.Books.Add(book2);

                //// Save the changes and everything should be working!
                //myDb.SaveChanges();

                //await databaseDbContext.SaveChangesAsync();

                logger.LogInformation("DatabaseDbContext", "---initial data of Surveys success---");
            }

            if (databaseDbContext.Roles.Count() == 4)
            {
                var roleCustomerUser = new Role { Id = Guid.NewGuid().ToString(), Code = Guid.NewGuid().ToString(), RoleName = RoleTypeHelper.RoleCustomerUser, Desc = "Role Account of Customer" };
                await databaseDbContext.Roles.AddAsync(roleCustomerUser);
                await databaseDbContext.SaveChangesAsync();
            }

            if (databaseDbContext.Roles.Count() == 5)
            {
                var roleSysAdmin = new Role { Id = Guid.NewGuid().ToString(), Code = Guid.NewGuid().ToString(), RoleName = RoleTypeHelper.RoleSysAdmin, Desc = "Role SysAdmin" };
                await databaseDbContext.Roles.AddAsync(roleSysAdmin);
                await databaseDbContext.SaveChangesAsync();
            }

            if (!databaseDbContext.CategoryStores.Any())
            {
                var catStore = new CategoryStore { Id = Guid.NewGuid().ToString(), Name = "Nhà hàng" };
                await databaseDbContext.CategoryStores.AddAsync(catStore);
                await databaseDbContext.SaveChangesAsync();

                var useradmin = databaseDbContext.Users.FirstOrDefault(x => x.UserName.ToLower().Equals("admin01"));
                var store1 = new Store();
                var optionSize = new StoreOption();
                var optionToping = new StoreOption();
                if (useradmin != null)
                {
                    if (!databaseDbContext.Stores.Any())
                    {
                        store1 = new Store
                        {
                            Id = Guid.NewGuid().ToString(),
                            CategoryStoreId = catStore.Id,
                            StatusStore = 1,
                            StoreAddress = "Số 1, Xuân thủy, Cầu Giấy, Việt Nam",
                            StoreName = "Store 1",
                            CreateByUserId = useradmin.Id,
                        };

                        var lstStoreTable = new List<StoreTable>()
                        {
                             new StoreTable
                             {
                                Id = Guid.NewGuid().ToString(),
                                Location = 1,
                                LocationUnit = 1,
                                StoreId = store1.Id,
                                TableCode = Guid.NewGuid().ToString(),
                                TableName = "Bàn 1",
                                TableStatus = 1
                             },
                             new StoreTable
                             {
                                Id = Guid.NewGuid().ToString(),
                                Location = 1,
                                LocationUnit = 1,
                                StoreId = store1.Id,
                                TableCode = Guid.NewGuid().ToString(),
                                TableName = "Bàn 2",
                                TableStatus = 1
                             },
                             new StoreTable
                             {
                                Id = Guid.NewGuid().ToString(),
                                Location = 1,
                                LocationUnit = 1,
                                StoreId = store1.Id,
                                TableCode = Guid.NewGuid().ToString(),
                                TableName = "Bàn 3",
                                TableStatus = 1
                             },
                             new StoreTable
                             {
                                Id = Guid.NewGuid().ToString(),
                                Location = 1,
                                LocationUnit = 1,
                                StoreId = store1.Id,
                                TableCode = Guid.NewGuid().ToString(),
                                TableName = "Bàn 4",
                                TableStatus = 1
                             },
                             new StoreTable
                             {
                                Id = Guid.NewGuid().ToString(),
                                Location = 1,
                                LocationUnit = 1,
                                StoreId = store1.Id,
                                TableCode = Guid.NewGuid().ToString(),
                                TableName = "Bàn 5",
                                TableStatus = 1
                             }
                        };

                        optionSize = new StoreOption { OptionId = Guid.NewGuid().ToString(), StoreId = store1.Id, StoreOptionName = "size", StoreOptionDescription = "Kích thước" };
                        optionToping = new StoreOption { OptionId = Guid.NewGuid().ToString(), StoreId = store1.Id, StoreOptionName = "toping", StoreOptionDescription = "Toping" };

                        var lstStoreOption = new List<StoreOption>() {
                            optionSize,
                            optionToping,
                            new StoreOption { OptionId = Guid.NewGuid().ToString(), StoreId = store1.Id, StoreOptionName = "color", StoreOptionDescription = "Màu sắc" },
                            new StoreOption { OptionId = Guid.NewGuid().ToString(), StoreId = store1.Id, StoreOptionName = "class", StoreOptionDescription = "Lớp" },
                            new StoreOption { OptionId = Guid.NewGuid().ToString(), StoreId = store1.Id, StoreOptionName = "devo", StoreOptionDescription = "Dễ vỡ" },
                        };


                        store1.StoreTables = lstStoreTable;
                        store1.StoreOptions = lstStoreOption;

                        await databaseDbContext.Stores.AddAsync(store1);
                        await databaseDbContext.SaveChangesAsync();
                    }

                    if (!databaseDbContext.CategoryProducts.Any())
                    {
                        var catProduct = new CategoryProduct
                        {
                            Id = Guid.NewGuid().ToString(),
                            CategoryName = "Danh mục 1",
                            Slug = "danh-muc-1",
                            Code = "danhmuc1code",
                            ParentId = null,
                        };

                        var lstProducts = new List<Product>() {
                            new Product { Id = Guid.NewGuid().ToString(), CategoryId = catProduct.Id, ProductName = "Sản phẩm 1",   CreateByUserId = useradmin.Id, StoreId = store1.Id },
                            new Product { Id = Guid.NewGuid().ToString(), CategoryId = catProduct.Id, ProductName = "Sản phẩm 02",   CreateByUserId = useradmin.Id, StoreId = store1.Id },
                            new Product { Id = Guid.NewGuid().ToString(), CategoryId = catProduct.Id, ProductName = "Sản phẩm 03",   CreateByUserId = useradmin.Id, StoreId = store1.Id },
                            new Product { Id = Guid.NewGuid().ToString(), CategoryId = catProduct.Id, ProductName = "Sản phẩm 04",   CreateByUserId = useradmin.Id, StoreId = store1.Id },
                            new Product { Id = Guid.NewGuid().ToString(), CategoryId = catProduct.Id, ProductName = "Sản phẩm 05",   CreateByUserId = useradmin.Id, StoreId = store1.Id },
                            new Product { Id = Guid.NewGuid().ToString(), CategoryId = catProduct.Id, ProductName = "Sản phẩm 06",   CreateByUserId = useradmin.Id, StoreId = store1.Id },
                            new Product { Id = Guid.NewGuid().ToString(), CategoryId = catProduct.Id, ProductName = "Sản phẩm 07",   CreateByUserId = useradmin.Id, StoreId = store1.Id },
                            new Product { Id = Guid.NewGuid().ToString(), CategoryId = catProduct.Id, ProductName = "Sản phẩm 08",   CreateByUserId = useradmin.Id, StoreId = store1.Id },
                            new Product { Id = Guid.NewGuid().ToString(), CategoryId = catProduct.Id, ProductName = "Sản phẩm 09",   CreateByUserId = useradmin.Id, StoreId = store1.Id },
                            new Product { Id = Guid.NewGuid().ToString(), CategoryId = catProduct.Id, ProductName = "Sản phẩm 10",   CreateByUserId = useradmin.Id, StoreId = store1.Id },
                            new Product { Id = Guid.NewGuid().ToString(), CategoryId = catProduct.Id, ProductName = "Sản phẩm 11",   CreateByUserId = useradmin.Id, StoreId = store1.Id },
                            new Product { Id = Guid.NewGuid().ToString(), CategoryId = catProduct.Id, ProductName = "Sản phẩm 12",   CreateByUserId = useradmin.Id, StoreId = store1.Id },
                            new Product { Id = Guid.NewGuid().ToString(), CategoryId = catProduct.Id, ProductName = "Sản phẩm 13",   CreateByUserId = useradmin.Id, StoreId = store1.Id },
                            new Product { Id = Guid.NewGuid().ToString(), CategoryId = catProduct.Id, ProductName = "Sản phẩm 14",   CreateByUserId = useradmin.Id, StoreId = store1.Id },
                        };
                        catProduct.Products = lstProducts;

                        await databaseDbContext.AddAsync(catProduct);
                        await databaseDbContext.SaveChangesAsync();
                    }
                }
            }

            if (!databaseDbContext.Permissions.Any())
            {
                await databaseDbContext.Permissions.AddRangeAsync(new List<Permission>() {
                    new Permission { Id = Guid.NewGuid().ToString(), PermissionName = Permissions.SysAdmin.ImportLocation },
                    new Permission { Id = Guid.NewGuid().ToString(), PermissionName = Permissions.SysAdmin.ViewLogs },
                    new Permission { Id = Guid.NewGuid().ToString(), PermissionName = Permissions.SysAdmin.DeleteLogs },
                    new Permission { Id = Guid.NewGuid().ToString(), PermissionName = Permissions.Account.Create },
                    new Permission { Id = Guid.NewGuid().ToString(), PermissionName = Permissions.Account.Delete },
                    new Permission { Id = Guid.NewGuid().ToString(), PermissionName = Permissions.Account.Edit },
                    new Permission { Id = Guid.NewGuid().ToString(), PermissionName = Permissions.Account.View },
                    new Permission { Id = Guid.NewGuid().ToString(), PermissionName = Permissions.Users.View },
                    new Permission { Id = Guid.NewGuid().ToString(), PermissionName = Permissions.Users.Create },
                    new Permission { Id = Guid.NewGuid().ToString(), PermissionName = Permissions.Users.Edit },
                    new Permission { Id = Guid.NewGuid().ToString(), PermissionName = Permissions.Users.Delete },
                    new Permission { Id = Guid.NewGuid().ToString(), PermissionName = Permissions.SysAdmin.GetListCategoryStore },
                    new Permission { Id = Guid.NewGuid().ToString(), PermissionName = Permissions.AdminStore.GetListStoreOption },
                    new Permission { Id = Guid.NewGuid().ToString(), PermissionName = Permissions.AdminStore.GetMenuProduct },
                    new Permission { Id = Guid.NewGuid().ToString(), PermissionName = Permissions.AdminStore.CreateProduct },
                    new Permission { Id = Guid.NewGuid().ToString(), PermissionName = Permissions.AdminStore.DeleteProduct },
                    new Permission { Id = Guid.NewGuid().ToString(), PermissionName = Permissions.AdminStore.UpdateProduct },
                    new Permission { Id = Guid.NewGuid().ToString(), PermissionName = Permissions.AdminStore.GetListProduct },
                    new Permission { Id = Guid.NewGuid().ToString(), PermissionName = Permissions.Employee.ConfirmOrder },
                    new Permission { Id = Guid.NewGuid().ToString(), PermissionName = Permissions.Employee.ConfirmPayOrder },
                    new Permission { Id = Guid.NewGuid().ToString(), PermissionName = Permissions.Employee.CreateOrder },
                    new Permission { Id = Guid.NewGuid().ToString(), PermissionName = Permissions.Employee.GetCategoryProductsForEmployee },
                    new Permission { Id = Guid.NewGuid().ToString(), PermissionName = Permissions.Employee.GetListOrderWithTableOrProductName },
                    new Permission { Id = Guid.NewGuid().ToString(), PermissionName = Permissions.Employee.GetListProductsOfStoreForEmployee },
                    new Permission { Id = Guid.NewGuid().ToString(), PermissionName = Permissions.Employee.GetListProductsOfStoreForEmployeeV2 },
                    new Permission { Id = Guid.NewGuid().ToString(), PermissionName = Permissions.Employee.GetListProductsOfStoreWithCategoryForEmployee },
                    new Permission { Id = Guid.NewGuid().ToString(), PermissionName = Permissions.Employee.GetListProductsOfStoreWithCategoryForEmployeeV2 },
                    new Permission { Id = Guid.NewGuid().ToString(), PermissionName = Permissions.Employee.GetListTables },
                    new Permission { Id = Guid.NewGuid().ToString(), PermissionName = Permissions.Employee.GetOrderProductsWithTable },
                    new Permission { Id = Guid.NewGuid().ToString(), PermissionName = Permissions.Employee.GetOrderWithTableViewProductsWithTableId },
                    new Permission { Id = Guid.NewGuid().ToString(), PermissionName = Permissions.Employee.UpdateOrder },
                    new Permission { Id = Guid.NewGuid().ToString(), PermissionName = Permissions.Upload.UploadFile },
                });
                await databaseDbContext.SaveChangesAsync();
            }

            if (!databaseDbContext.RoleToPermissions.Any())
            {
                // get role
                var roleSysadmin = await databaseDbContext.Roles.FirstOrDefaultAsync(x => x.RoleName == RoleTypeHelper.RoleSysAdmin);
                var roleAdminStore = await databaseDbContext.Roles.FirstOrDefaultAsync(x => x.RoleName == RoleTypeHelper.RoleAdmin);
                var roleOrderUser = await databaseDbContext.Roles.FirstOrDefaultAsync(x => x.RoleName == RoleTypeHelper.RoleOrderUser);
                var roleCookieUser = await databaseDbContext.Roles.FirstOrDefaultAsync(x => x.RoleName == RoleTypeHelper.RoleCookieUser);
                var rolePayUser = await databaseDbContext.Roles.FirstOrDefaultAsync(x => x.RoleName == RoleTypeHelper.RolePayUser);

                #region update roleSysAdmin
                var permissionsSysAdmin = databaseDbContext.Permissions
                    .Where(x => x.PermissionName == Permissions.SysAdmin.ImportLocation ||
                                x.PermissionName == Permissions.SysAdmin.ViewLogs ||
                                x.PermissionName == Permissions.SysAdmin.DeleteLogs ||
                                x.PermissionName == Permissions.Account.Create ||
                                x.PermissionName == Permissions.Account.Delete ||
                                x.PermissionName == Permissions.Account.Edit ||
                                x.PermissionName == Permissions.Account.View ||
                                x.PermissionName == Permissions.SysAdmin.GetListStores ||
                                x.PermissionName == Permissions.SysAdmin.GetListCategoryStore
                    ).Select(x => x).ToList();

                foreach (var permission in permissionsSysAdmin)
                {
                    // add Permissions To Role
                    roleSysadmin.RoleToPermissions.Add(new RoleToPermission
                    {
                        Permission = permission,
                        Role = roleSysadmin,
                    });
                }
                // save to db
                await databaseDbContext.SaveChangesAsync();
                #endregion

                #region Update RoleAdminStore
                var permissionsAdminStore = databaseDbContext.Permissions
                   .Where(x => x.PermissionName == Permissions.AdminStore.GetMenuProduct ||
                               x.PermissionName == Permissions.AdminStore.GetListStoreOption ||
                               x.PermissionName == Permissions.AdminStore.GetListProduct ||
                               x.PermissionName == Permissions.AdminStore.CreateProduct ||
                               x.PermissionName == Permissions.AdminStore.UpdateProduct ||
                               x.PermissionName == Permissions.AdminStore.DeleteProduct ||
                               x.PermissionName == Permissions.Upload.UploadFile ||
                               x.PermissionName == Permissions.AdminStore.CreateCatProduct ||
                               x.PermissionName == Permissions.AdminStore.UpdateCatProduct ||
                               x.PermissionName == Permissions.AdminStore.DeleteCatProduct ||
                               x.PermissionName == Permissions.AdminStore.GetListCatProduct

                   ).Select(x => x).ToList();

                foreach (var permission in permissionsAdminStore)
                {
                    // add Permissions To Role
                    roleAdminStore.RoleToPermissions.Add(new RoleToPermission
                    {
                        Permission = permission,
                        Role = roleAdminStore,
                    });
                }
                // save to db
                await databaseDbContext.SaveChangesAsync();
                #endregion

                #region Update RoleOrderUser
                var permissionsOrderUsers = databaseDbContext.Permissions
                   .Where(x => x.PermissionName == Permissions.Employee.GetListTables ||
                               x.PermissionName == Permissions.Employee.GetCategoryProductsForEmployee ||
                               x.PermissionName == Permissions.Employee.GetListProductsOfStoreForEmployee ||
                               x.PermissionName == Permissions.Employee.GetListProductsOfStoreWithCategoryForEmployee ||
                               x.PermissionName == Permissions.Employee.GetListProductsOfStoreForEmployeeV2 ||
                               x.PermissionName == Permissions.Employee.GetListProductsOfStoreWithCategoryForEmployeeV2 ||
                               x.PermissionName == Permissions.Employee.CreateOrder ||
                               x.PermissionName == Permissions.Employee.UpdateOrder ||
                               x.PermissionName == Permissions.Employee.GetOrderProductsWithTable

                   ).Select(x => x).ToList();

                foreach (var permission in permissionsOrderUsers)
                {
                    // add Permissions To Role
                    roleOrderUser.RoleToPermissions.Add(new RoleToPermission
                    {
                        Permission = permission,
                        Role = roleOrderUser,
                    });
                }
                // save to db
                await databaseDbContext.SaveChangesAsync();
                #endregion

                #region Update RoleCookieUser
                var permissionsOrderCookies = databaseDbContext.Permissions
                   .Where(x => x.PermissionName == Permissions.Employee.GetOrderProductsWithTable ||
                               x.PermissionName == Permissions.Employee.GetListOrderWithTableOrProductName ||
                               x.PermissionName == Permissions.Employee.GetOrderWithTableViewProductsWithTableId ||
                               x.PermissionName == Permissions.Employee.ConfirmOrder || 
                               x.PermissionName == Permissions.Employee.UpdateOrderProductStatus
                   ).Select(x => x).ToList();

                foreach (var permission in permissionsOrderCookies)
                {
                    // add Permissions To Role
                    roleCookieUser.RoleToPermissions.Add(new RoleToPermission
                    {
                        Permission = permission,
                        Role = roleCookieUser,
                    });
                }
                // save to db
                await databaseDbContext.SaveChangesAsync();
                #endregion

                #region Update rolePayUser
                var permissionsPayUsers = databaseDbContext.Permissions
                   .Where(x => x.PermissionName == Permissions.Employee.ConfirmOrder ||
                               x.PermissionName == Permissions.Employee.ConfirmPayOrder
                   ).Select(x => x).ToList();

                foreach (var permission in permissionsPayUsers)
                {
                    // add Permissions To Role
                    rolePayUser.RoleToPermissions.Add(new RoleToPermission
                    {
                        Permission = permission,
                        Role = rolePayUser,
                    });
                }
                // save to db
                await databaseDbContext.SaveChangesAsync();
                #endregion

            }

            if (!databaseDbContext.Users.Include(x => x.UserToRoles).ThenInclude(x => x.Role).Any(x => x.UserToRoles.Any(r => r.Role.RoleName == RoleTypeHelper.RoleSysAdmin)))
            {
                var roleSysAdmin = await databaseDbContext.Roles.FirstOrDefaultAsync(x => x.RoleName == RoleTypeHelper.RoleSysAdmin);
                string password = "string@12345";
                var hashPass = Helpers.SercurityHelper.GenerateSaltedHash(password);
                var userSysAdmin = new User { Id = Guid.NewGuid().ToString(), Age = 29, BirthDay = new DateTime(1991, 1, 1).ToUniversalTime(), CountLoginFailed = 0, FirstName = "sysadmin", LastName = "sysadmin", IsActived = 1, Email = "nguyenvannam0411@gmail.com", UserName = "ngvannam", HashPassword = hashPass.Hash, SaltPassword = hashPass.Salt, Gender = 1, OldPassword = hashPass.Hash + ";" + hashPass.Salt, PhoneNumber = "0349801673" };

                roleSysAdmin.UserToRoles.Add(new UserToRole
                {
                    Role = roleSysAdmin,
                    User = userSysAdmin,
                });

                // save to db
                await databaseDbContext.SaveChangesAsync();
            }
        }
    }
}
