using Microsoft.EntityFrameworkCore;
using StoreOrder.WebApplication.Data.Models.Account;
using StoreOrder.WebApplication.Data.Models.Location;
using StoreOrder.WebApplication.Data.Models.Orders;
using StoreOrder.WebApplication.Data.Models.Products;
using StoreOrder.WebApplication.Data.Models.Stores;
using StoreOrder.WebApplication.Data.Orders;
using System.Reflection.Metadata;

namespace StoreOrder.WebApplication.Data
{
    public class StoreOrderDbContext : DbContext
    {
        public StoreOrderDbContext(DbContextOptions options) : base(options)
        {
        }

        #region Methods
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Map Entity names to DB Table names
            #region ConfigEntityUser
            // User
            modelBuilder.Entity<User>(c =>
            {
                c.ToTable("Users", "Account");
                c.HasKey(m => m.Id);
                c.Property(m => m.Id).HasMaxLength(255);
                c.Property(m => m.FirstName).HasMaxLength(50).IsRequired(false);
                c.Property(m => m.LastName).HasMaxLength(50).IsRequired(false);
                c.Property(m => m.PhoneNumber).HasMaxLength(11);
                c.Property(m => m.Email).HasMaxLength(50);
                c.HasIndex(m => new { m.Email, m.UserName }).IsUnique();
                c.Property(m => m.SaltPassword).HasColumnType("TEXT");
                c.Property(m => m.HashPassword).HasColumnType("TEXT");
                c.Property(m => m.OldPassword).HasColumnType("TEXT");
                c.HasMany(g => g.UserDevices).WithOne(c => c.CurrentUser).HasForeignKey(c => c.CurrentUserId).IsRequired(false);
                c.HasMany(g => g.UseExternalSignIns).WithOne(c => c.User).HasForeignKey(c => c.UserId).IsRequired(false);
                c.HasIndex(m => m.Email).IsUnique();
            });
            #endregion

            #region UserDevice
            // UserDevice
            modelBuilder.Entity<UserDevice>(c =>
            {
                c.ToTable("UserDevices", "Account");
                c.HasKey(m => m.Id);
                c.Property(m => m.Id).HasMaxLength(255);
                c.Property(m => m.CodeDevice).HasMaxLength(250);
            });
            #endregion

            #region UseExternalSignIn
            // UseExternalSignIn
            modelBuilder.Entity<ExternalSignIn>(c =>
            {
                c.ToTable("ExternalSignIns", "Account");
                c.HasKey(m => m.Id);
                c.Property(m => m.Id).HasMaxLength(255);
                c.Property(m => m.TokenLogin).HasColumnType("TEXT");
            });
            #endregion

            #region UserLogin
            modelBuilder.Entity<UserLogin>(c =>
            {
                c.ToTable("UserLogins", "Account");
                c.HasKey(m => m.Id);
                c.Property(m => m.Id).ValueGeneratedOnAdd();
                c.Property(m => m.TokenType).HasMaxLength(25);
                c.Property(m => m.NameIdentifier).HasMaxLength(50);
                c.Property(m => m.AccessToken).HasColumnType("TEXT");
                c.HasOne(m => m.User).WithMany(m => m.UserLogins).HasForeignKey(m => m.UserId).OnDelete(DeleteBehavior.Restrict);
            });
            #endregion

            // Role
            modelBuilder.Entity<Role>(c =>
            {
                c.ToTable("Roles", "Account");
                c.HasKey(m => m.Id);
                c.Property(m => m.Id).HasMaxLength(255);
                c.Property(m => m.RoleName).HasMaxLength(255);
                c.Property(m => m.Desc).HasMaxLength(255);
                c.Property(m => m.Code).HasMaxLength(255);
                c.HasIndex(e => e.RoleName).IsUnique();
            });

            // Permistion
            modelBuilder.Entity<Permission>(c =>
            {
                c.ToTable("Permissions", "Account");
                c.HasKey(m => m.Id);
                c.Property(m => m.Id).HasMaxLength(255);
                c.Property(m => m.PermissionName).HasMaxLength(255);
            });

