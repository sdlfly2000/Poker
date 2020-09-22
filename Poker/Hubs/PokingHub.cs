using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System;
using Poker.Hubs.models;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Poker.Hubs
{
    public class PokingHub : Hub
    {
        public async Task NewMessage(string userName, string message)
        {
            await Clients.All.SendAsync("messageReceived", userName, message);
        }

        public string CreateOrGetSession(string sessionId)
        {
            var vote = new Vote
            {
                SessionId = Guid.NewGuid().ToString(),
                Clients = new List<Client>(),
                Host = new Client { 
                    UserName = "Jay",
                    Vote = 10,
                    IsReady = false
                },
                IsAlive = true
            };

            var result = JsonConvert.SerializeObject(vote);

            return result;
        }

        public int Greeting()
        {
            return 100;
        }
    }
}
