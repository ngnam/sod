using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Data.DTO
{
    public class StoreTableDTO
    {
        public string Id { get; set; }
        public string TableName { get; set; }
        public string TableCode { get; set; }
        public int? Location { get; set; }
        public int? LocationUnit { get; set; }
        public int? TableStatus { get; set; }
        public int? TableOrder { get; set; }
        public string StoreId { get; set; }
    }

    public class StoreTableDTOGroupByLocalionDTO
    {
        public StoreTableDTOGroupByLocalionDTO()
        {
        }
        public int? location { get; set; }
        public IEnumerable<StoreTableDTO> lstTable { get; set; }
    }

}
