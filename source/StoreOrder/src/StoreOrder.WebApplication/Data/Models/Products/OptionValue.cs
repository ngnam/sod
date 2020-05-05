using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Data.Models.Products
{
    public class OptionValue
    {
        public string ValueId { get; set; }
        public string ValueName { get; set; }
        public string ProductId { get; set; }
        public string OptionId { get; set; }
        public virtual Option Option { get; set; }
    }
}
