using StoreOrder.WebApplication.Data.DTO.Captchas;
using StoreOrder.WebApplication.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StoreOrder.WebApplication.Data.DTO
{
    public class OrderProductDTO : CaptchaCodeModel
    {
        public OrderProductDTO()
        {
            this.Products = new List<OrderDetailDTO>();
            this.OrderId = "0";
            this.CreatedOn = null;
            this.UpdatedOn = null;
        }
        [Required]
        public string OrderId { get; set; }
        [Required]
        public string TableId { get; set; }
        public string TableName{ get; set; }
        public int OrderStatus { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public List<OrderDetailDTO> Products { get; set; }
    }

    public class OrderDetailDTO
    {
        public OrderDetailDTO()
        {
            this.Amount = 1;
            this.Status = (int)TypeOrderDetail.NewOrder;
            this.Price = 0;
            this.OptionId_OptionValueIds = new string[] {};
        }
        [Required]
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string[] OptionId_OptionValueIds { get; set; }
        [MaxLength(500)]
        public string OptionDescription { get; set; }
        public decimal Price { get; set; }
        [MaxLength(500)]
        public string Note { get; set; }
        public int? Amount { get; set; }
        public int? Status { get; set; }
        public string OrderDetailId { get; set; }
    }
}
