using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Data.DTO
{
    public class CustomerLoginDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        public int Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Age { get; set; }
        public DateTime? BirthDay { get; set; }
        [Required]
        public int TypeLogin { get; set; }
        public string TokenLogin { get; set; }
        [Required]
        [MaxLength(50)]
        public string AppId { get; set; } 
    }
}
