using Microsoft.EntityFrameworkCore;
using StoreOrder.WebApplication.Data.Models.Loging;

namespace StoreOrder.WebApplication.Data
{
    public interface IAdminLogDbContext
    {
        DbSet<Log> Logs { get; set; }
    }
}
