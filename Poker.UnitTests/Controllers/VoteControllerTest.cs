using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Moq;
using Poker.Cache;
using Poker.Models;
using System.Collections.Generic;
using Newtonsoft.Json;
using Poker.Controllers;

namespace Poker.UnitTests.Controllers
{
    using Microsoft.AspNetCore.Http;

    [TestClass]
    public class VoteControllerTest
    {
        private Guid _sessionId1 = Guid.NewGuid();
        private Guid _sessionId2 = Guid.NewGuid();

        private VoteController _controller;

        [TestInitialize]
        public void TestInitialize()
        {
            var sessionIds = new List<Guid> { _sessionId1, _sessionId2};

            var sessionCache = new Mock<ISessionCache>();
            sessionCache
                .Setup(s => s.SetVote(It.IsAny<Guid>(), It.IsAny<Vote>()))
                .Returns<Guid, Vote>((id, vote) => vote);
            sessionCache
                .Setup(s => s.AddSessionId(It.IsAny<Guid>()))
                .Returns<Guid>((id) => new List<Guid> { id });
            sessionCache.Setup(s => s.GetAllSessionIds()).Returns(sessionIds);

            _controller = new VoteController(sessionCache.Object);
        }

        [TestMethod, TestCategory("UnitTest")]
        public void VoteController_GIVEN_WHEN_CreateSession_THEN_Vote_return()
        {
            // Arrange

            // Act
            var vote = _controller.CreateSession();
            var oVote = JsonConvert.DeserializeObject<Vote>(vote);

            // Asserts
            Assert.IsNotNull(oVote);
        }

        [TestMethod, TestCategory("UnitTest")]
        public void VoteController_GIVEN_sessionId_WHEN_GetSession_THEN_true()
        {
            // Arrange

            // Act
            var result = _controller.GetSession(_sessionId1.ToString());

            // Asserts
            Assert.IsNotNull(result);
        }
    }
}