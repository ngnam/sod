using StoreOrder.WebApplication.Data.Common;
using StoreOrder.WebApplication.Data.Models.Loging;
using System;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Data.Repositories.Interfaces
{
    public interface ILogRepository
    {
        Task<PagedList<Log>> GetLogsAsync(string search, int page = 1, int pageSize = 10);

        Task DeleteLogsOlderThanAsync(DateTime deleteOlderThan);
    }
}
