using StoreOrder.WebApplication.Data.Models.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Data.Models.Location
{
    public class Provider
    {
        public string Id { get; set; } 
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Type { get; set; }
        public string NameWithType { get; set; }
        public string Code { get; set; } // Id
        public string Path { get; set; } // nullable
        public string PathWithType { get; set; } // nullable
        public string ParentId { get; set; } // nullable
        public virtual Provider ParentProvider { get; set; }
        public virtual ICollection<Store> Stores { get; set; }
        public virtual ICollection<Provider> Childs { get; set; }
    }
}
