using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StoreOrder.WebApplication.Authorization;
using StoreOrder.WebApplication.Controllers.ApiBase;
using StoreOrder.WebApplication.Data.DTO.Log;
using StoreOrder.WebApplication.Data.Repositories.Interfaces;
using StoreOrder.WebApplication.Services.Interfaces;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Controllers
{
    [Authorize]
    [ApiVersion("1")]
    public class LogController : ApiBaseController<LogController>
    {
        private readonly ILogService _logService;
        public LogController(
            ILogService logService,
            ILoggerFactory loggerFactory,
            IAuthRepository authRepository) : base(loggerFactory, authRepository)
        {
            _logService = logService;
        }

        [HttpGet("ErrorsLog"), MapToApiVersion("1")]
        [Authorize(Policy = Permissions.SysAdmin.ViewLogs)]
        public async Task<IActionResult> ErrorsLog(int? page, string search)
        {
            await CheckIsSignoutedAsync();

            var logs = await _logService.GetLogsAsync(search, page ?? 1);

            return Ok(new { data = logs, search = search });
        }

        [HttpPost("DeleteLogs"), MapToApiVersion("1")]
        [Authorize(Policy = Permissions.SysAdmin.DeleteLogs)]
        public async Task<IActionResult> DeleteLogs([FromBody]LogsDto log)
        {
            await CheckIsSignoutedAsync();

            if (ModelState.IsValid)
            {
                await _logService.DeleteLogsOlderThanAsync(log.DeleteOlderThan.Value);
            }

            return Ok(1);
        }

        [HttpPost("DeleteLogs"), MapToApiVersion("2")]
        [Authorize(Policy = Permissions.SysAdmin.DeleteLogs)]
        public async Task<IActionResult> DeleteLogsV2([FromForm]LogsDto log)
        {
            await CheckIsSignoutedAsync();

            if (ModelState.IsValid)
            {
                await _logService.DeleteLogsOlderThanAsync(log.DeleteOlderThan.Value);
            }

            return Ok(1);
        }

        [HttpGet("ClearAll"), MapToApiVersion("1")]
        [AllowAnonymous]
        public async Task<IActionResult> ClearAll()
        {
            var result = await _logService.ClearAllLogs();
            return Ok(result);
        }

        [HttpGet("Count"), MapToApiVersion("1")]
        [AllowAnonymous]
        public async Task<IActionResult> CountLogs()
        {
            var result = await _logService.CountLogs();
            return Ok(result);
        }
    }
}