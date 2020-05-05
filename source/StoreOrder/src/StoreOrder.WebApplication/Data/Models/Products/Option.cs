using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Data.Models.Products
{
    public class Option
    {
        public string Id { get; set; }
        public string OptionName { get; set; }
        public string ProductId { get; set; }
        public virtual Product Product { get; set; }
        public virtual ICollection<OptionValue> OptionValues { get; set; }
    }
}
