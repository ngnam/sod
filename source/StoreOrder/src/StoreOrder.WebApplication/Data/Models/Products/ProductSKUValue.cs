using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Data.Models.Products
{
    public class ProductSKUValue
    {
        public string ProductId { get; set; }

        public string SkuId { get; set; }

        public string OptionId { get; set; }
        public string ValueId { get; set; }

        public virtual ProductSKU ProductSKU { get; set; }
        public virtual ProductOption ProductOption { get; set; }
        public virtual ProductOptionValue ProductOptionValue { get; set; }
    }
}
