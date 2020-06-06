using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreOrder.WebApplication.Helpers;
using System.IO;

namespace StoreOrder.WebApplication.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ApiVersion("1")]
    [Authorize(Roles ="CCXXLL")]
    public class CaptChaController : ControllerBase
    {
        [HttpGet(""), MapToApiVersion("1")]
        public IActionResult Index()
        {
            int width = 200;
            int height = 72;
            var captchaCode = Captcha.GenerateCaptchaCode(6);
            var result = Captcha.GenerateCaptchaImage(width, height, captchaCode);
            HttpContext.Session.SetString("CaptchaCode", result.CaptchaCode);
            Stream s = new MemoryStream(result.CaptchaByteData);
            return new FileStreamResult(s, "image/png");
        }
    }
}
