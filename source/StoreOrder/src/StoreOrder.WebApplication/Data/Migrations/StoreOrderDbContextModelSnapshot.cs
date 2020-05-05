﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using StoreOrder.WebApplication.Data;

namespace StoreOrder.WebApplication.Data.Migrations
{
    [DbContext(typeof(StoreOrderDbContext))]
    partial class StoreOrderDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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
                        .HasColumnType("character varying(500)")
                        .HasMaxLength(500);

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

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.HasKey("Id");

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

                    b.Property<DateTime?>("DateDone")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("DateRecieved")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("DurationTime")
                        .HasColumnType("integer");

                    b.Property<string>("OrderId")
                        .HasColumnType("character varying(50)");

                    b.Property<int?>("StatusCooking")
                        .HasColumnType("integer");

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

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Products.Option", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.Property<string>("OptionName")
                        .HasColumnType("character varying(250)")
                        .HasMaxLength(250);

                    b.Property<string>("ProductId")
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Options","Product");
                });

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Products.OptionValue", b =>
                {
                    b.Property<string>("ValueId")
                        .HasColumnType("text");

                    b.Property<string>("OptionId")
                        .HasColumnType("character varying(50)");

                    b.Property<string>("ProductId")
                        .HasColumnType("text");

                    b.Property<string>("ValueName")
                        .HasColumnType("character varying(250)")
                        .HasMaxLength(250);

                    b.HasKey("ValueId", "OptionId", "ProductId");

                    b.HasIndex("OptionId");

                    b.ToTable("OptionValues","Product");
                });

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Products.Product", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.Property<string>("CategoryId")
                        .HasColumnType("character varying(50)");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("ProductName")
                        .HasColumnType("character varying(250)")
                        .HasMaxLength(250);

                    b.Property<string>("ProductParentId")
                        .HasColumnType("character varying(50)");

                    b.Property<decimal?>("ProductPrice")
                        .HasColumnType("numeric");

                    b.Property<int?>("ProductStatus")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("ProductParentId");

                    b.ToTable("Products","Product");
                });

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Products.ProductSku", b =>
                {
                    b.Property<string>("SkuId")
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.Property<decimal?>("Price")
                        .HasColumnType("numeric");

                    b.Property<string>("ProductId")
                        .HasColumnType("character varying(50)");

                    b.Property<string>("SkuName")
                        .HasColumnType("character varying(250)")
                        .HasMaxLength(250);

                    b.HasKey("SkuId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductSkus","Product");
                });

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Products.SkuValue", b =>
                {
                    b.Property<string>("SkuId")
                        .HasColumnType("text");

                    b.Property<string>("ProductId")
                        .HasColumnType("text");

                    b.Property<string>("OptionId")
                        .HasColumnType("text");

                    b.Property<string>("ValueId")
                        .HasColumnType("text");

                    b.HasKey("SkuId", "ProductId", "OptionId");

                    b.ToTable("SkuValues","Product");
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
                        .HasColumnType("text");

                    b.Property<string>("CategoryStoreId")
                        .HasColumnType("character varying(50)");

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

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<double?>("lat")
                        .HasColumnType("double precision");

                    b.Property<double?>("lon")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("CategoryStoreId");

                    b.HasIndex("ProviderId");

                    b.ToTable("Stores","Store");
                });

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Stores.StoreTable", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.Property<int?>("Location")
                        .HasColumnType("integer");

                    b.Property<string>("StoreId")
                        .HasColumnType("text");

                    b.Property<string>("TableCode")
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

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Account.UserDevice", b =>
                {
                    b.HasOne("StoreOrder.WebApplication.Data.Models.Account.User", "CurrentUser")
                        .WithMany("UserDevices")
                        .HasForeignKey("CurrentUserId");
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

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Products.Option", b =>
                {
                    b.HasOne("StoreOrder.WebApplication.Data.Models.Products.Product", "Product")
                        .WithMany("Options")
                        .HasForeignKey("ProductId");
                });

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Products.OptionValue", b =>
                {
                    b.HasOne("StoreOrder.WebApplication.Data.Models.Products.Option", "Option")
                        .WithMany("OptionValues")
                        .HasForeignKey("OptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Products.Product", b =>
                {
                    b.HasOne("StoreOrder.WebApplication.Data.Models.Products.CategoryProduct", "CategoryProduct")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId");

                    b.HasOne("StoreOrder.WebApplication.Data.Models.Products.Product", "ParentProduct")
                        .WithMany("ChildProducts")
                        .HasForeignKey("ProductParentId");
                });

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Products.ProductSku", b =>
                {
                    b.HasOne("StoreOrder.WebApplication.Data.Models.Products.Product", "Product")
                        .WithMany("ProductSkus")
                        .HasForeignKey("ProductId");
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

            modelBuilder.Entity("StoreOrder.WebApplication.Data.Models.Stores.StoreTable", b =>
                {
                    b.HasOne("StoreOrder.WebApplication.Data.Models.Stores.Store", "Store")
                        .WithMany("StoreHasTables")
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
