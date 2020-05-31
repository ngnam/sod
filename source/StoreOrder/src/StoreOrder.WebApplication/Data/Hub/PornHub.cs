using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Data.Hub
{
    public class PornHub : Microsoft.AspNetCore.SignalR.Hub
    {
        private readonly StoreOrderDbContext _context;
        public PornHub(StoreOrderDbContext context)
        {
            _context = context;
        }
        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }

        [HubMethodName("SendOrderId")]
        public Task SendOrderId()
        {
            var result = _context.StoreTables.ToListAsync().Result;
            return Clients.All.SendAsync("ReceiveOrder", result);
        }

        [HubMethodName("SendMessageToUser")]
        public Task DirectMessage(string user, string message)
        {
            return Clients.User(user).SendAsync("ReceiveMessage", message);
        }
    }
}
