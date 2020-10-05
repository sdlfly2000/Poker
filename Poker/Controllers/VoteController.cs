using System;
using Microsoft.AspNetCore.Mvc;
using Poker.Cache;
using Poker.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;

namespace Poker.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VoteController : ControllerBase
    {
        private readonly ISessionCache _sessionCache;

        public VoteController(ISessionCache sessionCache)
        {
            _sessionCache = sessionCache;
        }

        [HttpGet]
        public string CreateSession()
        {
            var guid = Guid.NewGuid();
            var vote = new Vote
            {
                SessionId = guid.ToString()
            };

            _sessionCache.SetVote(guid, vote);
            _sessionCache.AddSessionId(guid);

            return JsonConvert.SerializeObject(vote);
        }

        [HttpGet]
        public bool GetSession(string sessionId)
        {
            var allSessionIds = _sessionCache.GetAllSessionIds() ?? new List<Guid>();
            var isSessionIdExist = allSessionIds.Any(s => s.Equals(Guid.Parse(sessionId)));
            Response.StatusCode = isSessionIdExist ? (int)HttpStatusCode.OK : (int)HttpStatusCode.NotFound;
            return isSessionIdExist;
        }

        [HttpGet]
        public string GetVote(string sessionId)
        {
            var allSessionIds = _sessionCache.GetAllSessionIds() ?? new List<Guid>();
            var isSessionIdExist = allSessionIds.Any(s => s.Equals(Guid.Parse(sessionId)));
            Response.StatusCode = isSessionIdExist ? (int)HttpStatusCode.OK : (int)HttpStatusCode.NotFound;
            return JsonConvert.SerializeObject(_sessionCache.GetVote(Guid.Parse(sessionId)));
        }
    }
}
