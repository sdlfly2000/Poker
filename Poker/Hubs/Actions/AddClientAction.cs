using Poker.Models;
using System.Linq;

namespace Poker.Hubs.Actions
{
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
