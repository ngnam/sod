
using Newtonsoft.Json;

namespace Imgur.API.Models.Impl
{
    /// <summary>An error returned after an Endpoint request.</summary>
    internal class ImgurError : IImgurError
    {
        /// <summary>A object description of the error.</summary>
        [JsonProperty("error")]
        public virtual Error Error { get; set; }

        /// <summary>The HttpMethod that was used to send the request.</summary>
        public virtual string Method { get; set; }

        /// <summary>The request Uri that the error came from.</summary>
        public virtual string Request { get; set; }
    }

    internal class Error
    {
        [JsonProperty("code")]
        public int Code { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("exception")]
        public string[] Exception { get; set; }
    }
}
