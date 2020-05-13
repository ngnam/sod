﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using StoreOrder.WebApplication.Data;

namespace StoreOrder.WebApplication.Data.Migrations
{
    [DbContext(typeof(StoreOrderDbContext))]
    [Migration("20200512080213_AddColumnFixedPrice")]
    partial class AddColumnFixedPrice
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Account.ExternalSignIn", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("character varying(255)")
                        .HasMaxLength(255);

                    b.Property<int?>("IsVerified")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("LastLogin")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("TimeLifeToken")
                        .HasColumnType("integer");

                    b.Property<string>("TokenLogin")
                        .HasColumnType("TEXT");

                    b.Property<int?>("TypeLogin")
                        .HasColumnType("integer");

                    b.Property<string>("UserId")
                        .HasColumnType("character varying(255)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("ExternalSignIns","Account");
                });

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Account.Permission", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("character varying(255)")
                        .HasMaxLength(255);

                    b.Property<string>("PermissionName")
                        .HasColumnType("character varying(255)")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.ToTable("Permissions","Account");
                });

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Account.Role", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("character varying(255)")
                        .HasMaxLength(255);

                    b.Property<string>("Code")
                        .HasColumnType("character varying(255)")
                        .HasMaxLength(255);

                    b.Property<string>("Desc")
                        .HasColumnType("character varying(255)")
                        .HasMaxLength(255);

                    b.Property<string>("RoleName")
                        .HasColumnType("character varying(255)")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.HasIndex("RoleName")
                        .IsUnique();

                    b.ToTable("Roles","Account");
                });

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Account.RoleToPermission", b =>
                {
                    b.Property<string>("RoleId")
                        .HasColumnType("character varying(255)");

                    b.Property<string>("PermissionId")
                        .HasColumnType("character varying(255)");

                    b.HasKey("RoleId", "PermissionId");

                    b.HasIndex("PermissionId");

                    b.ToTable("RoleToPermissions","Account");
                });

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Account.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("character varying(255)")
                        .HasMaxLength(255);

                    b.Property<int?>("Age")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("BirthDay")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("CountLoginFailed")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Email")
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.Property<string>("FirstName")
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.Property<int?>("Gender")
                        .HasColumnType("integer");

                    b.Property<string>("HashPassword")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IsActived")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("LastLogin")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("LastName")
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.Property<string>("OldPassword")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("character varying(11)")
                        .HasMaxLength(11);

                    b.Property<string>("SaltPassword")
                        .HasColumnType("TEXT");

                    b.Property<string>("StoreId")
                        .HasColumnType("character varying(50)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("StoreId");

                    b.HasIndex("Email", "UserName")
                        .IsUnique();

                    b.ToTable("Users","Account");
                });

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Account.UserDevice", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("character varying(255)")
                        .HasMaxLength(255);

                    b.Property<string>("CodeDevice")
                        .HasColumnType("character varying(250)")
                        .HasMaxLength(250);

                    b.Property<string>("CurrentUserId")
                        .HasColumnType("character varying(255)");

                    b.Property<int?>("IsVerified")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("LastLogin")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("TimeCode")
                        .HasColumnType("integer");

                    b.Property<int?>("VerifiedCode")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CurrentUserId");

                    b.ToTable("UserDevices","Account");
                });

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Account.UserLogin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("AccessToken")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ExpiresIn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool?>("IsLoggedIn")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("LastLogin")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("NameIdentifier")
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.Property<string>("TokenType")
                        .HasColumnType("character varying(25)")
                        .HasMaxLength(25);

                    b.Property<string>("UserId")
                        .HasColumnType("character varying(255)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogins","Account");
                });

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Account.UserToRole", b =>
                {
                    b.Property<string>("RoleId")
                        .HasColumnType("character varying(255)");

                    b.Property<string>("UserId")
                        .HasColumnType("character varying(255)");

                    b.HasKey("RoleId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserToRoles","Account");
                });

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Location.Provider", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Code")
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Name")
                        .HasColumnType("character varying(250)")
                        .HasMaxLength(250);

                    b.Property<string>("NameWithType")
                        .HasColumnType("character varying(250)")
                        .HasMaxLength(250);

                    b.Property<string>("ParentId")
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Path")
                        .HasColumnType("character varying(250)")
                        .HasMaxLength(250);

                    b.Property<string>("PathWithType")
                        .HasColumnType("character varying(250)")
                        .HasMaxLength(250);

                    b.Property<string>("Slug")
                        .HasColumnType("character varying(250)")
                        .HasMaxLength(250);

                    b.Property<string>("Type")
                        .HasColumnType("character varying(250)")
                        .HasMaxLength(250);

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("Providers","Location");
                });

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Orders.Order", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("OrderStatus")
                        .HasColumnType("integer");

                    b.Property<string>("TableId")
                        .HasColumnType("character varying(50)");

                    b.Property<string>("UserId")
                        .HasColumnType("character varying(255)");

                    b.HasKey("Id");

                    b.HasIndex("TableId");

                    b.HasIndex("UserId");

                    b.ToTable("Orders","Order");
                });

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Orders.UserCookingOrder", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("CreateOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("DurationTime")
                        .HasColumnType("integer");

                    b.Property<string>("OrderId")
                        .HasColumnType("character varying(50)");

                    b.Property<int?>("StatusCooking")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("UpdateOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("UserId")
                        .HasColumnType("character varying(255)");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("UserId");

                    b.ToTable("UserCookingOrders","Order");
                });

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Products.CategoryProduct", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.Property<string>("CategoryName")
                        .HasColumnType("character varying(250)")
                        .HasMaxLength(250);

                    b.Property<string>("Code")
                        .HasColumnType("character varying(250)")
                        .HasMaxLength(250);

                    b.Property<string>("ParentId")
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Slug")
                        .HasColumnType("character varying(250)")
                        .HasMaxLength(250);

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("CategoryProducts","Product");
                });

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Products.Product", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.Property<string>("CategoryId")
                        .HasColumnType("character varying(50)");

                    b.Property<string>("CreateByUserId")
                        .HasColumnType("character varying(255)");

                    b.Property<DateTime?>("CreateOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("DeleteOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<decimal>("Depth")
                        .HasColumnType("numeric");

                    b.Property<decimal>("FixedPrice")
                        .HasColumnType("numeric");

                    b.Property<decimal>("Height")
                        .HasColumnType("numeric");

                    b.Property<decimal>("NetWeight")
                        .HasColumnType("numeric");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("character varying(250)")
                        .HasMaxLength(250);

                    b.Property<string>("ProductParentId")
                        .HasColumnType("character varying(50)");

                    b.Property<string>("StoreId")
                        .HasColumnType("character varying(50)");

                    b.Property<string>("UniversalProductCode")
                        .HasColumnType("character varying(32)")
                        .HasMaxLength(32);

                    b.Property<DateTime?>("UpdateOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<decimal>("Weight")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("CreateByUserId");

                    b.HasIndex("ProductParentId");

                    b.HasIndex("StoreId");

                    b.ToTable("Products","Product");
                });

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Products.ProductDetail", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ImageAlbumJson")
                        .HasColumnType("TEXT");

                    b.Property<int?>("ImageHeightThumb")
                        .HasColumnType("integer");

                    b.Property<string>("ImageOrigin")
                        .HasColumnType("character varying(500)")
                        .HasMaxLength(500);

                    b.Property<string>("ImageThumb")
                        .HasColumnType("character varying(500)")
                        .HasMaxLength(500);

                    b.Property<int?>("ImageWidthThumb")
                        .HasColumnType("integer");

                    b.Property<string>("LongDescription")
                        .HasColumnType("character varying(500)")
                        .HasMaxLength(500);

                    b.Property<string>("ProductId")
                        .HasColumnType("character varying(50)");

                    b.Property<string>("ShortDescription")
                        .HasColumnType("character varying(250)")
                        .HasMaxLength(250);

                    b.HasKey("Id");

                    b.HasIndex("Id");

                    b.HasIndex("ProductId")
                        .IsUnique();

                    b.ToTable("ProductDetails","Product");
                });

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Products.ProductImage", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("ImageHeightThumb")
                        .HasColumnType("integer");

                    b.Property<string>("ImageOrigin")
                        .HasColumnType("character varying(500)")
                        .HasMaxLength(500);

                    b.Property<string>("ImageThumb")
                        .HasColumnType("character varying(500)")
                        .HasMaxLength(500);

                    b.Property<int?>("ImageWidthThumb")
                        .HasColumnType("integer");

                    b.Property<long>("ProductDetailId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ProductDetailId");

                    b.ToTable("ProductImages","Product");
                });

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Products.ProductOption", b =>
                {
                    b.Property<string>("ProductId")
                        .HasColumnType("character varying(50)");

                    b.Property<string>("OptionId")
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.Property<string>("OptionName")
                        .IsRequired()
                        .HasColumnType("character varying(40)")
                        .HasMaxLength(40);

                    b.HasKey("ProductId", "OptionId");

                    b.ToTable("ProductOptions","Product");
                });

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Products.ProductOptionValue", b =>
                {
                    b.Property<string>("ProductId")
                        .HasColumnType("character varying(50)");

                    b.Property<string>("OptionId")
                        .HasColumnType("character varying(50)");

                    b.Property<string>("ValueId")
                        .HasColumnType("character varying(256)")
                        .HasMaxLength(256);

                    b.Property<string>("ValueName")
                        .IsRequired()
                        .HasColumnType("character varying(32)")
                        .HasMaxLength(32);

                    b.HasKey("ProductId", "OptionId", "ValueId");

                    b.ToTable("ProductOptionValues","Product");
                });

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Products.ProductSKU", b =>
                {
                    b.Property<string>("ProductId")
                        .HasColumnType("character varying(50)");

                    b.Property<string>("SkuId")
                        .HasColumnType("text");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<string>("Sku")
                        .IsRequired()
                        .HasColumnType("character varying(64)")
                        .HasMaxLength(64);

                    b.HasKey("ProductId", "SkuId");

                    b.HasIndex("Sku");

                    b.ToTable("ProductSKUs","Product");
                });

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Products.ProductSKUValue", b =>
                {
                    b.Property<string>("ProductId")
                        .HasColumnType("character varying(50)");

                    b.Property<string>("SkuId")
                        .HasColumnType("text");

                    b.Property<string>("OptionId")
                        .HasColumnType("character varying(50)");

                    b.Property<string>("ProductSKUProductId")
                        .HasColumnType("character varying(50)");

                    b.Property<string>("ProductSKUSkuId")
                        .HasColumnType("text");

                    b.Property<string>("ValueId")
                        .HasColumnType("character varying(256)");

                    b.HasKey("ProductId", "SkuId", "OptionId");

                    b.HasIndex("ProductSKUProductId", "ProductSKUSkuId");

                    b.HasIndex("ProductId", "OptionId", "ValueId");

                    b.ToTable("ProductSKUValues","Product");
                });

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Stores.CategoryStore", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Name")
                        .HasColumnType("character varying(250)")
                        .HasMaxLength(250);

                    b.HasKey("Id");

                    b.ToTable("CategoryStores","Store");
                });

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Stores.Store", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.Property<string>("CategoryStoreId")
                        .HasColumnType("character varying(50)");

                    b.Property<string>("CreateByUserId")
                        .HasColumnType("character varying(255)")
                        .HasMaxLength(255);

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("ProviderId")
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.Property<int?>("StatusStore")
                        .HasColumnType("integer");

                    b.Property<string>("StoreAddress")
                        .HasColumnType("character varying(250)")
                        .HasMaxLength(250);

                    b.Property<string>("StoreName")
                        .HasColumnType("character varying(250)")
                        .HasMaxLength(250);

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<double?>("lat")
                        .HasColumnType("double precision");

                    b.Property<double?>("lon")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("CategoryStoreId");

                    b.HasIndex("ProviderId");

                    b.ToTable("Stores","Store");
                });

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Stores.StoreOption", b =>
                {
                    b.Property<string>("OptionId")
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.Property<string>("StoreId")
                        .HasColumnType("character varying(50)");

                    b.Property<string>("StoreOptionDescription")
                        .HasColumnType("character varying(250)")
                        .HasMaxLength(250);

                    b.Property<string>("StoreOptionName")
                        .HasColumnType("character varying(250)")
                        .HasMaxLength(250);

                    b.HasKey("OptionId");

                    b.HasIndex("StoreId");

                    b.ToTable("StoreOptions","Store");
                });

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Stores.StoreTable", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.Property<int?>("Location")
                        .HasColumnType("integer");

                    b.Property<int?>("LocationUnit")
                        .HasColumnType("integer");

                    b.Property<string>("StoreId")
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.Property<string>("TableCode")
                        .HasColumnType("character varying(250)")
                        .HasMaxLength(250);

                    b.Property<string>("TableName")
                        .HasColumnType("character varying(250)")
                        .HasMaxLength(250);

                    b.Property<int?>("TableStatus")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("StoreId");

                    b.ToTable("StoreTables","Store");
                });

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Orders.OrderDetail", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.Property<int?>("Amount")
                        .HasColumnType("integer");

                    b.Property<string>("Note")
                        .HasColumnType("character varying(250)")
                        .HasMaxLength(250);

                    b.Property<string>("OrderId")
                        .HasColumnType("character varying(50)");

                    b.Property<string>("ProductId")
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.Property<int?>("Quality")
                        .HasColumnType("integer");

                    b.Property<int?>("Rate")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderDetails","Order");
                });

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Account.ExternalSignIn", b =>
                {
                    b.HasOne("StoreOrder.WebApplication.Data.Models.Account.User", "User")
                        .WithMany("UseExternalSignIns")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Account.RoleToPermission", b =>
                {
                    b.HasOne("StoreOrder.WebApplication.Data.Models.Account.Permission", "Permission")
                        .WithMany("RoleToPermissions")
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StoreOrder.WebApplication.Data.Models.Account.Role", "Role")
                        .WithMany("RoleToPermissions")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Account.User", b =>
                {
                    b.HasOne("StoreOrder.WebApplication.Data.Models.Stores.Store", "Store")
                        .WithMany("Users")
                        .HasForeignKey("StoreId");
                });

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Account.UserDevice", b =>
                {
                    b.HasOne("StoreOrder.WebApplication.Data.Models.Account.User", "CurrentUser")
                        .WithMany("UserDevices")
                        .HasForeignKey("CurrentUserId");
                });

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Account.UserLogin", b =>
                {
                    b.HasOne("StoreOrder.WebApplication.Data.Models.Account.User", "User")
                        .WithMany("UserLogins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Account.UserToRole", b =>
                {
                    b.HasOne("StoreOrder.WebApplication.Data.Models.Account.Role", "Role")
                        .WithMany("UserToRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StoreOrder.WebApplication.Data.Models.Account.User", "User")
                        .WithMany("UserToRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Location.Provider", b =>
                {
                    b.HasOne("StoreOrder.WebApplication.Data.Models.Location.Provider", "ParentProvider")
                        .WithMany("Childs")
                        .HasForeignKey("ParentId");
                });

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Orders.Order", b =>
                {
                    b.HasOne("StoreOrder.WebApplication.Data.Models.Stores.StoreTable", "StoreTable")
                        .WithMany("Orders")
                        .HasForeignKey("TableId");

                    b.HasOne("StoreOrder.WebApplication.Data.Models.Account.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Orders.UserCookingOrder", b =>
                {
                    b.HasOne("StoreOrder.WebApplication.Data.Models.Orders.Order", "Order")
                        .WithMany("UserCookingOrders")
                        .HasForeignKey("OrderId");

                    b.HasOne("StoreOrder.WebApplication.Data.Models.Account.User", "User")
                        .WithMany("UserCookingOrders")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Products.CategoryProduct", b =>
                {
                    b.HasOne("StoreOrder.WebApplication.Data.Models.Products.CategoryProduct", "ParentCategory")
                        .WithMany("Childs")
                        .HasForeignKey("ParentId");
                });

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Products.Product", b =>
                {
                    b.HasOne("StoreOrder.WebApplication.Data.Models.Products.CategoryProduct", "CategoryProduct")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId");

                    b.HasOne("StoreOrder.WebApplication.Data.Models.Account.User", "User")
                        .WithMany("Products")
                        .HasForeignKey("CreateByUserId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("StoreOrder.WebApplication.Data.Models.Products.Product", "ParentProduct")
                        .WithMany("ChildProducts")
                        .HasForeignKey("ProductParentId");

                    b.HasOne("StoreOrder.WebApplication.Data.Models.Stores.Store", "Store")
                        .WithMany("Products")
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Products.ProductDetail", b =>
                {
                    b.HasOne("StoreOrder.WebApplication.Data.Models.Products.Product", "Product")
                        .WithOne("ProductDetail")
                        .HasForeignKey("StoreOrder.WebApplication.Data.Models.Products.ProductDetail", "ProductId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Products.ProductImage", b =>
                {
                    b.HasOne("StoreOrder.WebApplication.Data.Models.Products.ProductDetail", "ProductDetail")
                        .WithMany("ProductImages")
                        .HasForeignKey("ProductDetailId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();
                });

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Products.ProductOption", b =>
                {
                    b.HasOne("StoreOrder.WebApplication.Data.Models.Products.Product", "Product")
                        .WithMany("ProductOptions")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Products.ProductOptionValue", b =>
                {
                    b.HasOne("StoreOrder.WebApplication.Data.Models.Products.ProductOption", "ProductOption")
                        .WithMany("ProductOptionValues")
                        .HasForeignKey("ProductId", "OptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Products.ProductSKU", b =>
                {
                    b.HasOne("StoreOrder.WebApplication.Data.Models.Products.Product", "Product")
                        .WithMany("ProductSKUs")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Products.ProductSKUValue", b =>
                {
                    b.HasOne("StoreOrder.WebApplication.Data.Models.Products.ProductOption", "ProductOption")
                        .WithMany("ProductSKUValues")
                        .HasForeignKey("ProductId", "OptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StoreOrder.WebApplication.Data.Models.Products.ProductSKU", null)
                        .WithMany("ProductSKUValues")
                        .HasForeignKey("ProductId", "SkuId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("StoreOrder.WebApplication.Data.Models.Products.ProductSKU", "ProductSKU")
                        .WithMany()
                        .HasForeignKey("ProductSKUProductId", "ProductSKUSkuId");

                    b.HasOne("StoreOrder.WebApplication.Data.Models.Products.ProductOptionValue", "ProductOptionValue")
                        .WithMany("ProductSKUValues")
                        .HasForeignKey("ProductId", "OptionId", "ValueId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Stores.Store", b =>
                {
                    b.HasOne("StoreOrder.WebApplication.Data.Models.Stores.CategoryStore", "CategoryStore")
                        .WithMany("Stores")
                        .HasForeignKey("CategoryStoreId");

                    b.HasOne("StoreOrder.WebApplication.Data.Models.Location.Provider", "Provider")
                        .WithMany("Stores")
                        .HasForeignKey("ProviderId");
                });

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Stores.StoreOption", b =>
                {
                    b.HasOne("StoreOrder.WebApplication.Data.Models.Stores.Store", "Store")
                        .WithMany("StoreOptions")
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Stores.StoreTable", b =>
                {
                    b.HasOne("StoreOrder.WebApplication.Data.Models.Stores.Store", "Store")
                        .WithMany("StoreTables")
                        .HasForeignKey("StoreId");
                });

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Orders.OrderDetail", b =>
                {
                    b.HasOne("StoreOrder.WebApplication.Data.Models.Orders.Order", "Order")
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderId");
                });
#pragma warning restore 612, 618
        }
    }
}
