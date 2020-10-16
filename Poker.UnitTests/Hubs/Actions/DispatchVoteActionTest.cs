using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Poker.Hubs.Actions;
using Poker.Models;
using System.Collections.Generic;
using System.Linq;

namespace Poker.UnitTests.Hubs.Actions
{
    [TestClass]
    public class DispatchVoteActionTest
    {
        private string ConnectionId1 = "ConnectionId1";
        private string ConnectionId2 = "ConnectionId2";
        private string ConnectionId3 = "ConnectionId3";

        private Vote _vote;
        private DispatchVoteAction _action;

        [TestInitialize]
        public void TestInitialize()
        {
            var client1 = new Client { ConnectionId = ConnectionId1, Vote = "2" };
            var client2 = new Client { ConnectionId = ConnectionId2, Vote = "3" };
            var client3 = new Client { ConnectionId = ConnectionId3, Vote = "5" };

            _vote = new Vote { Clients = new List<Client> { client1, client2, client3 } };
            _action = new DispatchVoteAction();
        }

        [TestMethod, TestCategory("UnitTest")]
        public void DispatchVoteAction_instance_registered()
        {
            string[] args = { "%LAUNCHER_ARGS%" };

            // Arrange
            var host = Program.CreateHostBuilder(args).Build();
            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                // Act
                var dispatchVoteAction = services.GetRequiredService<IDispatchVoteAction>();

                // Assert
                Assert.IsNotNull(dispatchVoteAction);
            }
        }

        [TestMethod, TestCategory("UnitTest")]
        public void DispatchVoteAction_GIVEN_a_ConnectionId_and_Vote_WHEN_Mask_THEN_dataInOtherClient_hidden_in_Vote_return()
        {
            // Arrange

            // Act
            var vote = _action.Mask(_vote, ConnectionId1);

            // Asserts
            Assert.IsNotNull(vote);
            Assert.IsTrue(vote.Clients.Where(c => !c.ConnectionId.Equals(ConnectionId1)).All(c => c.Vote.Equals("-----")));
            Assert.IsFalse(vote.Clients.First(c => c.ConnectionId.Equals(ConnectionId1)).Vote.Equals(string.Empty));
        }
    }
}
