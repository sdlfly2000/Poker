using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.DependencyInjection;
using Poker.Cache;

namespace Poker.UnitTests.Cache
{
    [TestClass]
    public class SessionCacheTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
        }

        [TestMethod, TestCategory("UnitTest")]
        public void SessionCache_Get_SessionCache_Instance()
        {
            string[] args = { "%LAUNCHER_ARGS%" };

            // Arrange
            var host = Program.CreateHostBuilder(args).Build();
            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                // Act
                var sessionCache = services.GetRequiredService<ISessionCache>();

                // Assert
                Assert.IsNotNull(sessionCache);
            }
        }
    }
}
