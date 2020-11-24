using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using Poker.Cache;
using Poker.Models;
using System.Linq;
using Poker.Hubs.Actions;
using System.Collections.Generic;

namespace Poker.Hubs
{
    public class PokingHub : Hub
    {
        private readonly ISessionCache _sessionCache;
        private readonly IAddClientAction _addClientAction;
        private readonly IDispatchVoteAction _dispatchVoteAction;

        public PokingHub(
            ISessionCache sessionCache, 
            IAddClientAction addClientAction,
            IDispatchVoteAction dispatchVoteAction)
        {
            _sessionCache = sessionCache;
            _addClientAction = addClientAction;
            _dispatchVoteAction = dispatchVoteAction;
        }

        public string UpdateCurrentClient(string currentClient, string sessionId)
        {
            var vote = _sessionCache.GetVote(Guid.Parse(sessionId));
            var oCurrentClient = JsonConvert.DeserializeObject<Client>(currentClient);
            if (vote != null)
            {
                vote.Clients = UpdateClient(vote.Clients, oCurrentClient);

                _dispatchVoteAction.DispatchVote(Clients, vote);

                return JsonConvert.SerializeObject(_dispatchVoteAction.Mask(vote, oCurrentClient.ConnectionId));
            }

            return null;
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

            RemoveClient(oClient.ConnectionId);

            var vote = _sessionCache.GetVote(Guid.Parse(sessionId));
            _sessionCache.UpdateVote(_addClientAction.Add(vote, oClient));

            _dispatchVoteAction.DispatchVote(Clients, vote);
            Groups.AddToGroupAsync(oClient.ConnectionId, vote.SessionId);

            return JsonConvert.SerializeObject(
                _dispatchVoteAction.Mask(vote, oClient.ConnectionId));
        }

        public void SetOpenToPublic(string sessionId)
        {
            var vote = _sessionCache.GetVote(Guid.Parse(sessionId));
            
            if(vote != null)
            {
                vote.IsPublicOpen = true;
                _dispatchVoteAction.DispatchVote(Clients, vote);
            }
        }

        public void ClearVotes(string sessionId)
        {
            var vote = _sessionCache.GetVote(Guid.Parse(sessionId));

            if (vote != null)
            {
                vote.IsPublicOpen = false;
                vote.Clients = vote.Clients
                    .Select(c => {
                        c.Vote = null;
                        c.IsReady = false;
                        return c;
                    }).ToList();
                _dispatchVoteAction.DispatchVote(Clients, vote);
                _dispatchVoteAction.DispatchClearVote(Clients, vote);
            }
        }

        #region Override

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var connectionId = Context.ConnectionId;
            RemoveClient(connectionId);
            return base.OnDisconnectedAsync(exception);
        }

        #endregion

        #region Private Methods

        private IList<Client> UpdateClient(IList<Client> clients, Client client)
        {
            var results = new List<Client>();
            results.AddRange(clients.Select(c =>
            {
                return c.ConnectionId.Equals(client.ConnectionId)
                ? client
                : c;
            }));

            return results;
        }

        private bool RemoveClient(string connectionId)
        {
            var votes = _sessionCache.RemoveClient(connectionId);
            if (votes != null)
            {
                votes.Select(v => Groups.RemoveFromGroupAsync(connectionId, v.SessionId)).ToList();
                votes.Select(v => _dispatchVoteAction.DispatchVote(Clients, v)).ToList();

                votes.Where(v => v.Clients.Count == 0).Select(v => _sessionCache.RemoveSession(v.SessionId)).ToList();
            }

            return true;
        }

        #endregion
    }
}
