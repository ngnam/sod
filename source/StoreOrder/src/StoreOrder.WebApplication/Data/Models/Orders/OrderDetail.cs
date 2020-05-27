using StoreOrder.WebApplication.Data.Models.Orders;
using System.ComponentModel.DataAnnotations;

namespace StoreOrder.WebApplication.Data.Orders
{
    public class OrderDetail
    {
        public string Id { get; set; }
        [MaxLength(50)]
        public string ProductId { get; set; }
        [MaxLength(250)]
        public string ProductName { get; set; }
        public string[] OptionId_OptionValueIds { get; set; }
        [MaxLength(500)]
        public string OptionDescription { get; set; }
        public decimal Price { get; set; }
        public int? Amount { get; set; }
        public int? Rate { get; set; }
        public int? Status { get; set; }
        [MaxLength(500)]
        public string Note { get; set; }
        public string OrderId { get; set; }
        public Order Order { get; set; }
    }
}
