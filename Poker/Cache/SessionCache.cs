using System;
using Microsoft.Extensions.Caching.Memory;
using Poker.Models;
using System.Collections.Generic;

namespace Poker.Cache
{
    using System.Linq;

    public class SessionCache : ISessionCache
    {
        private readonly IMemoryCache _memoryCache;

        public SessionCache(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public Vote GetVote(Guid sessionId)
        {
           return  _memoryCache.Get<Vote>(sessionId);
        }

        public bool SetVote(Guid sessionId, Vote vote)
        {
            return _memoryCache.Set(sessionId, vote) != null;
        }

        public bool UpdateVote(Vote vote)
        {
            var sessionId = vote.SessionId;
            _memoryCache.Remove(sessionId);
            return _memoryCache.Set(sessionId, vote) != null;
        }

        public Vote RemoveClient(string connectionId)
        {
            var sessionId = GetAllSessionIds();
            var votes = sessionId.Select(GetVote)
                .Where(v => v.Clients.Any(c => c.ConnectionId.Equals(connectionId)))
                .ToList();
        }

        public IList<Guid> GetAllSessionIds()
        {
            return _memoryCache.Get<IList<Guid>>("SessionIds");
        }

        public IList<Guid> AddSessionId(Guid sessionId)
        {
            var sessionIds = GetAllSessionIds();
            if (!sessionIds.Any(s => s.Equals(sessionId)))
            {
                sessionIds.Add(sessionId);
            }

            return _memoryCache.Set("SessionIds", sessionIds);
        }
    }
}
