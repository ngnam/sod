using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Data.Models.Products
{
    public class SkuValue
    {
        public string SkuId { get; set; }
        public string ProductId { get; set; }
        public string OptionId { get; set; }
        public string ValueId { get; set; }
    }
}
