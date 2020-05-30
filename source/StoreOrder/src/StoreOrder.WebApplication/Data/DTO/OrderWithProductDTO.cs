using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StoreOrder.WebApplication.Data.DTO
{
    public class OrderWithProductDTO
    {
        public OrderWithProductDTO()
        {
            this.Products = new List<ViewProductOptionDTO>();
            this.TotalCount = this.Products.Count;
        }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int? TotalCount { get; private set; }
        public List<ViewProductOptionDTO> Products { get; set; }
    }

    public class ViewProductOptionDTO
    {
        public ViewProductOptionDTO()
        {
            this.OptionId_OptionValueIds = new string[] { };
        }
        [Required]
        public string TableId { get; set; }
        public string TableName { get; set; }
        [Required]
        public string[] OptionId_OptionValueIds { get; set; }
        public string OptionDescription { get; set; }
        public string Note { get; set; }
        public int Amount { get; set; }
        public int Status { get; set; }
        [Required]
        public string OrderDetailId { get; set; }
    }
}
