using Kaerber.MUD.Platform.Managers;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Acceptance.Server.Managers {
    [TestFixture]
    public class CommandManagerAcceptance : BaseAcceptanceTest {
        [Test]
        public void Load_ReadsCommands() {
            var commandManager = new CommandManager( MUD.Entities.World.CommandsPath );
            var command = commandManager.Load( string.Empty, "kill" );

            Assert.IsNotNull( command );
        }
    }
}
