using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Poker.Cache;
using Poker.Models;

namespace Poker.Controllers
{
    using Newtonsoft.Json;

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

            return JsonConvert.SerializeObject(vote);
        }
    }
}
