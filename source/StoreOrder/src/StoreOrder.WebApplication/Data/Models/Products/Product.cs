using StoreOrder.WebApplication.Data.Models.Account;
using StoreOrder.WebApplication.Data.Models.Stores;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Data.Models.Products
{
    //https://stackoverflow.com/questions/24923469/modeling-product-variants
    public enum ProductStatus
    {
        New,
        Deleted
    }

    public class Product
    {
        public Product()
        {
            this.CreateOn = DateTime.UtcNow;
            this.ChildProducts = new List<Product>();
            this.ProductSKUs = new HashSet<ProductSKU>();
            this.ProductOptions = new HashSet<ProductOption>();
        }

        public string Id { get; set; }
        [Required]
        public string ProductName { get; set; }
        [MaxLength(32)]
        public String UniversalProductCode { get; set; }
        public Decimal Height { get; set; }
        public Decimal Weight { get; set; }
        public Decimal NetWeight { get; set; }
        public Decimal Depth { get; set; }
        public DateTime? CreateOn { get; set; }
        public DateTime? UpdateOn { get; set; }
        public DateTime? DeleteOn { get; set; }
        public string ProductParentId { get; set; }
        public virtual Product ParentProduct { get; set; }
        public string CategoryId { get; set; }
        public virtual CategoryProduct CategoryProduct { get; set; }
        public virtual IList<Product> ChildProducts { get; set; }
        public string CreateByUserId { get; set; }
        public virtual User User { get; set; }
        public string StoreId { get; set; }
        public virtual Store Store { get; set; }
        public virtual ICollection<ProductSKU> ProductSKUs { get; set; }
        public virtual ICollection<ProductOption> ProductOptions { get; set; }
        public virtual ProductDetail ProductDetail { get; set; }

    }
}
