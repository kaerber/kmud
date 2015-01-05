using Kaerber.MUD.Server;
using MudSocket = Kaerber.MUD.Telnet.Socket;

using NUnit.Framework;
using Moq;

namespace Kaerber.MUD.Tests {
    [TestFixture]
    public class TelnetListenerTest {
        [Test]
        public void EventWithoutHandlersDoesNotFail() {
            var mockSocket = new Mock<MudSocket>( null );
            var listener = new TelnetListener();
            listener.Accept( mockSocket.Object );
        }

        [Test]
        public void ConstructorTest() {
            var listener = new TelnetListener();
            Assert.IsNotNull( listener );
        }
    }
}
