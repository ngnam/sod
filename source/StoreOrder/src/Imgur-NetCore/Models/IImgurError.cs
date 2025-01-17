﻿
namespace Imgur.API.Models
{
    /// <summary>An error returned after an Endpoint request.</summary>
    public interface IImgurError
    {
        /// <summary>The HttpMethod that was used to send the request.</summary>
        string Method { get; set; }

        /// <summary>The request Uri that the error came from.</summary>
        string Request { get; set; }
    }
}
