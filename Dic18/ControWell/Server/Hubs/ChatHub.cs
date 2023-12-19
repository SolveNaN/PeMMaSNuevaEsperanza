using Microsoft.AspNetCore.SignalR;

namespace ControWell.Server.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message,List<Balance> balancesChat)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message,balancesChat);
        }
    }
}
