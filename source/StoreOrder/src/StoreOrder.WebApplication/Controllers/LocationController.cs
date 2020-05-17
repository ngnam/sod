using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StoreOrder.WebApplication.Data;
using StoreOrder.WebApplication.Data.DTO;
using StoreOrder.WebApplication.Data.Models.Location;
using StoreOrder.WebApplication.Data.Wrappers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Controllers
{
    [Produces("application/json", "application/problem+json")]
    [Route("[controller]")]
    [ApiController]
    [ApiVersion("1")]
    public class LocationController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<LocationController> _logger;
        private readonly StoreOrderDbContext _context;

        public LocationController(
             IWebHostEnvironment env,
             ILoggerFactory loggerFactory,
             StoreOrderDbContext context)
        {
            _env = env;
            _logger = loggerFactory.CreateLogger<LocationController>();
            _context = context;
        }

        [HttpGet("import/{fileJSON}"), MapToApiVersion("1")]
        [Authorize(Roles = RoleTypeHelper.RoleAdmin)]
        public async Task<IActionResult> ImportData(string fileJSON)
        {
            // read jSON file
            string FileJSONPath = Path.Combine(_env.WebRootPath, fileJSON);
            // check file exist
            if (!System.IO.File.Exists(FileJSONPath))
            {
                throw new ApiException("File path not exist", (int)HttpStatusCode.BadRequest);
            }

            // read File
            var JSON = System.IO.File.ReadAllText(FileJSONPath);
            IDictionary<string, ProviderDTO> jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject<IDictionary<string, ProviderDTO>>(JSON);
            List<Provider> providers = new List<Provider>();

            // convert dictionary to object
            foreach (var item in jsonObj)
            {
                Provider provider = new Provider
                {
                    Id = item.Key,
                    Name = item.Value.Name,
                    Code = item.Value.Code,
                    ParentId = item.Value.ParentId,
                    NameWithType = item.Value.NameWithType,
                    Path = item.Value.Path,
                    PathWithType = item.Value.PathWithType,
                    Slug = item.Value.Slug,
                    Type = item.Value.Type
                };
                providers.Add(provider);
            }

            await _context.Providers.AddRangeAsync(providers);

            // save to db
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                throw new ApiException(ex);
            }

            return Ok(1);
        }

        [HttpGet(""), MapToApiVersion("1")]
        public IActionResult GetProvinder(string code = null)
        {
            Dictionary<string, IEnumerable<ProviderDTO>> providersDictionary = new Dictionary<string, IEnumerable<ProviderDTO>>();
            List<ProviderDTO> lstProviders = new List<ProviderDTO>();
            if (!string.IsNullOrEmpty(code))
            {
                code = code.Trim();
                var providers = _context.Providers
                   .Where(p => p.ParentId == code)
                   .Select(x => new ProviderDTO
                   {
                       Name = x.Name,
                       Code = x.Code,
                       NameWithType = x.NameWithType,
                       ParentId = x.ParentId,
                       Path = x.Path,
                       Slug = x.Slug,
                       Type = x.Type,
                       PathWithType = x.PathWithType
                   });
                if (providers.Count() == 0)
                {
                    throw new ApiException("Không tìm thấy mã hành chính nào hợp lệ!", (int)HttpStatusCode.BadRequest);
                }
                else
                {
                    lstProviders = providers.ToList();
                }
            }
            else
            {
                lstProviders = _context.Providers
                   .Where(p => p.ParentId == null && p.Path == null && p.PathWithType == null && (p.Type.Equals("tinh") || p.Type.Equals("thanh-pho")))
                   .Select(x => new ProviderDTO
                   {
                       Name = x.Name,
                       Code = x.Code,
                       NameWithType = x.NameWithType,
                       ParentId = x.ParentId,
                       Path = x.Path,
                       Slug = x.Slug,
                       Type = x.Type,
                       PathWithType = x.PathWithType
                   }).ToList();
            }

            providersDictionary = lstProviders
                   .GroupBy(p => p.Code)
                   .ToDictionary(p => p.Key, p => p.Select(x => new ProviderDTO
                   {
                       Name = x.Name,
                       Code = x.Code,
                       NameWithType = x.NameWithType,
                       ParentId = x.ParentId,
                       Path = x.Path,
                       Slug = x.Slug,
                       Type = x.Type,
                       PathWithType = x.PathWithType
                   }));

            return Ok(providersDictionary);
        }
    }
}