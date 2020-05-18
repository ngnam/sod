using System.Collections.Generic;

namespace StoreOrder.WebApplication.Data.DTO
{
    public class OrderWithTableDTO
    {
        public string TableId { get; set; }
        public string TableName { get; set; }
        public int TotalFoods { get; set; }
        public int TimeOrder { get; set; }
        public int TableStatus { get; set; }
    }

    public class OrderWithTableViewProductsDTO
    {
        public OrderWithTableViewProductsDTO()
        {
            this.Products = new List<ViewProductDTO>();
        }
        public string TableId { get; set; }
        public string TableName { get; set; }
        public List<ViewProductDTO> Products { get; set; }
    }

    public class ViewProductDTO
    {
        public ViewProductDTO()
        {
            this.OptionId_OptionValueIds = new string[] { };
        }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string[] OptionId_OptionValueIds { get; set; }
        public string OptionDescription { get; set; }
        public string OrderNote { get; set; }
        public int AmountFood { get; set; }
        public int ProductOrderStatus { get; set; }
    }
}
