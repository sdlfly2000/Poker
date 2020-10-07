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

        public IList<Vote> RemoveClient(string connectionId)
        {
            var sessionId = GetAllSessionIds() ?? new List<Guid>();
            var votes = sessionId
                .Select(GetVote)
                .Where(v => v.Clients.Any(c => c.ConnectionId.Equals(connectionId)))
                .ToList();

            if (votes != null)
            {
                votes.Select(v => v.Clients.Remove(v.Clients.FirstOrDefault(c => c.ConnectionId.Equals(connectionId)))).ToList();
                return votes.Select(UpdateVote).ToList();
            }

            return null;
        }

        public bool RemoveSession(string sessionId)
        {
            var sessionIds = GetAllSessionIds();
            if (sessionIds.Any(s => s.Equals(Guid.Parse(sessionId))))
            {
                sessionIds.Remove(Guid.Parse(sessionId));
                _memoryCache.Set("SessionIds", sessionIds);
            }

            _memoryCache.Remove(sessionId);
            return true;
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
