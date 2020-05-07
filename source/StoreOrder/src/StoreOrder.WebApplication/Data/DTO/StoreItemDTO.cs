using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Data.DTO
{
    public class StoreItemDTO
    {
        public StoreItemDTO()
        {

        }
        public string Id { get; set; }
        public string StoreName { get; set; }
        public string StoreAddress { get; set; }
        public double? lat { get; set; }
        public double? lon { get; set; }
        public int? StatusStore { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
