using System.ComponentModel.DataAnnotations;

namespace StoreOrder.WebApplication.Data.DTO.Captchas
{
    public class CaptchaCodeModel
    {
        //[Required]
        //[StringLength(6)]
        public string CaptchaCode { get; set; }
    }
}
