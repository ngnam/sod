using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StoreOrder.WebApplication.Data.DTO
{
    public class OrderProductDTO
    {
        //- id = id bàn
        //- productId = id món
        //- listOptionDetail = danh sách id các option
        //- note = ghi chú của món ăn
        //- amount = số lượng
        [Required]
        public string TableId { get; set; }
        [Required]
        public string ProductId { get; set; }
        public List<ProductOptionDetailDTO> ProductOptionDetailDTOs { get; set; }
        [Required]
        public string OrderNote { get; set; }
        [Required]
        public int? Amount { get; set; }
    }

    public class ProductOptionDetailDTO
    {
        public string ProductId { get; set; }
        [Required]
        public string OptionId { get; set; }
        [Required]
        public string OptionValueId { get; set; }
        [Required]
        public string SkuId { get; set; }
        [Required]
        public decimal Price { get; set; }
    }
}
