using Kaerber.MUD.Entities;

using Moq;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Server {
    [TestFixture]
    public class ServerTest {
        [Test]
        public void Update_CallsUpdateOnWorld() {
            var world = new Mock<World>( null, new Clock( 0 ) );
            world.Setup( w => w.Pulse( 0 ) );
            World.Instance = world.Object;
            var server = new MUD.Server.Server();

            server.Pulse( 0 );

            world.Verify( w => w.Pulse( 0 ), Times.Once() );
        }
    }
}
