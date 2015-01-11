using Kaerber.MUD.Server.Managers;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Acceptance.Server.Managers {
    [TestFixture]
    public class CommandManagerAcceptance : BaseAcceptanceTest {
        [Test]
        public void Load_ReadsCommands() {
            var commandManager = new CommandManager();
            commandManager.Load();

            Assert.IsNotNull( commandManager.Get( "kill" ) );
        }
    }
}
