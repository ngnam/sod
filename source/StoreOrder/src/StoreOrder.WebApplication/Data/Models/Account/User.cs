using Microsoft.AspNetCore.Builder;
using StoreOrder.WebApplication.Data.DTO;
using StoreOrder.WebApplication.Data.Models.Orders;
using StoreOrder.WebApplication.Data.Models.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Data.Models.Account
{
    public enum ActiveUser
    {
        Actived,
        Deactived
    }

    public enum VerifiUser
    {
        verified,
        notVerified
    }

    public enum ScreenId
    {
        userAdmin,
        userOrder,
        userCooker,
        userCrasher
    }

    public class User
    {
        public User()
        {
            this.CreatedAt = DateTime.UtcNow;
            this.UserDevices = new HashSet<UserDevice>();
            this.UseExternalSignIns = new HashSet<ExternalSignIn>();
            this.UserCookingOrders = new HashSet<UserCookingOrder>();
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
        public virtual ICollection<UserCookingOrder> UserCookingOrders { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<UserLogin> UserLogins { get; set; }
    }
}
