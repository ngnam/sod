using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
                var hashPass = Helpers.SercurityHelper.GenerateSaltedHash(10, password);

                //// I will add 1 user to one Role
                //var book1 = new Book();
                //var book2 = new Book();
                var roleAdmin = new Role { Id = Guid.NewGuid().ToString(), Code = Guid.NewGuid().ToString(), RoleName = RoleTypeHelper.RoleAdmin, Desc = "Admin of Store" };
                var roleOrderUser = new Role { Id = Guid.NewGuid().ToString(), Code = Guid.NewGuid().ToString(), RoleName = RoleTypeHelper.RoleOrderUser, Desc = "NV Order of Store" };
                var roleCookieUser = new Role { Id = Guid.NewGuid().ToString(), Code = Guid.NewGuid().ToString(), RoleName = RoleTypeHelper.RoleCookieUser, Desc = "NV phụ bếp of Store" };
                var rolePayUser = new Role { Id = Guid.NewGuid().ToString(), Code = Guid.NewGuid().ToString(), RoleName = RoleTypeHelper.RolePayUser, Desc = "Nv Thanh toán of Store" };
                var roleCustomerUser = new Role { Id = Guid.NewGuid().ToString(), Code = Guid.NewGuid().ToString(), RoleName = RoleTypeHelper.roleCustomerUser, Desc = "Role Account of Customer" };

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
                var roleCustomerUser = new Role { Id = Guid.NewGuid().ToString(), Code = Guid.NewGuid().ToString(), RoleName = RoleTypeHelper.roleCustomerUser, Desc = "Role Account of Customer" };
                await databaseDbContext.Roles.AddAsync(roleCustomerUser);
                await databaseDbContext.SaveChangesAsync();
            }

            if (!databaseDbContext.CategoryStores.Any())
            {
                var catStore = new CategoryStore { Id = Guid.NewGuid().ToString(), Name = "Store1" };
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
                            UserId = useradmin.Id,
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
        }
    }
}
