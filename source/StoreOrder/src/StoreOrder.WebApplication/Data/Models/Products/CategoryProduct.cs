using StoreOrder.WebApplication.Data.Models.Stores;
using System.Collections.Generic;

namespace StoreOrder.WebApplication.Data.Models.Products
{
    public class CategoryProduct
    {
        public CategoryProduct()
        {
            this.Childs = new List<CategoryProduct>();
            this.Products = new HashSet<Product>();
        }
        public string Id { get; set; }
        public string CategoryName { get; set; }
        public string Slug { get; set; }
        public string Code { get; set; }
        public string ParentId { get; set; }
        public virtual CategoryProduct ParentCategory { get; set; }
        public virtual IList<CategoryProduct> Childs { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public string StoreId { get; set; }
        public virtual Store Store { get; set; }
    }
}
