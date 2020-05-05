using System;
using System.Collections.Generic;
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
            this.CreatedAt = DateTime.UtcNow;
        }

        public string Id { get; set; }
        public string ProductName { get; set; }
        public decimal? ProductPrice { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int? ProductStatus { get; set; }
        public string ProductParentId { get; set; }
        public virtual Product ParentProduct { get; set; }
        public string CategoryId { get; set; }
        public virtual CategoryProduct CategoryProduct { get; set; }
        public virtual IList<Product> ChildProducts { get; set; }
        public virtual ICollection<Option> Options { get; set; }
        public virtual ICollection<ProductSku> ProductSkus { get; set; }
    }
}
