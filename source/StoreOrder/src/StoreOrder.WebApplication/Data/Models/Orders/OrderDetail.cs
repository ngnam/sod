using StoreOrder.WebApplication.Data.Models.Orders;
using StoreOrder.WebApplication.Data.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Data.Orders
{
    public class OrderDetail
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public int? Quality { get; set; }
        public int? Amount { get; set; }
        public int? Rate { get; set; }
        public string Note { get; set; }
        public string OrderId { get; set; }
        public Order Order { get; set; }
    }
}
