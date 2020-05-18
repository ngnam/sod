using StoreOrder.WebApplication.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StoreOrder.WebApplication.Data.DTO
{
    public class OrderProductDTO
    {
        public OrderProductDTO()
        {
            this.Products = new List<ProductOptionOrderDTO>();
            this.OrderId = "0";
            this.CreatedOn = DateTime.UtcNow;
            if (!this.OrderId.Equals("0"))
            {
                this.UpdatedOn = DateTime.UtcNow;
            }
        }
        [Required]
        public string OrderId { get; set; }
        [Required]
        public string TableId { get; set; }
        public string TableName{ get; set; }
        public int OrderStatus { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public List<ProductOptionOrderDTO> Products { get; set; }
    }

    public class ProductOptionOrderDTO
    {
        public ProductOptionOrderDTO()
        {
            this.AmountFood = 1;
            this.ProductOrderStatus = (int)TypeProductOrderWithTable.NEW;
            this.Price = 0;
            this.OptionId_OptionValueIds = new string[] {};
        }
        [Required]
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string[] OptionId_OptionValueIds { get; set; }
        public string OptionDescription { get; set; }
        public decimal Price { get; set; }
        public string OrderNote { get; set; }
        public int AmountFood { get; set; }
        public int ProductOrderStatus { get; set; }
    }
}
