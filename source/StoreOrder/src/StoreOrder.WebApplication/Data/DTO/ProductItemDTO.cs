using System;
using System.Collections.Generic;
using System.Linq;

namespace StoreOrder.WebApplication.Data.DTO
{
    public class ProductItemDTO
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public String UniversalProductCode { get; set; }
        public Decimal Height { get; set; }
        public Decimal Weight { get; set; }
        public Decimal NetWeight { get; set; }
        public Decimal Depth { get; set; }
        public string OptionId { get; set; }
        public string OptionName { get; set; }
        public string ValueId { get; set; }
        public string ValueName { get; set; }
        public string SkuId { get; set; }
        public String Sku { get; set; }
        public decimal Price { get; set; }
        public decimal? FixedPrice { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string ImageThumb { get; set; }
        public int? ImageWidthThumb { get; set; }
        public int? ImageHeightThumb { get; set; }
        public string ImageOrigin { get; set; }
    }

    public class ProductItemGroupByNameDTO
    {
        public ProductItemGroupByNameDTO()
        {
        }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal? FixedPrice { get; set; }
        public String UniversalProductCode { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string ImageThumb { get; set; }
        public int? ImageWidthThumb { get; set; }
        public int? ImageHeightThumb { get; set; }
        public IEnumerable<ProductOptionDTO> ProductOptionDTOs { get; set; }
    }

    public class ProductOptionDTO
    {
        public ProductOptionDTO()
        {
        }
        public string OptionId { get; set; }
        public string OptionName { get; set; }
        public IEnumerable<ProductOptionValueDTO> ProductOptionValueDTOs { get; set; }
    }

    public class ProductOptionValueDTO
    {
        public string ValueId { get; set; }
        public string ValueName { get; set; }
        public string SkuId { get; set; }
        public String Sku { get; set; }
        public decimal Price { get; set; }
    }

    public class ProductSkuDTO
    {
        public string SkuId { get; set; }
        public String Sku { get; set; }
        public decimal Price { get; set; }
    }
}
