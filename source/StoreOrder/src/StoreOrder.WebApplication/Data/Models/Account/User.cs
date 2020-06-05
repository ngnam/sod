using StoreOrder.WebApplication.Data.Models.Orders;
using StoreOrder.WebApplication.Data.Models.Products;
using StoreOrder.WebApplication.Data.Models.Stores;
using System;
using System.Collections.Generic;

namespace StoreOrder.WebApplication.Data.Models.Account
{
    public class User
    {
        public User()
        {
            this.CreatedAt = DateTime.UtcNow;
            this.UserDevices = new HashSet<UserDevice>();
            this.UseExternalSignIns = new HashSet<ExternalSignIn>();
            this.UserToRoles = new HashSet<UserToRole>();
            this.Orders = new HashSet<Order>();
            this.Products = new HashSet<Product>();
            this.UserLogins = new HashSet<UserLogin>();
        }
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; } // unique
        public string UserName { get; set; } // unique
        public string SaltPassword { get; set; }
        public string HashPassword { get; set; }
        public int? CountLoginFailed { get; set; }
        public string OldPassword { get; set; }
        public int? IsActived { get; set; }
        public int? Age { get; set; }
        public DateTime? BirthDay { get; set; }
        public DateTime? CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? LastLogin { get; set; }
        public virtual ICollection<UserDevice> UserDevices { get; set; }
        public virtual ICollection<ExternalSignIn> UseExternalSignIns { get; set; }
        public virtual ICollection<UserToRole> UserToRoles { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<UserLogin> UserLogins { get; set; }
        public string StoreId { get; set; }
        public virtual Store Store { get; set; }
        public virtual UserDetail UserDetail { get; set; }
    }
}