            // UserToRole
            modelBuilder.Entity<UserToRole>()
                .ToTable("UserToRoles", "Account");
            modelBuilder.Entity<UserToRole>()
                .HasKey(c => new { c.RoleId, c.UserId });
            modelBuilder.Entity<UserToRole>()
                .HasOne(bc => bc.User)
                .WithMany(bc => bc.UserToRoles)
                .HasForeignKey(bc => bc.UserId);
            modelBuilder.Entity<UserToRole>()
                .HasOne(bc => bc.Role)
                .WithMany(c => c.UserToRoles)
                .HasForeignKey(bc => bc.RoleId);

            // RoleToPermisstion
            modelBuilder.Entity<RoleToPermission>()
                .ToTable("RoleToPermissions", "Account");
            modelBuilder.Entity<RoleToPermission>()
                .HasKey(c => new { c.RoleId, c.PermissionId });
            modelBuilder.Entity<RoleToPermission>()
                .HasOne(bc => bc.Role)
                .WithMany(bc => bc.RoleToPermissions)
                .HasForeignKey(bc => bc.RoleId);
            modelBuilder.Entity<RoleToPermission>()
                .HasOne(bc => bc.Permission)
                .WithMany(c => c.RoleToPermissions)
                .HasForeignKey(bc => bc.PermissionId);

            // Location Provider
            modelBuilder.Entity<Provider>(c =>
            {
                c.ToTable("Providers", "Location");
                c.HasKey(c => c.Id);
                c.Property(m => m.Id).HasMaxLength(50);
                c.Property(m => m.Name).HasMaxLength(250);
                c.Property(m => m.Slug).HasMaxLength(250);
                c.Property(m => m.Type).HasMaxLength(250);
                c.Property(m => m.NameWithType).HasMaxLength(250);
                c.Property(m => m.Path).HasMaxLength(250);
                c.Property(m => m.PathWithType).HasMaxLength(250);
                c.Property(m => m.Code).HasMaxLength(50);
                c.Property(m => m.ParentId).HasMaxLength(50).IsRequired(false);
                c.HasMany(m => m.Childs).WithOne(m => m.ParentProvider).HasForeignKey(m => m.ParentId);
            });

            // category Store
            modelBuilder.Entity<CategoryStore>(c =>
            {
                c.ToTable("CategoryStores", "Store");
                c.HasKey(m => m.Id);
                c.Property(m => m.Id).HasMaxLength(50);
                c.Property(m => m.Name).HasMaxLength(250);
                c.HasMany(m => m.Stores).WithOne(m => m.CategoryStore).HasForeignKey(m => m.CategoryStoreId);
            });

            // Store
            modelBuilder.Entity<Store>(c =>
            {
                c.ToTable("Stores", "Store");
                c.HasKey(m => m.Id);
                c.Property(m => m.Id).HasMaxLength(50);
                c.Property(m => m.StoreName).HasMaxLength(250);
                c.Property(m => m.StoreAddress).HasMaxLength(250).IsRequired(false);
                c.Property(m => m.ProviderId).HasMaxLength(50);
                c.Property(m => m.CreateByUserId).HasMaxLength(255);
                c.HasOne(m => m.Provider).WithMany(m => m.Stores).HasForeignKey(m => m.ProviderId);
                c.HasMany(m => m.StoreTables).WithOne(m => m.Store).HasForeignKey(m => m.StoreId);
                c.HasMany(m => m.Users).WithOne(m => m.Store).HasForeignKey(m => m.StoreId);
            });

            // Table
            modelBuilder.Entity<StoreTable>(c =>
            {
                c.ToTable("StoreTables", "Store");
                c.HasKey(m => m.Id);
                c.Property(m => m.Id).HasMaxLength(50);
                c.Property(m => m.TableName).HasMaxLength(250);
                c.Property(m => m.TableCode).HasMaxLength(250);
                c.Property(m => m.StoreId).HasMaxLength(50);
                c.HasMany(m => m.Orders).WithOne(m => m.StoreTable).HasForeignKey(m => m.TableId);
            });

