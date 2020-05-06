using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StoreOrder.WebApplication.Data.Models.Products
{
    public class ProductOption
    {
        public ProductOption()
        {
            this.ProductSKUValues = new HashSet<ProductSKUValue>();
            this.ProductOptionValues = new HashSet<ProductOptionValue>();
        }
        public string ProductId { get; set; }
        public string OptionId { get; set; }
        [Required]
        [MaxLength(40)]
        public string OptionName { get; set; }
        public virtual Product Product { get; set; }

        public virtual ICollection<ProductSKUValue> ProductSKUValues { get; set; }
        public virtual ICollection<ProductOptionValue> ProductOptionValues { get; set; }
    }
}