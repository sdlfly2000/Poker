using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Moq;
using Poker.Cache;
using Poker.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Poker.Controllers;

namespace Poker.UnitTests.Controllers
{
    using System.Linq;

    using Newtonsoft.Json;

    [TestClass]
    public class VoteControllerTest
    {
        private Guid _sessionId1 = Guid.NewGuid();
        private Guid _sessionId2 = Guid.NewGuid();
        private Guid _sessionIdNotExist = Guid.NewGuid();

        private Mock<ISessionCache> _sessionCache;
        private VoteController _controller;

        [TestInitialize]
        public void TestInitialize()
        {
            var sessionIds = new List<Guid> { _sessionId1, _sessionId2};

            _sessionCache = new Mock<ISessionCache>();
            _sessionCache
                .Setup(s => s.SetVote(It.IsAny<Guid>(), It.IsAny<Vote>()))
                .Returns<Guid, Vote>((id, vote) => vote);
            _sessionCache
                .Setup(s => s.AddSessionId(It.IsAny<Guid>()))
                .Returns<Guid>((id) => new List<Guid> { id });
            _sessionCache.Setup(s => s.GetAllSessionIds()).Returns(sessionIds);
            _sessionCache
                .Setup(s => s.GetVote(It.IsAny<Guid>()))
                .Returns<Guid>((id) => new Vote
                               {
                                   SessionId = id.ToString()
                               });

            _controller = new VoteController(_sessionCache.Object);
        }

        [TestMethod, TestCategory("UnitTest")]
        public void VoteController_GIVEN_WHEN_CreateSession_THEN_Vote_return()
        {
            // Arrange

            // Act
            var vote = _controller.CreateSession();

            // Asserts
            Assert.IsNotNull(vote);
        }

        [TestMethod, TestCategory("UnitTest")]
        public void VoteController_GIVEN_sessionId_WHEN_GetSession_THEN_true()
        {
            // Arrange

            // Act
            var result = (OkObjectResult)_controller.GetSession(_sessionId1.ToString());

            // Asserts
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.Value);
        }

        [TestMethod, TestCategory("UnitTest")]
        public void VoteController_GIVEN_NotExist_sessionId_WHEN_GetSession_THEN_false()
        {
            // Arrange

            // Act
            var result = (OkObjectResult)_controller.GetSession(_sessionIdNotExist.ToString());

            // Asserts
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.Value);
        }

        [TestMethod, TestCategory("UnitTest")]
        public void VoteController_GIVEN_Empty_sessionId_WHEN_GetSession_THEN_false()
        {
            // Arrange

            // Act
            var result = (OkObjectResult)_controller.GetSession(string.Empty);

            // Asserts
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.Value);
        }

        [TestMethod, TestCategory("UnitTest")]
        public void VoteController_GIVEN_WHEN_GetAllVotes_THEN_Votes()
        {
            // Arrange

            // Act
            var result = (OkObjectResult)_controller.GetAllVotes();
            var votes = JsonConvert.DeserializeObject<IList<Vote>>((string)result.Value);

            // Asserts
            Assert.IsNotNull(result);
            Assert.IsNotNull(votes);
            Assert.IsTrue(votes.Any(v => v.SessionId.Equals(_sessionId1.ToString())));
            Assert.IsTrue(votes.Any(v => v.SessionId.Equals(_sessionId2.ToString())));
        }

        [TestMethod, TestCategory("UnitTest")]
        public void VoteController_GIVEN_WHEN_GetAllVotes_THEN_Empty_Votes()
        {
            // Arrange
            _sessionCache.Setup(s => s.GetAllSessionIds()).Returns((IList<Guid>)null);

            // Act
            var result = (NoContentResult)_controller.GetAllVotes();

            // Asserts
            Assert.IsNotNull(result);
        }
    }
}