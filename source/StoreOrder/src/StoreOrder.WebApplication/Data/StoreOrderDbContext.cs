using Microsoft.EntityFrameworkCore;
using StoreOrder.WebApplication.Data.Models.Account;
using StoreOrder.WebApplication.Data.Models.Location;
using StoreOrder.WebApplication.Data.Models.Orders;
using StoreOrder.WebApplication.Data.Models.Products;
using StoreOrder.WebApplication.Data.Models.Stores;
using StoreOrder.WebApplication.Data.Orders;

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
                c.Property(m => m.SaltPassword).HasMaxLength(255);
                c.Property(m => m.HashPassword).HasMaxLength(255);
                c.Property(m => m.OldPassword).HasMaxLength(255);
                c.HasMany(g => g.UserDevices).WithOne(c => c.CurrentUser).HasForeignKey(c => c.CurrentUserId).IsRequired(false);
                c.HasMany(g => g.UseExternalSignIns).WithOne(c => c.User).HasForeignKey(c => c.UserId).IsRequired(false);
            });
            #endregion

            #region UserDevice
            // UserDevice
            modelBuilder.Entity<UserDevice>(c => {
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
                c.Property(m => m.TokenLogin).HasMaxLength(500);
            });
            #endregion

            // Role
            modelBuilder.Entity<Role>(c => {
                c.ToTable("Roles", "Account");
                c.HasKey(m => m.Id);
                c.Property(m => m.Id).HasMaxLength(255);
                c.Property(m => m.RoleName).HasMaxLength(255);
                c.Property(m => m.Desc).HasMaxLength(255);
                c.Property(m => m.Code).HasMaxLength(255);
            });

            // Permistion
            modelBuilder.Entity<Permission>(c => {
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
            modelBuilder.Entity<Provider>(c => {
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
            modelBuilder.Entity<Store>(c => {
                c.ToTable("Stores", "Store");
                c.HasKey(m => m.Id);
                c.Property(m => m.StoreName).HasMaxLength(250);
                c.Property(m => m.StoreAddress).HasMaxLength(250).IsRequired(false);
                c.Property(m => m.ProviderId).HasMaxLength(50);
                c.HasOne(m => m.Provider).WithMany(m => m.Stores).HasForeignKey(m => m.ProviderId);
                c.HasMany(m => m.StoreHasTables).WithOne(m => m.Store).HasForeignKey(m => m.StoreId);
            });

            // Table
            modelBuilder.Entity<StoreTable>(c => {
                c.ToTable("StoreTables", "Store");
                c.HasKey(m => m.Id);
                c.Property(m => m.Id).HasMaxLength(50);
                c.Property(m => m.TableCode).HasMaxLength(250);
                c.HasMany(m => m.Orders).WithOne(m => m.StoreTable).HasForeignKey(m => m.TableId);
            });

            // CategoryProduct
            modelBuilder.Entity<CategoryProduct>(c => {
                c.ToTable("CategoryProducts", "Product");
                c.HasKey(m => m.Id);
                c.Property(m => m.Id).HasMaxLength(50);
                c.Property(m => m.CategoryName).HasMaxLength(250);
                c.Property(m => m.Slug).HasMaxLength(250);
                c.Property(m => m.Code).HasMaxLength(250);
                c.HasMany(m => m.Childs).WithOne(m => m.ParentCategory).HasForeignKey(m => m.ParentId);
                c.HasMany(m => m.Products).WithOne(m => m.CategoryProduct).HasForeignKey(m => m.CategoryId);
            });

            modelBuilder.Entity<Product>(c => {
                c.ToTable("Products", "Product");
                c.HasKey(m => m.Id);
                c.Property(m => m.Id).HasMaxLength(50);
                c.Property(m => m.ProductName).HasMaxLength(250);
                c.HasMany(m => m.ChildProducts).WithOne(m => m.ParentProduct).HasForeignKey(m => m.ProductParentId);
                c.HasMany(m => m.Options).WithOne(m => m.Product).HasForeignKey(m => m.ProductId);
                c.HasMany(m => m.ProductSkus).WithOne(m => m.Product).HasForeignKey(m => m.ProductId);
            });

            modelBuilder.Entity<ProductSku>(c => {
                c.ToTable("ProductSkus", "Product");
                c.HasKey(m => m.SkuId);
                c.Property(m => m.SkuId).HasMaxLength(50);
                c.Property(m => m.SkuName).HasMaxLength(250);
            });

            modelBuilder.Entity<SkuValue>(c => {
                c.ToTable("SkuValues", "Product");
                c.HasKey(m => new { m.SkuId, m.ProductId, m.OptionId });
            });

            modelBuilder.Entity<Option>(c => {
                c.ToTable("Options", "Product");
                c.HasKey(m => m.Id);
                c.Property(m => m.Id).HasMaxLength(50);
                c.Property(m => m.OptionName).HasMaxLength(250);
                c.HasMany(m => m.OptionValues).WithOne(m => m.Option).HasForeignKey(m => m.OptionId);
            });

            modelBuilder.Entity<OptionValue>(c => {
                c.ToTable("OptionValues", "Product");
                c.HasKey(m => new { m.ValueId, m.OptionId, m.ProductId });
                c.Property(m => m.ValueName).HasMaxLength(250);
            });

            modelBuilder.Entity<Order>(c => {
                c.ToTable("Orders", "Order");
                c.HasKey(m => m.Id);
                c.Property(m => m.Id).HasMaxLength(50);
                c.HasOne(m => m.User).WithMany(m => m.Orders).HasForeignKey(m => m.UserId);
                c.HasMany(m => m.OrderDetails).WithOne(m => m.Order).HasForeignKey(m => m.OrderId);
                c.HasMany(m => m.UserCookingOrders).WithOne(m => m.Order).HasForeignKey(m => m.OrderId);
            });
            
            modelBuilder.Entity<OrderDetail>(c => {
                c.ToTable("OrderDetails", "Order");
                c.HasKey(m => m.Id);
                c.Property(m => m.Id).HasMaxLength(50);
                c.Property(m => m.ProductId).HasMaxLength(50);
                c.Property(m => m.Note).HasMaxLength(250);
            });

            modelBuilder.Entity<UserCookingOrder>(c => {
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
        // Location
        public DbSet<Provider> Providers { get; set; }
        // Order
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<UserCookingOrder> UserCookingOrders { get; set; }
        // Product
        public DbSet<CategoryProduct> CategoryProducts { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<OptionValue> OptionValues { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductSku> ProductSkus { get; set; }
        public DbSet<SkuValue> SkuValues { get; set; }
        // Store
        public DbSet<CategoryStore> CategoryStores { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<StoreTable> StoreTables { get; set; }
        #endregion
    }
}
