using StoreOrder.WebApplication.Data.Models.Account;
using StoreOrder.WebApplication.Data.Models.Stores;
using StoreOrder.WebApplication.Data.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Data.Models.Orders
{
    public enum OrderStatus
    {
        Waiting,
        Cooking,
        Done
    }
    public class Order
    {
        public string Id { get; set; }
        public int? OrderStatus { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public string TableId { get; set; }
        public virtual StoreTable StoreTable { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<UserCookingOrder> UserCookingOrders { get; set; }

    }
}
