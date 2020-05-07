using System;

namespace StoreOrder.WebApplication.Data.Models.Account
{
    public class UserLogin
    {
        public UserLogin()
        {
            this.LastLogin = DateTime.UtcNow;
        }
        public int Id { get; set; }
        public string AccessToken { get; set; }
        public DateTime? ExpiresIn { get; set; }
        public bool? IsLoggedIn { get; set; }
        public string TokenType { get; set; }
        public string UserId { get; set;}
        public string NameIdentifier { get; set; }
        public DateTime? LastLogin { get; set; }
        public virtual User User { get; set; }
    }
}