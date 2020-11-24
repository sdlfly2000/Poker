using Microsoft.AspNetCore.SignalR;
using Poker.Models;

namespace Poker.Hubs.Actions
{
    public interface IDispatchVoteAction
    {
        bool DispatchVote(IHubCallerClients clients, Vote vote);

        bool DispatchClearVote(IHubCallerClients clients, Vote vote);

        Vote Mask(Vote vote, string currentConnnectionId);
    }
}
