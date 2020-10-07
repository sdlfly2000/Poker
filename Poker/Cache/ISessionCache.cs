using Poker.Models;
using System;
using System.Collections.Generic;

namespace Poker.Cache
{
    public interface ISessionCache
    {
        Vote SetVote(Guid sessionId, Vote vote);

        Vote GetVote(Guid sessionId);

        Vote UpdateVote(Vote vote);

        IList<Vote> RemoveClient(string connectionId);

        IList<Guid> AddSessionId(Guid sessionId);

        IList<Guid> GetAllSessionIds();

        bool RemoveSession(string sessionId);
    }
}
