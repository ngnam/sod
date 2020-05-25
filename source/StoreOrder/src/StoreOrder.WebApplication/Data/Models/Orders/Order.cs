using StoreOrder.WebApplication.Data.Models.Account;
using StoreOrder.WebApplication.Data.Models.Stores;
using StoreOrder.WebApplication.Data.Orders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StoreOrder.WebApplication.Data.Models.Orders
{
    public class Order
    {
        public string Id { get; set; }
        public int? OrderStatus { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdateOn { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public string TableId { get; set; }
        [MaxLength(250)]
        public string TableName { get; set; }
        public virtual StoreTable StoreTable { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<UserCookingOrder> UserCookingOrders { get; set; }

    }
}
