using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using Poker.Cache;
using Poker.Models;
using System.Linq;
using Poker.Hubs.Actions;

namespace Poker.Hubs
{
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

        public string GetSession(string sessionId)
        {
            return _sessionCache.GetAllSessionIds()
                .FirstOrDefault(s => s.Equals(Guid.Parse(sessionId)))
                .ToString();
        }

        public string JoinSession(string client, string sessionId)
        {
            var oClient = JsonConvert.DeserializeObject<Client>(client);
            var vote = _sessionCache.GetVote(Guid.Parse(sessionId));
            _sessionCache.UpdateVote(_addClientAction.Add(vote, oClient));

            var jsonVote = JsonConvert.SerializeObject(vote);

            Clients.Group(vote.SessionId).SendAsync("NewClientJoin", jsonVote);
            Groups.AddToGroupAsync(oClient.ConnectionId, vote.SessionId);

            return jsonVote;
        }

        #region Override

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var connectionId = Context.ConnectionId;
            var vote = _sessionCache.RemoveClient(connectionId);
            if (vote != null)
            {
                Groups.RemoveFromGroupAsync(connectionId, vote.SessionId);
                Clients.Group(vote.SessionId).SendAsync("NewClientJoin", JsonConvert.SerializeObject(vote));

                if(vote.Clients.Count == 0)
                {
                    _sessionCache.RemoveSession(vote.SessionId);
                }
            }

            return base.OnDisconnectedAsync(exception);
        }

        #endregion
    }
}
