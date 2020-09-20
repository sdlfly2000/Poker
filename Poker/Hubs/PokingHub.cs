using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Poker.Hubs
{
    public class PokingHub : Hub
    {
        public async Task NewMessage(string userName, string message)
        {
            await Clients.All.SendAsync("messageReceived", userName, message);
        }
    }
}
