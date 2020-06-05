using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Data.Models.Stores
{
    public class CategoryStore
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string SlugCode { get; set; }

        public ICollection<Store> Stores { get; set; }
    }
}
