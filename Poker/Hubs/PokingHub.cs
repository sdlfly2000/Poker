using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using Poker.Cache;
using Poker.Models;

namespace Poker.Hubs
{
    public class PokingHub : Hub
    {
        private readonly ISessionCache _sessionCache;

        public PokingHub(ISessionCache sessionCache)
        {
            _sessionCache = sessionCache;
        }

        public async Task Session(string userName, string message)
        {
            await Clients.All.SendAsync("messageReceived", userName, message);
        }

        public string JoinSession(string client, string sessionId)
        {
            var oClient = JsonConvert.DeserializeObject<Client>(client);
            var vote = _sessionCache.GetVote(Guid.Parse(sessionId));
            vote.Clients.Add(oClient);
            _sessionCache.UpdateVote(vote);
            return JsonConvert.SerializeObject(vote);
        }
    }
}
