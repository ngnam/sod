using StoreOrder.WebApplication.Data.Models.Account;
using StoreOrder.WebApplication.Data.Models.Location;
using StoreOrder.WebApplication.Data.Models.Products;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Data.Models.Stores
{
    public class Store
    {
        public Store()
        {
            this.CreatedAt = DateTime.UtcNow;
            this.StoreTables = new HashSet<StoreTable>();
            this.Products = new HashSet<Product>();
            this.StoreOptions = new HashSet<StoreOption>();
            this.Users = new HashSet<User>();
            this.CategoryProducts = new HashSet<CategoryProduct>();
        }
        public string Id { get; set; }
        public string StoreName { get; set; }
        public string StoreAddress { get; set; }
        public double? lat { get; set; }
        public double? lon { get; set; }
        public int? StatusStore { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string ProviderId { get; set; } // nullable
        public virtual Provider Provider { get; set; }
        public string CreateByUserId { get; set; }
        public virtual ICollection<StoreTable> StoreTables { get; set; }
        public string CategoryStoreId { get; set; }
        public virtual CategoryStore CategoryStore { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<StoreOption> StoreOptions { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<CategoryProduct> CategoryProducts { get; set; }
    }
}
