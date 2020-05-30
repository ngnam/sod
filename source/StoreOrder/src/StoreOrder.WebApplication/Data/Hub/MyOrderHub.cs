using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace StoreOrder.WebApplication.Data.Hub
{
    public class MyOrderHub : Microsoft.AspNetCore.SignalR.Hub
    {
        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }

        public Task SendMessage(string user, string message)
        {
            return Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public Task SendMessageToCaller(string message)
        {
            return Clients.Caller.SendAsync("ReceiveMessage", message);
        }

        public Task SendMessageToGroup(string message)
        {
            return Clients.Group("SignalR Users").SendAsync("ReceiveMessage", message);
        }
    }
}
