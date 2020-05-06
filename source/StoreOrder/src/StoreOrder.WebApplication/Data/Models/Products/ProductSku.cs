using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Data.Models.Products
{
    public class ProductSKU
    {
        public ProductSKU()
        {
            this.ProductSKUValues = new HashSet<ProductSKUValue>();
        }
        public string ProductId { get; set; }
        public string SkuId { get; set; }

        [Required]
        [MaxLength(64)]
        public String Sku { get; set; }
        public decimal Price { get; set; }

        public Product Product { get; set; }
        public ICollection<ProductSKUValue> ProductSKUValues { get; set; }
    }
}
