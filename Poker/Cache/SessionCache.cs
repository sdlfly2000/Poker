using Poker.Hubs.models;
using System;
using Microsoft.Extensions.Caching.Memory;

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

        public bool SetVote(Guid sessionId, Vote vote)
        {
            return _memoryCache.Set(sessionId, vote) != null;
        }
    }
}
