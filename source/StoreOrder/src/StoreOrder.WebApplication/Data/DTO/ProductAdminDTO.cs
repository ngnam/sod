using System;

namespace StoreOrder.WebApplication.Data.DTO
{
    public class ProductAdminDTO
    {
        public string Id { get; set; }
        public string ProductName { get; set; }
        public String UniversalProductCode { get; set; }
        public Decimal Height { get; set; }
        public Decimal Weight { get; set; }
        public Decimal NetWeight { get; set; }
        public Decimal Depth { get; set; }
        public DateTime? CreateOn { get; set; }
        public DateTime? UpdateOn { get; set; }
        public DateTime? DeleteOn { get; set; }
        public string CategoryId { get; set; }
        public string CreateByUserId { get; set; }
    }
}
