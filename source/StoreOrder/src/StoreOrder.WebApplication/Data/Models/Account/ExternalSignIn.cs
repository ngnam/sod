using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Data.Models.Account
{
    public enum TypeLogin
    {
        GOOGLE, 
        FACEBOOK,
        QACODE,
        ONTIMEPASSWORD,
        SMSCODE
    }
    public class ExternalSignIn
    {
        public ExternalSignIn()
        {
        }
        public string Id { get; set; }
        public int? TypeLogin { get; set; }
        public int? IsVerified { get; set; }
        public DateTime? LastLogin { get; set; }
        public string TokenLogin { get; set; }
        public int? TimeLifeToken { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
