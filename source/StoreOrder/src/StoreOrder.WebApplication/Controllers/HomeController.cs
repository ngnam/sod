using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StoreOrder.WebApplication.Data;
using StoreOrder.WebApplication.Data.Models.Account;

namespace StoreOrder.WebApplication.Controllers
{
    [Route("[controller]")]
    [ApiVersion("1")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly StoreOrderDbContext _context;
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<HomeController> _logger;

        public HomeController(
            StoreOrderDbContext context,
            ILogger<HomeController> logger)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet(""), MapToApiVersion("1")]
        public async Task<IActionResult> Get()
        {
            return Ok(await ApiResult<User>.CreateAsync(_context.Users, 0, 10));
        }

        [HttpGet(""), MapToApiVersion("2")]
        public IActionResult GetV2()
        {
            return Ok(Summaries);
        }
    }
}
