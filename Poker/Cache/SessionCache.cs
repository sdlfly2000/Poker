using System;
using Microsoft.Extensions.Caching.Memory;
using Poker.Models;
using System.Collections.Generic;
using System.Linq;
using Common.Core.DependencyInjection;
using Common.Core.Cache;

namespace Poker.Cache
{
    [ServiceLocate(typeof(ISessionCache))]
    public class SessionCache : ISessionCache
    {
        private readonly ICacheService _cacheService;

        private MemoryCacheEntryOptions _memoryCacheEntryOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
                    };

        public SessionCache(ICacheService cacheService)
        {
            _cacheService = cacheService;

            _memoryCacheEntryOptions.PostEvictionCallbacks.Add(new PostEvictionCallbackRegistration
               {
                    EvictionCallback = (key, value, reason, status) =>
                        {
                            if (!((Vote)value).Clients.Any())
                            {
                                var sessionIds = GetAllSessionIds();
                                sessionIds.Remove(Guid.Parse((string)key));
                                _cacheService.Set("SessionIds", sessionIds);
                            }
                        }
               });
        }

        public Vote GetVote(Guid sessionId)
        {
           return _cacheService.Get<Vote>(sessionId.ToString());
        }

        public Vote SetVote(Guid sessionId, Vote vote)
        {
            return _cacheService.Set(sessionId.ToString(), vote, _memoryCacheEntryOptions);
        }

        public Vote UpdateVote(Vote vote)
        {
            var sessionId = vote.SessionId;
            _cacheService.Remove(sessionId);
            return _cacheService.Set(sessionId, vote, _memoryCacheEntryOptions);
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
                _cacheService.Set("SessionIds", sessionIds);
            }

            _cacheService.Remove(sessionId);
            return true;
        }

        public IList<Guid> GetAllSessionIds()
        {
            return _cacheService.Get<IList<Guid>>("SessionIds");
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

            return _cacheService.Set("SessionIds", sessionIds);
        }
    }
}
