using StoreOrder.WebApplication.Data.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Data.Models.Stores
{
    public class StoreOption
    {
        public string OptionId { get; set; }
        public string StoreOptionName { get; set; }
        public string StoreOptionDescription { get; set; }
        public string StoreId { get; set; }
        public virtual Store Store { get; set; }
    }
}
