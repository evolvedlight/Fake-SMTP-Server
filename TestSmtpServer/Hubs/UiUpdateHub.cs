using Microsoft.AspNetCore.SignalR;

namespace TestSmtpServer.Hubs
{
    public class UiUpdateHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
