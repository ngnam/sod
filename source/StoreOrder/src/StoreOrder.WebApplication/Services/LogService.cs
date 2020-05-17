using StoreOrder.WebApplication.Data.DTO.Log;
using StoreOrder.WebApplication.Data.Repositories.Interfaces;
using StoreOrder.WebApplication.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Services
{
    public class LogService : ILogService
    {
        protected readonly ILogRepository Repository;

        public LogService(ILogRepository repository)
        {
            Repository = repository;
        }

        public virtual async Task<LogsDto> GetLogsAsync(string search, int page = 1, int pageSize = 10)
        {
            var pagedList = await Repository.GetLogsAsync(search, page, pageSize);
            LogsDto logs = new LogsDto
            {
                Logs = pagedList.Data.Select(p => new LogDto
                {
                    Id = p.Id,
                    Exception = p.Exception,
                    Level = p.Level,
                    LogEvent = p.LogEvent,
                    Message = p.Message,
                    MessageTemplate = p.MessageTemplate,
                    Properties = p.Properties,
                    TimeStamp = p.TimeStamp
                }).ToList(),
                PageSize = pagedList.PageSize,
                TotalCount = pagedList.TotalCount
            };

            return logs;
        }

        public virtual async Task DeleteLogsOlderThanAsync(DateTime deleteOlderThan)
        {
            await Repository.DeleteLogsOlderThanAsync(deleteOlderThan);
        }
    }
}
