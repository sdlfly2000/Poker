using Poker.Hubs.models;
using System;

namespace Poker.Cache
{
    public interface ISessionCache
    {
        bool SetVote(Guid sessionId, Vote vote);

        Vote GetVote(Guid sessionId);
    }
}
