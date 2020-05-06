using StoreOrder.WebApplication.Data.Models.Location;
using StoreOrder.WebApplication.Data.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Data.Models.Stores
{
    public enum StatusStore
    {
        Active, 
        Deactive,
        New
    }
    public class Store
    {
        public Store()
        {
            this.CreatedAt = DateTime.UtcNow;
            this.StoreTables = new HashSet<StoreTable>();
            this.Products = new HashSet<Product>();
            this.StoreOptions = new HashSet<StoreOption>();
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
        public string UserId { get; set; }
        public virtual ICollection<StoreTable> StoreTables { get; set; }
        public string CategoryStoreId { get; set; }
        public virtual CategoryStore CategoryStore { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<StoreOption> StoreOptions { get; set; }
    }
}
