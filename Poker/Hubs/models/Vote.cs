using System.Collections.Generic;

namespace Poker.Hubs.models
{
    public class Vote
    {
        public Vote()
        {
            Clients = new List<Client>();
        }
        public string SessionId;

        public IList<Client> Clients;

        public Client Host;

        public bool IsAlive;
    }
}