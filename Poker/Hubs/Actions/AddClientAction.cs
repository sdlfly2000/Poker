using Poker.Models;
using System.Linq;
using Common.Core.DependencyInjection;

namespace Poker.Hubs.Actions
{
    [ServiceLocate(typeof(IAddClientAction))]
    public class AddClientAction : IAddClientAction
    {
        public Vote Add(Vote vote, Client client)
        {
            var clients = vote.Clients;
            if (!clients.Any(c => c.ConnectionId.Equals(client.ConnectionId)))
            {
                vote.Clients.Add(client);
            }

            return vote;
        }
    }
}
