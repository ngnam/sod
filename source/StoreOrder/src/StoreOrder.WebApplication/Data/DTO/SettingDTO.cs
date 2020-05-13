using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace StoreOrder.WebApplication.Data.DTO
{
    public class SettingDTO
    {
        [Required]
        [MaxLength(64)]
        public string SettingKey { get; set; }
        [MaxLength(500)]
        public string SettingValue { get; set; }
        [MaxLength(500)]
        public string SettingDesc { get; set; }
    }

    public class SettingFromHeaderDTO
    {
        public string UserAgent { get; set; }
        public string AuthenticationId { get; set; }
        //Header["userAgent"]="Duc", 
        //Header["authenticationId"]="duc"
    }

    public class SettingItemDTO
    {
        public string SettingKey { get; set; }
        public string SettingValue { get; set; }
    }
}
