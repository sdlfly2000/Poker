using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System;
using Poker.Hubs.models;

namespace Poker.Hubs
{
    public class PokingHub : Hub
    {
        public async Task NewMessage(string userName, string message)
        {
            await Clients.All.SendAsync("messageReceived", userName, message);
        }

        public Vote CreateOrGetSession(string sessionId)
        {
            return new Vote
               {
                   SessionId = Guid.NewGuid().ToString()
               };
        }
    }
}
