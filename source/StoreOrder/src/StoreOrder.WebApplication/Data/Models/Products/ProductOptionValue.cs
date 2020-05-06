using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StoreOrder.WebApplication.Data.Models.Products
{
    public class ProductOptionValue
    {
        public ProductOptionValue()
        {
            this.ProductSKUValues = new HashSet<ProductSKUValue>();
        }
        public string ProductId { get; set; }

        public string ValueId { get; set; }
        public string OptionId { get; set; }
        [Required]
        [MaxLength(32)]
        public string ValueName { get; set; }

        public virtual ProductOption ProductOption { get; set; }
        public virtual ICollection<ProductSKUValue> ProductSKUValues { get; set; }
    }
}