using StoreOrder.WebApplication.Data.Models.Account;
using System;

namespace StoreOrder.WebApplication.Data.Models.Orders
{
    public class UserCookingOrder
    {
        public string Id { get; set; }
        public int? StatusCooking { get; set; }
        public DateTime? CreateOn { get; set; }
        public DateTime? UpdateOn { get; set; }
        public int? DurationTime { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public string OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
}
