using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StoreOrder.WebApplication.Data.Migrations
{
    public partial class InitialDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Account");

            migrationBuilder.EnsureSchema(
                name: "Location");

            migrationBuilder.EnsureSchema(
                name: "Order");

            migrationBuilder.EnsureSchema(
                name: "Product");

            migrationBuilder.EnsureSchema(
                name: "Store");

            migrationBuilder.CreateTable(
                name: "Permissions",
                schema: "Account",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 255, nullable: false),
                    PermissionName = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "Account",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 255, nullable: false),
                    RoleName = table.Column<string>(maxLength: 255, nullable: true),
                    Desc = table.Column<string>(maxLength: 255, nullable: true),
                    Code = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "Account",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 255, nullable: false),
                    FirstName = table.Column<string>(maxLength: 50, nullable: true),
                    LastName = table.Column<string>(maxLength: 50, nullable: true),
                    Gender = table.Column<int>(nullable: true),
                    PhoneNumber = table.Column<string>(maxLength: 11, nullable: true),
                    Email = table.Column<string>(maxLength: 50, nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    SaltPassword = table.Column<string>(type: "TEXT", nullable: true),
                    HashPassword = table.Column<string>(type: "TEXT", nullable: true),
                    CountLoginFailed = table.Column<int>(nullable: true),
                    OldPassword = table.Column<string>(type: "TEXT", nullable: true),
                    IsActived = table.Column<int>(nullable: true),
                    Age = table.Column<int>(nullable: true),
                    BirthDay = table.Column<DateTime>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    LastLogin = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Providers",
                schema: "Location",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 50, nullable: false),
                    Name = table.Column<string>(maxLength: 250, nullable: true),
                    Slug = table.Column<string>(maxLength: 250, nullable: true),
                    Type = table.Column<string>(maxLength: 250, nullable: true),
                    NameWithType = table.Column<string>(maxLength: 250, nullable: true),
                    Code = table.Column<string>(maxLength: 50, nullable: true),
                    Path = table.Column<string>(maxLength: 250, nullable: true),
                    PathWithType = table.Column<string>(maxLength: 250, nullable: true),
                    ParentId = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Providers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Providers_Providers_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "Location",
                        principalTable: "Providers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CategoryProducts",
                schema: "Product",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 50, nullable: false),
                    CategoryName = table.Column<string>(maxLength: 250, nullable: true),
                    Slug = table.Column<string>(maxLength: 250, nullable: true),
                    Code = table.Column<string>(maxLength: 250, nullable: true),
                    ParentId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryProducts_CategoryProducts_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "Product",
                        principalTable: "CategoryProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CategoryStores",
                schema: "Store",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 50, nullable: false),
                    Name = table.Column<string>(maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryStores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleToPermissions",
                schema: "Account",
                columns: table => new
                {
                    RoleId = table.Column<string>(nullable: false),
                    PermissionId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleToPermissions", x => new { x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_RoleToPermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalSchema: "Account",
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleToPermissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Account",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExternalSignIns",
                schema: "Account",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 255, nullable: false),
                    TypeLogin = table.Column<int>(nullable: true),
                    IsVerified = table.Column<int>(nullable: true),
                    LastLogin = table.Column<DateTime>(nullable: true),
                    TokenLogin = table.Column<string>(type: "TEXT", nullable: true),
                    TimeLifeToken = table.Column<int>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalSignIns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalSignIns_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserDevices",
                schema: "Account",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 255, nullable: false),
                    CodeDevice = table.Column<string>(maxLength: 250, nullable: true),
                    IsVerified = table.Column<int>(nullable: true),
                    VerifiedCode = table.Column<int>(nullable: true),
                    TimeCode = table.Column<int>(nullable: true),
                    LastLogin = table.Column<DateTime>(nullable: true),
                    CurrentUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDevices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserDevices_Users_CurrentUserId",
                        column: x => x.CurrentUserId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserToRoles",
                schema: "Account",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserToRoles", x => new { x.RoleId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserToRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Account",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserToRoles_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stores",
                schema: "Store",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 50, nullable: false),
                    StoreName = table.Column<string>(maxLength: 250, nullable: true),
                    StoreAddress = table.Column<string>(maxLength: 250, nullable: true),
                    lat = table.Column<double>(nullable: true),
                    lon = table.Column<double>(nullable: true),
                    StatusStore = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    ProviderId = table.Column<string>(maxLength: 50, nullable: true),
                    UserId = table.Column<string>(maxLength: 255, nullable: true),
                    CategoryStoreId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stores_CategoryStores_CategoryStoreId",
                        column: x => x.CategoryStoreId,
                        principalSchema: "Store",
                        principalTable: "CategoryStores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Stores_Providers_ProviderId",
                        column: x => x.ProviderId,
                        principalSchema: "Location",
                        principalTable: "Providers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "Product",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 50, nullable: false),
                    ProductName = table.Column<string>(maxLength: 250, nullable: false),
                    ProductPrice = table.Column<decimal>(nullable: true),
                    UniversalProductCode = table.Column<string>(maxLength: 32, nullable: true),
                    Height = table.Column<decimal>(nullable: false),
                    Weight = table.Column<decimal>(nullable: false),
                    NetWeight = table.Column<decimal>(nullable: false),
                    Depth = table.Column<decimal>(nullable: false),
                    CreateOn = table.Column<DateTime>(nullable: true),
                    UpdateOn = table.Column<DateTime>(nullable: true),
                    DeleteOn = table.Column<DateTime>(nullable: true),
                    ProductParentId = table.Column<string>(nullable: true),
                    CategoryId = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    StoreId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_CategoryProducts_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "Product",
                        principalTable: "CategoryProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_Products_ProductParentId",
                        column: x => x.ProductParentId,
                        principalSchema: "Product",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_Stores_StoreId",
                        column: x => x.StoreId,
                        principalSchema: "Store",
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Products_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "StoreOptions",
                schema: "Store",
                columns: table => new
                {
                    OptionId = table.Column<string>(maxLength: 50, nullable: false),
                    StoreOptionName = table.Column<string>(maxLength: 250, nullable: true),
                    StoreOptionDescription = table.Column<string>(maxLength: 250, nullable: true),
                    StoreId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreOptions", x => x.OptionId);
                    table.ForeignKey(
                        name: "FK_StoreOptions_Stores_StoreId",
                        column: x => x.StoreId,
                        principalSchema: "Store",
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "StoreTables",
                schema: "Store",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 50, nullable: false),
                    TableName = table.Column<string>(maxLength: 250, nullable: true),
                    TableCode = table.Column<string>(maxLength: 250, nullable: true),
                    Location = table.Column<int>(nullable: true),
                    LocationUnit = table.Column<int>(nullable: true),
                    TableStatus = table.Column<int>(nullable: true),
                    StoreId = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreTables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreTables_Stores_StoreId",
                        column: x => x.StoreId,
                        principalSchema: "Store",
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductOptions",
                schema: "Product",
                columns: table => new
                {
                    ProductId = table.Column<string>(nullable: false),
                    OptionId = table.Column<string>(maxLength: 50, nullable: false),
                    OptionName = table.Column<string>(maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductOptions", x => new { x.ProductId, x.OptionId });
                    table.ForeignKey(
                        name: "FK_ProductOptions_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "Product",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductSKUs",
                schema: "Product",
                columns: table => new
                {
                    ProductId = table.Column<string>(nullable: false),
                    SkuId = table.Column<string>(nullable: false),
                    Sku = table.Column<string>(maxLength: 64, nullable: false),
                    Price = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSKUs", x => new { x.ProductId, x.SkuId });
                    table.ForeignKey(
                        name: "FK_ProductSKUs_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "Product",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                schema: "Order",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 50, nullable: false),
                    OrderStatus = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    TableId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_StoreTables_TableId",
                        column: x => x.TableId,
                        principalSchema: "Store",
                        principalTable: "StoreTables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductOptionValues",
                schema: "Product",
                columns: table => new
                {
                    ProductId = table.Column<string>(nullable: false),
                    ValueId = table.Column<string>(maxLength: 256, nullable: false),
                    OptionId = table.Column<string>(nullable: false),
                    ValueName = table.Column<string>(maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductOptionValues", x => new { x.ProductId, x.OptionId, x.ValueId });
                    table.ForeignKey(
                        name: "FK_ProductOptionValues_ProductOptions_ProductId_OptionId",
                        columns: x => new { x.ProductId, x.OptionId },
                        principalSchema: "Product",
                        principalTable: "ProductOptions",
                        principalColumns: new[] { "ProductId", "OptionId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                schema: "Order",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 50, nullable: false),
                    ProductId = table.Column<string>(maxLength: 50, nullable: true),
                    Quality = table.Column<int>(nullable: true),
                    Amount = table.Column<int>(nullable: true),
                    Rate = table.Column<int>(nullable: true),
                    Note = table.Column<string>(maxLength: 250, nullable: true),
                    OrderId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "Order",
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserCookingOrders",
                schema: "Order",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 50, nullable: false),
                    StatusCooking = table.Column<int>(nullable: true),
                    CreateOn = table.Column<DateTime>(nullable: true),
                    UpdateOn = table.Column<DateTime>(nullable: true),
                    DurationTime = table.Column<int>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    OrderId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCookingOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserCookingOrders_Orders_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "Order",
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserCookingOrders_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductSKUValues",
                schema: "Product",
                columns: table => new
                {
                    ProductId = table.Column<string>(nullable: false),
                    SkuId = table.Column<string>(nullable: false),
                    OptionId = table.Column<string>(nullable: false),
                    ValueId = table.Column<string>(nullable: true),
                    ProductSKUProductId = table.Column<string>(nullable: true),
                    ProductSKUSkuId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSKUValues", x => new { x.ProductId, x.SkuId, x.OptionId });
                    table.ForeignKey(
                        name: "FK_ProductSKUValues_ProductOptions_ProductId_OptionId",
                        columns: x => new { x.ProductId, x.OptionId },
                        principalSchema: "Product",
                        principalTable: "ProductOptions",
                        principalColumns: new[] { "ProductId", "OptionId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductSKUValues_ProductSKUs_ProductId_SkuId",
                        columns: x => new { x.ProductId, x.SkuId },
                        principalSchema: "Product",
                        principalTable: "ProductSKUs",
                        principalColumns: new[] { "ProductId", "SkuId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductSKUValues_ProductSKUs_ProductSKUProductId_ProductSKU~",
                        columns: x => new { x.ProductSKUProductId, x.ProductSKUSkuId },
                        principalSchema: "Product",
                        principalTable: "ProductSKUs",
                        principalColumns: new[] { "ProductId", "SkuId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductSKUValues_ProductOptionValues_ProductId_OptionId_Val~",
                        columns: x => new { x.ProductId, x.OptionId, x.ValueId },
                        principalSchema: "Product",
                        principalTable: "ProductOptionValues",
                        principalColumns: new[] { "ProductId", "OptionId", "ValueId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExternalSignIns_UserId",
                schema: "Account",
                table: "ExternalSignIns",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleToPermissions_PermissionId",
                schema: "Account",
                table: "RoleToPermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDevices_CurrentUserId",
                schema: "Account",
                table: "UserDevices",
                column: "CurrentUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email_UserName",
                schema: "Account",
                table: "Users",
                columns: new[] { "Email", "UserName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserToRoles_UserId",
                schema: "Account",
                table: "UserToRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Providers_ParentId",
                schema: "Location",
                table: "Providers",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                schema: "Order",
                table: "OrderDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_TableId",
                schema: "Order",
                table: "Orders",
                column: "TableId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                schema: "Order",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCookingOrders_OrderId",
                schema: "Order",
                table: "UserCookingOrders",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCookingOrders_UserId",
                schema: "Order",
                table: "UserCookingOrders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryProducts_ParentId",
                schema: "Product",
                table: "CategoryProducts",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                schema: "Product",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductParentId",
                schema: "Product",
                table: "Products",
                column: "ProductParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_StoreId",
                schema: "Product",
                table: "Products",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_UserId",
                schema: "Product",
                table: "Products",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSKUs_Sku",
                schema: "Product",
                table: "ProductSKUs",
                column: "Sku");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSKUValues_ProductSKUProductId_ProductSKUSkuId",
                schema: "Product",
                table: "ProductSKUValues",
                columns: new[] { "ProductSKUProductId", "ProductSKUSkuId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductSKUValues_ProductId_OptionId_ValueId",
                schema: "Product",
                table: "ProductSKUValues",
                columns: new[] { "ProductId", "OptionId", "ValueId" });

            migrationBuilder.CreateIndex(
                name: "IX_StoreOptions_StoreId",
                schema: "Store",
                table: "StoreOptions",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_CategoryStoreId",
                schema: "Store",
                table: "Stores",
                column: "CategoryStoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_ProviderId",
                schema: "Store",
                table: "Stores",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreTables_StoreId",
                schema: "Store",
                table: "StoreTables",
                column: "StoreId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExternalSignIns",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "RoleToPermissions",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "UserDevices",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "UserToRoles",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "OrderDetails",
                schema: "Order");

            migrationBuilder.DropTable(
                name: "UserCookingOrders",
                schema: "Order");

            migrationBuilder.DropTable(
                name: "ProductSKUValues",
                schema: "Product");

            migrationBuilder.DropTable(
                name: "StoreOptions",
                schema: "Store");

            migrationBuilder.DropTable(
                name: "Permissions",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "Orders",
                schema: "Order");

            migrationBuilder.DropTable(
                name: "ProductSKUs",
                schema: "Product");

            migrationBuilder.DropTable(
                name: "ProductOptionValues",
                schema: "Product");

            migrationBuilder.DropTable(
                name: "StoreTables",
                schema: "Store");

            migrationBuilder.DropTable(
                name: "ProductOptions",
                schema: "Product");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "Product");

            migrationBuilder.DropTable(
                name: "CategoryProducts",
                schema: "Product");

            migrationBuilder.DropTable(
                name: "Stores",
                schema: "Store");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "CategoryStores",
                schema: "Store");

            migrationBuilder.DropTable(
                name: "Providers",
                schema: "Location");
        }
    }
}
