using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Poker.Cache;
using Poker.Hubs;
using Poker.Hubs.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Poker.Models;

namespace Poker.UnitTests.Hubs
{
    using System.Threading;

    [TestClass]
    public class PokingHubTest
    {
        private readonly string ConnectionId1 = "ConnectionId1";
        private readonly string ConnectionId2 = "ConnectionId2";

        private Guid SessionId1 = Guid.NewGuid();
        private Guid SessionId2 = Guid.NewGuid();

        private Client _client1;
        private Client _client2;

        private Mock<ISessionCache> _sessionCacheMock;
        private Mock<IAddClientAction> _addClientActionMock;
        private Mock<IDispatchVoteAction> _dispatchVoteActionMock;

        private PokingHub _pokingHub;
        
        [TestInitialize]
        public void TestInitialize()
        {
            var sessionIds = new List<Guid> { SessionId1, SessionId2 };

            _client1 = new Client{ConnectionId = ConnectionId1 };
            _client2 = new Client{ConnectionId = ConnectionId2 };

            var vote1 = new Vote
                            {
                                SessionId = SessionId1.ToString(),
                                Clients = new List<Client> { _client1 }
                            };
            var vote2 = new Vote
                            {
                                SessionId = SessionId2.ToString(),
                                Clients = new List<Client> { _client2 }
                            };

            var votes = new List<Vote> { vote1, vote2 };

            _sessionCacheMock = new Mock<ISessionCache>();
            _addClientActionMock = new Mock<IAddClientAction>();
            _dispatchVoteActionMock = new Mock<IDispatchVoteAction>();
            var groupManager = new Mock<IGroupManager>();

            _sessionCacheMock
                .Setup(s => s.GetVote(It.IsAny<Guid>()))
                .Returns<Guid>((id) => votes.FirstOrDefault(v => v.SessionId.Equals(id.ToString())));

            _sessionCacheMock.Setup(s => s.GetAllSessionIds()).Returns(sessionIds);

            _sessionCacheMock
                .Setup(s => s.RemoveClient(It.IsAny<string>()))
                .Returns<string>((connectionId) => votes.Where(v => v.Clients.All(c => !c.ConnectionId.Equals(connectionId))).ToList());

            _sessionCacheMock
                .Setup(s => s.RemoveSession(It.IsAny<string>()))
                .Returns<string>((connectionId) => true);

            _dispatchVoteActionMock
                .Setup(d => d.Dispatch(It.IsAny<IHubCallerClients>(), It.IsAny<Vote>()))
                .Returns(null);

            _dispatchVoteActionMock
                .Setup(d => d.Mask(It.IsAny<Vote>(), It.IsAny<string>()))
                .Returns<Vote, string>((vote, id) => vote);

            groupManager
                .Setup(g => g.AddToGroupAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns<string, string, CancellationToken>((connectionId, sessionId, token) => null);

            groupManager
                .Setup(g => g.RemoveFromGroupAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns<string, string, CancellationToken>((connectionId, sessionId, token) => null);

            _pokingHub = new PokingHub(_sessionCacheMock.Object, _addClientActionMock.Object, _dispatchVoteActionMock.Object);


            _pokingHub.Groups = groupManager.Object;
        }

        [TestMethod, TestCategory("UnitTest")]
        public void PokingHub_GIVEN_currentClient_And_sessionId_WHEN_UpdateCurrentClient_THEN_update_Vote_return()
        {
            // Arrange
            var currentClient = new Client{ ConnectionId = ConnectionId1, IsReady = true };
            var jsonCurrentClient = JsonConvert.SerializeObject(currentClient);

            // Act
            var result = _pokingHub.UpdateCurrentClient(jsonCurrentClient, SessionId1.ToString());
            var vote = JsonConvert.DeserializeObject<Vote>(result);

            // Assert
            Assert.IsNotNull(vote);
            Assert.IsTrue(vote.Clients.First().IsReady);
        }

        [TestMethod, TestCategory("UnitTest")]
        public void PokingHub_GIVEN_sessionId_WHEN_GetSession_THEN_sessionId()
        {
            // Arrange

            // Act
            var result = _pokingHub.GetSession(SessionId1.ToString());

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(SessionId1.ToString(), result);
        }

        [TestMethod, TestCategory("UnitTest")]
        public void PokingHub_GIVEN_client_And_sessionId_WHEN_JoinSession_THEN_vote()
        {
            // Arrange
            var jsonClient = JsonConvert.SerializeObject(_client2);

            // Act
            var result = _pokingHub.JoinSession(jsonClient, SessionId1.ToString());
            var vote = JsonConvert.DeserializeObject<Vote>(result);

            // Assert
            Assert.IsNotNull(vote);
            Assert.IsTrue(vote.Clients.Any(c => c.ConnectionId.Equals(ConnectionId2)));
        }



    }
}