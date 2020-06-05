using System.ComponentModel.DataAnnotations;

namespace StoreOrder.WebApplication.Data.Models.Account
{
    public class UserDetail
    {
        public int Id { get; set; }
        public string ProvideId { get; set; }
        public string GAvartar { get; set; }
        [MaxLength(550)]
        public string Address1 { get; set; }
        [MaxLength(550)]
        public string Address2 { get; set; }
        [MaxLength(550)]
        public string Address3 { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}
