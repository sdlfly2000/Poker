using Poker.Models;
using System;
using System.Collections.Generic;

namespace Poker.Cache
{
    public interface ISessionCache
    {
        bool SetVote(Guid sessionId, Vote vote);

        Vote GetVote(Guid sessionId);

        bool UpdateVote(Vote vote);

        Vote RemoveClient(string connectionId);

        IList<Guid> AddSessionId(Guid sessionId);

        IList<Guid> GetAllSessionIds();
    }
}
