using Microsoft.AspNetCore.Http;
using StoreOrder.WebApplication.Data.Models.FileUpload;
using System.IO;

namespace StoreOrder.WebApplication.Extensions
{
    public static class HttpRequestExtensions
    {
        /// <summary>
        /// single file
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static FileModel File(this HttpRequest request)
        {
            try
            {
                if (request.Form.Files.Count == 0) return null;
                IFormFile formFile = request.Form.Files[0];
                if (formFile == null) return null;

                var file = new FileModel()
                {
                    ContentType = formFile.ContentType,
                    FileName = formFile.FileName,
                    Length = formFile.Length,
                    Name = formFile.Name
                };
                using (var ms = new MemoryStream())
                {
                    formFile.CopyTo(ms);
                    file.Bytes = ms.ToArray();
                }
                return file;
            }
            catch
            {
                return null;
            }
        }
    }
}