            // CategoryProduct
            modelBuilder.Entity<CategoryProduct>(c =>
            {
                c.ToTable("CategoryProducts", "Product");
                c.HasKey(m => m.Id);
                c.Property(m => m.Id).HasMaxLength(50);
                c.Property(m => m.CategoryName).HasMaxLength(250);
                c.Property(m => m.Slug).HasMaxLength(250);
                c.Property(m => m.Code).HasMaxLength(250);
                c.HasMany(m => m.Childs).WithOne(m => m.ParentCategory).HasForeignKey(m => m.ParentId);
                c.HasMany(m => m.Products).WithOne(m => m.CategoryProduct).HasForeignKey(m => m.CategoryId);
                c.HasOne(m => m.Store).WithMany(m => m.CategoryProducts).HasForeignKey(m => m.StoreId);
            });

            modelBuilder.Entity<ProductSKUValue>(c =>
            {
                c.ToTable("ProductSKUValues", "Product");
                c.HasOne<ProductSKU>()
                    .WithMany(p => p.ProductSKUValues)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ProductSKU>(c =>
            {
                c.ToTable("ProductSKUs", "Product");
                c.HasKey(p => new { p.ProductId, p.SkuId });
                c.HasOne(p => p.Product)
                .WithMany(ps => ps.ProductSKUs)
                .HasForeignKey(x => x.ProductId);
                c.HasIndex(p => p.Sku);
                c.Property(p => p.SkuId);
            });


            modelBuilder.Entity<ProductSKUValue>(c =>
            {
                c.ToTable("ProductSKUValues", "Product");
                c.HasOne<ProductSKU>()
                    .WithMany(p => p.ProductSKUValues)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Restrict);
                c.HasKey(p => new { p.ProductId, p.SkuId, p.OptionId });
                c.HasOne(p => p.ProductOptionValue)
                    .WithMany(ps => ps.ProductSKUValues)
                    .HasForeignKey(x => new { x.ProductId, x.OptionId, x.ValueId })
                    .OnDelete(DeleteBehavior.Restrict);
            });


            modelBuilder.Entity<ProductOptionValue>(c =>
            {
                c.ToTable("ProductOptionValues", "Product");
                c.HasKey(p => new { p.ProductId, p.OptionId, p.ValueId });
                c.HasOne(p => p.ProductOption)
                    .WithMany(ps => ps.ProductOptionValues)
                    .HasForeignKey(x => new { x.ProductId, x.OptionId });
                c.Property(p => p.ValueId).HasMaxLength(256);
            });


            modelBuilder.Entity<ProductOption>(c =>
            {
                c.ToTable("ProductOptions", "Product");
                c.HasKey(p => new { p.ProductId, p.OptionId });
                c.HasOne(p => p.Product)
                    .WithMany(po => po.ProductOptions)
                    .HasForeignKey(x => new { x.ProductId })
                    .OnDelete(DeleteBehavior.Restrict);
                c.Property(p => p.OptionId).HasMaxLength(50);
            });

            modelBuilder.Entity<Product>(c =>
            {
                c.ToTable("Products", "Product");
                c.HasKey(m => m.Id);
                c.Property(m => m.Id).HasMaxLength(50);
                c.Property(m => m.ProductName).HasMaxLength(250);
                c.HasMany(m => m.ChildProducts).WithOne(m => m.ParentProduct).HasForeignKey(m => m.ProductParentId);
                c.HasOne(m => m.User).WithMany(m => m.Products).HasForeignKey(m => m.CreateByUserId).OnDelete(DeleteBehavior.SetNull);
                c.HasOne(m => m.Store).WithMany(m => m.Products).HasForeignKey(m => m.StoreId).OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<ProductDetail>(c =>
            {
                c.ToTable("ProductDetails", "Product");
                c.HasIndex(m => m.Id);
                c.Property(m => m.Id).ValueGeneratedOnAdd();
                c.Property(m => m.ShortDescription).HasMaxLength(250);
                c.Property(m => m.LongDescription).HasMaxLength(500);
                c.Property(m => m.ImageAlbumJson).HasColumnType("TEXT");
                c.Property(m => m.ImageThumb).HasMaxLength(500);
                c.Property(m => m.ImageOrigin).HasMaxLength(500);
                c.HasOne(m => m.Product).WithOne(mc => mc.ProductDetail)
                    .HasForeignKey<ProductDetail>(m => m.ProductId)
                    .OnDelete(DeleteBehavior.SetNull);
                c.HasMany(m => m.ProductImages)
                    .WithOne(m => m.ProductDetail)
                    .HasForeignKey(m => m.ProductDetailId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<ProductImage>(c => {
                c.ToTable("ProductImages", "Product");
                c.HasKey(m => m.Id);
                c.Property(m => m.Id).ValueGeneratedOnAdd();
                c.Property(m => m.ImageThumb).HasMaxLength(500);
                c.Property(m => m.ImageOrigin).HasMaxLength(500);
            });

            modelBuilder.Entity<StoreOption>(c =>
            {
                c.ToTable("StoreOptions", "Store");
                c.HasKey(m => m.OptionId);
                c.Property(m => m.OptionId).HasMaxLength(50);
                c.Property(m => m.StoreOptionName).HasMaxLength(250);
                c.Property(m => m.StoreOptionDescription).HasMaxLength(250);
                c.HasOne(m => m.Store).WithMany(m => m.StoreOptions).HasForeignKey(m => m.StoreId).OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Order>(c =>
            {
                c.ToTable("Orders", "Order");
                c.HasKey(m => m.Id);
                c.Property(m => m.Id).HasMaxLength(50);
                c.HasOne(m => m.User).WithMany(m => m.Orders).HasForeignKey(m => m.UserId);
                c.HasMany(m => m.OrderDetails).WithOne(m => m.Order).HasForeignKey(m => m.OrderId);
                c.HasMany(m => m.UserCookingOrders).WithOne(m => m.Order).HasForeignKey(m => m.OrderId);
            });

            modelBuilder.Entity<OrderDetail>(c =>
            {
                c.ToTable("OrderDetails", "Order");
                c.HasKey(m => m.Id);
                c.Property(m => m.Id).HasMaxLength(50);
                c.Property(m => m.ProductId).HasMaxLength(50);
                c.Property(m => m.Note).HasMaxLength(250);
            });

            modelBuilder.Entity<UserCookingOrder>(c =>
            {
                c.ToTable("UserCookingOrders", "Order");
                c.HasKey(m => m.Id);
                c.Property(m => m.Id).HasMaxLength(50);
                c.HasOne(m => m.User).WithMany(m => m.UserCookingOrders).HasForeignKey(m => m.UserId);
            });
        }
        #endregion Methods

        #region Properties
        // Account
        public DbSet<User> Users { get; set; }
        public DbSet<UserDevice> UserDevices { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserToRole> UserToRoles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RoleToPermission> RoleToPermissions { get; set; }
        public DbSet<ExternalSignIn> ExternalSignIns { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        // Location
        public DbSet<Provider> Providers { get; set; }
        // Order
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<UserCookingOrder> UserCookingOrders { get; set; }
        // Product
        public DbSet<CategoryProduct> CategoryProducts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductOption> ProductOptions { get; set; }
        public DbSet<ProductOptionValue> ProductOptionValues { get; set; }
        public DbSet<ProductSKU> ProductSKUs { get; set; }
        public DbSet<ProductSKUValue> ProductSKUValues { get; set; }
        public DbSet<ProductDetail> ProductDetails { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        // Store
        public DbSet<CategoryStore> CategoryStores { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<StoreTable> StoreTables { get; set; }
        public DbSet<StoreOption> StoreOptions { get; set; }
        #endregion
    }
}
