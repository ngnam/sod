using System.Collections.Generic;

namespace StoreOrder.WebApplication.Data.DTO
{
    public class OrderWithProductDTO
    {
        public OrderWithProductDTO()
        {
            this.Products = new List<ViewProductOptionDTO>();
        }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int TotalCount { get; set; }
        public List<ViewProductOptionDTO> Products { get; set; }
    }

    public class ViewProductOptionDTO
    {
        public ViewProductOptionDTO()
        {
            this.OptionId_OptionValueIds = new string[] { };
        }
        public string TableId { get; set; }
        public string TableName { get; set; }
        public string[] OptionId_OptionValueIds { get; set; }
        public string OptionDescription { get; set; }
        public string OrderNote { get; set; }
        public int AmountFood { get; set; }
        public int ProductOrderStatus { get; set; }
    }
}
