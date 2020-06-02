using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StoreOrder.WebApplication.Data.DTO
{
    public class UserProfileDTO
    {
        public UserProfileDTO()
        {
            this.ScreenId = new HashSet<int>();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string GAvartar { get; set; }
        public ICollection<int> ScreenId { get; set; }
        public int? Gender { get; set; }
        public string StoreName { get; set; }
        public string GroupName { get; set; }
    }

    public class UserProfileUpdateDTO
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        public string GAvartar { get; set; }
        public int? Gender { get; set; }
        [MaxLength(11)]
        public string PhoneNumber { get; set; }
        [Required]
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
