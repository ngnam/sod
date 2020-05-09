using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Data.Models.Products
{
    public class ProductDetail
    {
        public ProductDetail()
        {
            this.ProductImages = new HashSet<ProductImage>();
        }
        public long Id { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string ImageThumb { get; set; }
        public int? ImageWidthThumb { get; set; }
        public int? ImageHeightThumb { get; set; }
        public string ImageOrigin { get; set; }
        public string ImageAlbumJson { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; }
        public string ProductId { get; set; }
        public virtual Product Product { get; set; }
    }

    public class ProductImage
    {
        public long Id { get; set; }
        public string ImageThumb { get; set; }
        public int? ImageWidthThumb { get; set; }
        public int? ImageHeightThumb { get; set; }
        public string ImageOrigin { get; set; }
        public long ProductDetailId { get; set; }
        public virtual ProductDetail ProductDetail {get; set;}
    }

}
