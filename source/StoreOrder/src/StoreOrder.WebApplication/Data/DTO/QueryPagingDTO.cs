namespace StoreOrder.WebApplication.Data.DTO
{
    public class QueryPagingDTO
    {
        public int? pageIndex { get; set; } = 0;
        public int pageSize { get; set; } = 10;
        public string sortColumn { get; set; } = null;
        public string sortOrder { get; set; } = null;
        public string filterColumn { get; set; } = null;
        public string filterQuery { get; set; } = null;
    }
}
