using System;
using Microsoft.Extensions.Caching.Memory;
using Poker.Models;
using System.Collections.Generic;
using System.Linq;

namespace Poker.Cache
{
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

        public Vote SetVote(Guid sessionId, Vote vote)
        {
            return _memoryCache.Set(sessionId, vote);
        }

        public Vote UpdateVote(Vote vote)
        {
            var sessionId = vote.SessionId;
            _memoryCache.Remove(sessionId);
            return _memoryCache.Set(sessionId, vote);
        }

        public Vote RemoveClient(string connectionId)
        {
            var sessionId = GetAllSessionIds() ?? new List<Guid>();
            var vote = sessionId
                .Select(GetVote)
                .FirstOrDefault(v => v.Clients.Any(c => c.ConnectionId.Equals(connectionId)));

            if (vote != null)
            {
                var clientToRemove = vote.Clients.FirstOrDefault(c => c.ConnectionId.Equals(connectionId));
                vote.Clients.Remove(clientToRemove);
                return UpdateVote(vote);
            }

            return null;
        }

        public void RemoveSession(string sessionId)
        {
            var sessionIds = GetAllSessionIds();
            if (sessionIds.Any(s => s.Equals(Guid.Parse(sessionId))))
            {
                sessionIds.Remove(Guid.Parse(sessionId));
                _memoryCache.Set("SessionIds", sessionIds);
            }

            _memoryCache.Remove(sessionId);
        }

        public IList<Guid> GetAllSessionIds()
        {
            return _memoryCache.Get<IList<Guid>>("SessionIds");
        }

        public bool IsSessionExist(string sessionId)
        {
            return GetAllSessionIds().Any(s => s.Equals(sessionId));
        }
        
        public IList<Guid> AddSessionId(Guid sessionId)
        {
            var sessionIds = GetAllSessionIds() ?? new List<Guid>();

            if (!sessionIds.Any(s => s.Equals(sessionId)))
            {
                sessionIds.Add(sessionId);
            }

            return _memoryCache.Set("SessionIds", sessionIds);
        }
    }
}
