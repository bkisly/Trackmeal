using Microsoft.AspNetCore.SignalR;

namespace Trackmeal.Hubs
{
    public class OrderStatusHub : Hub
    {
        public async Task SendNewStatus(int orderId, byte newStatusId, string newStatusName)
        {
            await Clients.All.SendAsync("ReceiveNewStatus", orderId, newStatusId, newStatusName);
        }
    }
}
