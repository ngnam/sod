namespace StoreOrder.WebApplication.Data.DTO
{
    public class OrderWithTableDTO
    {
        public string TableId { get; set; }
        public string TableName { get; set; }
        public int? TotalFoods { get; set; }
        public int? TableStatus { get; set; }
    }
}
