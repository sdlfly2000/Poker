using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Poker.Cache;
using Poker.Models;

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

        [HttpPost]
        public string CreateSession([FromBody] Client client)
        {
            var guid = Guid.NewGuid();
            var vote = new Vote
            {
                SessionId = guid.ToString(),
                Clients = new List<Client> { client },
                Host = client,
                IsAlive = true
            };

            _sessionCache.SetVote(guid, vote);

            return vote.SessionId;
        }
    }
}
