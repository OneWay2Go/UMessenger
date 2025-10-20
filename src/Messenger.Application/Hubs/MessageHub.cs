using Microsoft.AspNetCore.SignalR;

namespace Messenger.Application.Hubs
{
    public class MessageHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            Console.WriteLine($"{user}:{message}");
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
