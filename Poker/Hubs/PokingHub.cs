using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System;
using Poker.Hubs.models;
using System.Collections.Generic;
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

        public string CreateSession(Client client)
        {
            var guid = Guid.NewGuid();
            var vote = new Vote
            {
                SessionId = guid.ToString(),
                Clients = new List<Client> { client },
                Host = client,
                IsAlive = true
            };

            _sessionCache.SetVote(guid, vote);

            return vote.SessionId;
        }
    }
}
