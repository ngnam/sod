using Microsoft.EntityFrameworkCore;
using StoreOrder.WebApplication.Data.Common;
using StoreOrder.WebApplication.Data.Models.Loging;
using StoreOrder.WebApplication.Data.Repositories.Interfaces;
using StoreOrder.WebApplication.Extensions;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Data.Repositories
{
    public class LogRepository<TDbContext> : ILogRepository
           where TDbContext : DbContext, IAdminLogDbContext
    {
        protected readonly TDbContext DbContext;

        public LogRepository(TDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public virtual async Task DeleteLogsOlderThanAsync(DateTime deleteOlderThan)
        {
            var logsToDelete = await DbContext.Logs.Where(x => x.TimeStamp < deleteOlderThan.Date).ToListAsync();

            if (logsToDelete.Count == 0) return;

            DbContext.Logs.RemoveRange(logsToDelete);
            await DbContext.SaveChangesAsync();
        }

        public virtual async Task<PagedList<Log>> GetLogsAsync(string search, int page = 1, int pageSize = 10)
        {
            var pagedList = new PagedList<Log>();
            Expression<Func<Log, bool>> searchCondition = x => x.LogEvent.Contains(search) || x.Message.Contains(search) || x.Exception.Contains(search);
            var logs = await DbContext.Logs
                .AsNoTracking()
                .WhereIf(!string.IsNullOrEmpty(search), searchCondition)
                .PageBy(x => x.Id, page, pageSize)
                .ToListAsync();

            pagedList.Data.AddRange(logs);
            pagedList.PageSize = pageSize;
            pagedList.TotalCount = await DbContext.Logs.WhereIf(!string.IsNullOrEmpty(search), searchCondition).CountAsync();

            return pagedList;
        }

        public virtual async Task<int> ClearAllLogs()
        {
            string sqlCommand = "TRUNCATE TABLE logging.logs RESTART IDENTITY;";
            return await DbContext.Database.ExecuteSqlRawAsync(sqlCommand);
        }

        public virtual async Task<int> CountLogs()
        {
            return await DbContext.Logs.CountAsync();
        }
    }
}
