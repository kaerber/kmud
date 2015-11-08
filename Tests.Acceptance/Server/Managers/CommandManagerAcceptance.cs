using Kaerber.MUD.Platform.Managers;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Acceptance.Server.Managers {
    [TestFixture]
    public class CommandManagerAcceptance : BaseAcceptanceTest {
        [Test]
        public void Load_ReadsCommands() {
            var commandManager = new CommandManager( @"E:\Dev\Kaerber.MUD\assets\commands.json" );
            var command = commandManager.Load( string.Empty, "kill" );

            Assert.IsNotNull( command );
        }
    }
}
