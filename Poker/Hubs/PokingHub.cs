using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using Poker.Cache;
using Poker.Models;

namespace Poker.Hubs
{
    using Poker.Hubs.Actions;

    public class PokingHub : Hub
    {
        private readonly ISessionCache _sessionCache;
        private readonly IAddClientAction _addClientAction;

        public PokingHub(
            ISessionCache sessionCache, 
            IAddClientAction addClientAction)
        {
            _sessionCache = sessionCache;
            _addClientAction = addClientAction;
        }

        public async Task Session(string userName, string message)
        {
            await Clients.All.SendAsync("messageReceived", userName, message);
        }

        public string JoinSession(string client, string sessionId)
        {
            var oClient = JsonConvert.DeserializeObject<Client>(client);
            var vote = _sessionCache.GetVote(Guid.Parse(sessionId));
            _sessionCache.UpdateVote(_addClientAction.Add(vote, oClient));
            return JsonConvert.SerializeObject(vote);
        }

        #region Override

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var connectionId = Context.ConnectionId;
            var vote = _sessionCache.

        }

        #endregion
    }
}
