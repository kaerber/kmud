using NUnit.Framework;

namespace Kaerber.MUD.Tests.Controllers
{
    [TestFixture]
    public class UserLoginConnectionCommandTest
    {
        [Test]
        public void HijackConnectionTest()
        {
            // Old player receives message that he's disconnected
            // New player full initialized and ready to play
            // New player has command set:default
            // Old player dropped with exception, new player connects normally
        }
    }
}
