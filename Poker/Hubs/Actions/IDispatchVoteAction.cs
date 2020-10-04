using Microsoft.AspNetCore.SignalR;
using Poker.Models;

namespace Poker.Hubs.Actions
{
    public interface IDispatchVoteAction
    {
        void Dispatch(IHubCallerClients clients, Vote vote);

        Vote Mask(Vote vote, string currentConnnectionId);
    }
}
