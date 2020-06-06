using StoreOrder.WebApplication.Data.Models.Orders;
using System.Collections;
using System.Collections.Generic;

namespace StoreOrder.WebApplication.Data.Models.Stores
{
    public class StoreTable
    {
        public string Id { get; set; }
        public string TableName { get; set; }
        public string TableCode { get; set; }
        public int? Location { get; set; }
        public int? LocationUnit { get; set; }
        public int? TableStatus { get; set; }
        public int? TableOrder { get; set; }
        public string StoreId { get; set; }
        public virtual Store Store { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
