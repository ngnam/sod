using System.ComponentModel.DataAnnotations;

namespace StoreOrder.WebApplication.Data.DTO
{
    public class RegistrationUserDTO
    {
        [Required]
        public string RoleId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Username { get; set; }
        public int? Gender { get; set; }
        public int? Age { get; set; }
    }
}
