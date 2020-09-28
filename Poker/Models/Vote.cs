using System.Collections.Generic;

namespace Poker.Models
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