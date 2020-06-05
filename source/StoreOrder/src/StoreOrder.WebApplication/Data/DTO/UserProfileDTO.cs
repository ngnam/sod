using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StoreOrder.WebApplication.Data.DTO
{
    public class UserProfileDTO
    {
        public UserProfileDTO()
        {
            this.GroupScreens = new HashSet<GroupScreen>();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string GAvartar { get; set; }
        public int? Gender { get; set; }
        public string StoreName { get; set; }
        public ICollection<GroupScreen> GroupScreens { get; set; }
        public DateTime? BirthDay { get; set; }
        public string ProviderId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
    }

    public class GroupScreen
    {
        public string GroupName { get; set; }
        public int ScreenId { get; set; }
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
        [Range(1954, 2015)]
        public int? YearOfBirth { get; set; }
        [Range(1, 12)]
        public int? MonthOfBirth { get; set; }
        [Range(1, 31)]
        public int? DayOfBirth { get; set; }
        public string ProviderId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
