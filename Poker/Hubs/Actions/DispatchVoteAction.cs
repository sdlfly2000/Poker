﻿using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Poker.Models;
using System.Linq;

namespace Poker.Hubs.Actions
{
    public class DispatchVoteAction : IDispatchVoteAction
    {
        public void Dispatch(IHubCallerClients clients, Vote vote)
        {
            if (vote != null)
            {
                vote.Clients
                    .Select(c => clients.Client(c.ConnectionId).SendAsync(
                        "NewClientJoin",
                        JsonConvert.SerializeObject(Mask(vote, c.ConnectionId)))).ToList();
            }
        }

        public Vote Mask(Vote vote, string currentConnnectionId)
        {
            var resultVote = DuplicateVote(vote);

            if (!resultVote.IsPublicOpen)
            {
                resultVote.Clients = resultVote.Clients.Select(c =>
                {
                    if (!c.ConnectionId.Equals(currentConnnectionId))
                    {
                        c.Vote = "-----";
                    }
                    return c;
                }).ToList();
            }

            return resultVote;
        }

        #region Private Methods

        private Vote DuplicateVote(Vote vote)
        {
            return vote != null 
                ? new Vote
                    {
                        SessionId = vote.SessionId,
                        IsPublicOpen = vote.IsPublicOpen,
                        Host = MapClient(vote.Host),
                        Clients = vote.Clients.Select(c => MapClient(c)).ToList()
                    }
                : null;
        }

        private Client MapClient(Client client)
        {
            return client != null
                ?  new Client
                    {
                        ConnectionId = client.ConnectionId,
                        Vote = client.Vote,
                        UserName = client.UserName,
                        IsReady = client.IsReady
                    }
                : null;
        }

        #endregion
    }
}
