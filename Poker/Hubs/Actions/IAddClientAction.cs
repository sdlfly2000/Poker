using Poker.Models;

namespace Poker.Hubs.Actions
{
    public interface IAddClientAction
    {
        Vote Add(Vote vote, Client client);
    }
}
