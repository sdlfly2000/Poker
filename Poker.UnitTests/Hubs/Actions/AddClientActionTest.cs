using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Poker.Hubs.Actions;
using Poker.Models;
using System;
using System.Linq;

namespace Poker.UnitTests.Hubs.Actions
{
    [TestClass]
    public class AddClientActionTest
    {
        private Guid ConnectionId = Guid.NewGuid();

        private Vote _vote;
        private Client _client;
        private AddClientAction _action;

        [TestInitialize]
        public void TestInitialize()
        {
            _client = new Client { ConnectionId = ConnectionId.ToString() };
            _vote = new Vote();

            _action = new AddClientAction();
        }

        [TestMethod, TestCategory("UnitTest")]
        public void AddClientAction_registed_instance()
        {
            string[] args = { "%LAUNCHER_ARGS%" };

            // Arrange
            var host = Program.CreateHostBuilder(args).Build();
            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                // Act
                var addClientAction = services.GetRequiredService<IAddClientAction>();

                // Assert
                Assert.IsNotNull(addClientAction);
            }
        }

        [TestMethod, TestCategory("UnitTest")]
        public void AddClientAction_GIVEN_one_Client_And_Vote_WHEN_Add_THEN_Vote_with_Client()
        {
            // Arrange

            // Act
            var vote = _action.Add(_vote, _client);

            // Asserts
            Assert.IsNotNull(vote);
            Assert.IsTrue(vote.Clients.Any(c => c.ConnectionId.Equals(ConnectionId.ToString())));
        }
    }
}
