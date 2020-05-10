using System.ComponentModel.DataAnnotations;

namespace StoreOrder.WebApplication.Data.DTO
{
    public class RegistorStoreDTO
    {
        [Required]
        public string StoreName { get; set; }
        [Required]
        public string CatStoreId { get; set; }
        public string StoreAddress { get; set; }
        public string ProviderId { get; set; }
        public double? lat { get; set; }
        public double? lon { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
