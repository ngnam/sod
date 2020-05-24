using Imgur.API;
using Imgur.API.Authentication.Impl;
using Imgur.API.Endpoints.Impl;
using Imgur.API.Models;
using Imgur.API.Models.Impl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StoreOrder.WebApplication.Authorization;
using StoreOrder.WebApplication.Controllers.ApiBase;
using StoreOrder.WebApplication.Data.MimeTypes;
using StoreOrder.WebApplication.Data.Models.FileUpload;
using StoreOrder.WebApplication.Data.Repositories.Interfaces;
using StoreOrder.WebApplication.Data.Wrappers;
using StoreOrder.WebApplication.Extensions;
using StoreOrder.WebApplication.Settings;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Controllers
{
    [Authorize]
    [ApiVersion("1")]
    public class UploadMediaController : ApiBaseController<UploadMediaController>
    {
        private readonly IWebHostEnvironment _env;
        protected readonly IConfiguration _configuration;

        public UploadMediaController(
           IWebHostEnvironment env,
           IConfiguration configuration,
           IAuthRepository authRepository,
           ILoggerFactory loggerFactory) : base(loggerFactory, authRepository)
        {
            _env = env;
            _configuration = configuration;
        }

        [HttpPost("upload-file/{type}"), MapToApiVersion("1")]
        [DisableRequestSizeLimit]
        [Authorize(Policy = Permissions.Upload.UploadFile)]
        public async Task<IActionResult> UploadFile(string type = "image")
        {
            await CheckIsSignoutedAsync();

            // Get file Upload from HttpRequest FormData
            FileModel postedFile = Request.File();
            // Check has file
            if (postedFile == null)
            {
                throw new ApiException("File không tồn tại, Vui lòng chọn lại file!", (int)HttpStatusCode.BadRequest);
            }

            // is upload image
            if (type == "image")
            {
                postedFile.Name = "image";
                // Extention image
                if (!AcceptedImageFormats.MineTypes.Contains(postedFile.ContentType.ToLower()))
                {
                    throw new ApiException($"File chỉ hỗ trợ định dạng {string.Join(",", AcceptedImageFormats.MineTypes)}", (int)HttpStatusCode.BadRequest);
                }
                // Photo FileSize Upto 10 MB
                if (postedFile.Length > 10 * 1024 * 1024)
                {
                    throw new ApiException("Dung lượng file ảnh không được vượt quá 10 MB!", (int)HttpStatusCode.BadRequest);
                }
            }
            // is upload video
            else if (type == "video")
            {
                postedFile.Name = "video";
                // Extention Video 
                if (!AcceptedVideoFormats.MineTypes.Contains(postedFile.ContentType.ToLower()))
                {
                    throw new ApiException($"File chỉ hỗ trợ định dạng {string.Join(",", AcceptedVideoFormats.MineTypes)}", (int)HttpStatusCode.BadRequest);
                }
                // Photo FileSize Upto 10 MB
                if (postedFile.Length > 200 * 1024 * 1024)
                {
                    throw new ApiException("Dung lượng file video không được vượt quá 200 MB!", (int)HttpStatusCode.BadRequest);
                }

            }
            // not support media type
            else
            {
                throw new ApiException("not support media type", (int)HttpStatusCode.BadRequest);
            }

            // Save postedFile to ImgUr API
            try
            {
                var client = new ImgurClient(this.imgUrUploadSettings.ClientID, this.imgUrUploadSettings.ClientSecret);
                // state=#access_token=d9cb9143c19c67696e26f4ac59b2f7424af7650c&expires_in=315360000&token_type=bearer&refresh_token=ddd479acc0fa1b91b099599f864ac0a9afa82801&account_username=ngnamB&account_id=96736173
                var token = new OAuth2Token(
                    accessToken: "d9cb9143c19c67696e26f4ac59b2f7424af7650c", 
                    refreshToken: "ddd479acc0fa1b91b099599f864ac0a9afa82801", 
                    tokenType: "bearer",
                    accountId: "96736173", 
                    accountUsername: "ngnamB", 
                    expiresIn: 315360000);
                //var endpoint = new OAuth2Endpoint(client);
                //var token = endpoint.GetTokenByRefreshTokenAsync("REFRESH_TOKEN");
                client.SetOAuth2Token(token);
                var albumEndpoint = new AlbumEndpoint(client);
                // https://imgur.com/a/Dsoqayj
                IAlbum album = await albumEndpoint.GetAlbumAsync("Dsoqayj");
                var imageEndpoint = new ImageEndpoint(client);
                IImage imageOrVideo = await imageEndpoint.UploadImageBinaryAsync(image: postedFile.Bytes, albumId: album.Id);
                //Debug.Write("Image retrieved. Image Url: " + image.Link);
                return Ok(imageOrVideo);
            }
            catch (ImgurException imgurEx)
            {
                _logger.LogError("UploadFile---", imgurEx);
#if !DEBUG
                throw new ApiException("An error occurred getting an image from Imgur.");
#else
                throw new ApiException(imgurEx);
#endif
            }

        }

        private string GetFilePath(string directory, string fileName)
        {
            return Path.Combine(directory, fileName);
        }

        private ImgUrUpload _imgUrUploadSettings;
        protected ImgUrUpload imgUrUploadSettings
        {
            get
            {
                if (_imgUrUploadSettings == null) this._imgUrUploadSettings = _configuration.GetSection("ImgUrUpload").Get<ImgUrUpload>();
                return this._imgUrUploadSettings;
            }
        }
    }
}