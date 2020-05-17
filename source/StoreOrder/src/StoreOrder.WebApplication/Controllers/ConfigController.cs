using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StoreOrder.WebApplication.Data;
using StoreOrder.WebApplication.Data.DTO;
using StoreOrder.WebApplication.Data.Enums;
using StoreOrder.WebApplication.Data.Models.Settings;
using StoreOrder.WebApplication.Data.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Controllers
{
    [Produces("application/json", "application/problem+json")]
    [Route("[controller]")]
    [ApiController]
    [ApiVersion("1")]
    [ApiVersion("2")]
    public class ConfigController : ControllerBase
    {
        private readonly ILogger<ConfigController> _logger;
        private readonly StoreOrderDbContext _context;

        public ConfigController(
             ILoggerFactory loggerFactory,
             StoreOrderDbContext context)
        {
            _logger = loggerFactory.CreateLogger<ConfigController>();
            _context = context;
        }

        [HttpGet("environment/{settingType}")]
        public async Task<IActionResult> GetSettings([FromHeader] string AuthenticationId, [FromHeader] string UserAgent, string settingType = "employee")
        {
            if (string.IsNullOrEmpty(AuthenticationId) || string.IsNullOrEmpty(UserAgent))
            {
                throw new ApiException("Bạn không có quyền truy cập.", (int)HttpStatusCode.Unauthorized);
            }
            int typeSetting = settingType == "employee" ? (int)TypeSetting.EMPLPLOYEE : (int)TypeSetting.CUSTOMER;
            Dictionary<string, string> settings = new Dictionary<string, string>();
            var result = await _context.Settings
                .Where(s => s.SettingType == typeSetting && 
                (s.CreatedBy == AuthenticationId || s.LastUpdatedBy == AuthenticationId))
                .Select(s => new SettingItemDTO { 
                    SettingKey = s.SettingKey,
                    SettingValue = s.SettingValueVarchar
                }).Distinct().ToListAsync();

            if (result != null && result.Count > 0)
            {
                settings = result.GroupBy(s => s.SettingKey).ToDictionary(s => s.Key, s => s.FirstOrDefault().SettingValue);
            }

            return Ok(settings);
        }


        [HttpPost("environment/{settingType}"), MapToApiVersion("1")]
        public async Task<IActionResult> UpdateSettings([FromBody] SettingDTO model, [FromHeader] string AuthenticationId, [FromHeader] string UserAgent, string settingType = "employee")
        {
            if (string.IsNullOrEmpty(AuthenticationId) || string.IsNullOrEmpty(UserAgent))
            {
                throw new ApiException("Bạn không có quyền truy cập.", (int)HttpStatusCode.Unauthorized);
            }
            if (ModelState.IsValid)
            {
                model.SettingKey = model.SettingKey.Trim();
                int typeSetting = settingType == "employee" ? (int)TypeSetting.EMPLPLOYEE : (int)TypeSetting.CUSTOMER;
                var settingExist = _context.Settings.FirstOrDefault(s => 
                        s.SettingKey.ToLower().Equals(model.SettingKey.Trim().ToLower()) && 
                        s.SettingType == typeSetting &&
                        s.CreatedBy.Equals(AuthenticationId));

                // if settingKey exist 
                if (settingExist != null) {
                    // update
                    _context.Entry(settingExist).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    settingExist.LastUpdated = DateTime.UtcNow;
                    settingExist.LastUpdatedBy = AuthenticationId;
                    settingExist.SettingValueVarchar = model.SettingValue;
                    settingExist.SettingDesc = model.SettingDesc;
                } else {
                    // other Create
                    Setting setting = new Setting();
                    setting.CreatedBy = AuthenticationId;
                    setting.LastUpdatedBy = AuthenticationId;
                    setting.SettingType = typeSetting;
                    setting.SettingKey = model.SettingKey;
                    setting.SettingValueVarchar = model.SettingValue;
                    setting.SettingDesc = model.SettingDesc;
                    _context.Settings.Add(setting);
                }

                // save to database
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (System.Exception ex)
                {
                    _logger.Log(LogLevel.Warning, "Có lỗi xảy ra khi update Settings", ex.Message);
#if DEBUG
                    throw new ApiException(ex);
#else
                    throw new ApiException("Có lỗi xảy ra khi update database");
#endif
                }

            }

            return Ok(1);
        }

        [HttpPost("environment/{settingType}"), MapToApiVersion("2")]
        public async Task<IActionResult> UpdateSettingsV2([FromForm] SettingDTO model, [FromHeader] string AuthenticationId, [FromHeader] string UserAgent, string settingType = "employee")
        {
            if (string.IsNullOrEmpty(AuthenticationId) || string.IsNullOrEmpty(UserAgent))
            {
                throw new ApiException("Bạn không có quyền truy cập.", (int)HttpStatusCode.Unauthorized);
            }
            if (ModelState.IsValid)
            {
                model.SettingKey = model.SettingKey.Trim();
                int typeSetting = settingType == "employee" ? (int)TypeSetting.EMPLPLOYEE : (int)TypeSetting.CUSTOMER;
                var settingExist = _context.Settings.FirstOrDefault(s =>
                        s.SettingKey.ToLower().Equals(model.SettingKey.Trim().ToLower()) &&
                        s.SettingType == typeSetting &&
                        s.CreatedBy.Equals(AuthenticationId));

                // if settingKey exist 
                if (settingExist != null)
                {
                    // update
                    _context.Entry(settingExist).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    settingExist.LastUpdated = DateTime.UtcNow;
                    settingExist.LastUpdatedBy = AuthenticationId;
                    settingExist.SettingValueVarchar = model.SettingValue;
                    settingExist.SettingDesc = model.SettingDesc;
                }
                else
                {
                    // other Create
                    Setting setting = new Setting();
                    setting.CreatedBy = AuthenticationId;
                    setting.LastUpdatedBy = AuthenticationId;
                    setting.SettingType = typeSetting;
                    setting.SettingKey = model.SettingKey;
                    setting.SettingValueVarchar = model.SettingValue;
                    _context.Settings.Add(setting);
                }

                // save to database
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (System.Exception ex)
                {
                    _logger.Log(LogLevel.Warning, "Có lỗi xảy ra khi update Settings", ex.Message);
#if DEBUG
                    throw new ApiException(ex);
#else
                    throw new ApiException("Có lỗi xảy ra khi update database");
#endif
                }

            }

            return Ok(1);
        }
    }
}