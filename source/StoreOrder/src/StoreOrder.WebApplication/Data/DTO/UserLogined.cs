using Newtonsoft.Json;
using System;

namespace StoreOrder.WebApplication.Data.DTO
{
    public class UserLogined
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("expires_in")]
        public DateTimeOffset? ExpiresIn { get; set; }
        [JsonProperty("isLoggedIn")]
        public bool? IsLoggedIn { get; set; }
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
    }
}
