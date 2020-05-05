using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Data.Models.Products
{
    public class ProductSku
    {
        public string SkuId { get; set; }
        public string ProductId { get; set; }
        public string SkuName { get; set; }
        public decimal? Price { get; set; }
        public virtual Product Product { get; set; }
    }
}
