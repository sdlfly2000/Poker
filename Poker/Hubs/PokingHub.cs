using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using Poker.Cache;

namespace Poker.Hubs
{
    public class PokingHub : Hub
    {
        private readonly ISessionCache _sessionCache;

        public PokingHub(ISessionCache sessionCache)
        {
            _sessionCache = sessionCache;
        }

        public async Task NewMessage(string userName, string message)
        {
            await Clients.All.SendAsync("messageReceived", userName, message);
        }

        public string GetSession(string sessionId)
        {
            var vote = _sessionCache.GetVote(Guid.Parse(sessionId));

            var result = JsonConvert.SerializeObject(vote);

            return result;
        }
    }
}
