using StoreOrder.WebApplication.Data.DTO.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Services.Interfaces
{
    public interface ILogService
    {
        Task<LogsDto> GetLogsAsync(string search, int page = 1, int pageSize = 10);
        Task DeleteLogsOlderThanAsync(DateTime deleteOlderThan);
    }
}
