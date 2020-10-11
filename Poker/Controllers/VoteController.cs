using System;
using Microsoft.AspNetCore.Mvc;
using Poker.Cache;
using Poker.Models;
using System.Collections.Generic;
using System.Linq;
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
        public IActionResult CreateSession()
        {
            var guid = Guid.NewGuid();
            var vote = new Vote
            {
                SessionId = guid.ToString()
            };

            _sessionCache.SetVote(guid, vote);
            _sessionCache.AddSessionId(guid);

            return Ok(JsonConvert.SerializeObject(vote));
        }

        [HttpGet]
        public IActionResult GetSession(string sessionId)
        {
            var allSessionIds = _sessionCache.GetAllSessionIds() ?? new List<Guid>();
            var isSessionIdExist = allSessionIds.Any(s => s.Equals(Guid.Parse(sessionId)));
            return Ok(isSessionIdExist);
        }

        [HttpGet]
        public IActionResult GetAllVotes()
        {
            var allSessionIds = _sessionCache.GetAllSessionIds() ?? new List<Guid>();
            var votes = allSessionIds.Select(id => _sessionCache.GetVote(id)).ToList();
            if (votes.Any())
            {
                return Ok(JsonConvert.SerializeObject(votes));
            }

            return NoContent();
        }

        [HttpGet]
        public void RemoveVote(string sessionId)
        {
            _sessionCache.RemoveSession(sessionId);
        }
    }
}
