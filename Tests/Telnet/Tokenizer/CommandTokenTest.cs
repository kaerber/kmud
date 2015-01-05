using Kaerber.MUD.Telnet;
using Kaerber.MUD.Telnet.Tokenizer;

using NUnit.Framework;
using Moq;

namespace Kaerber.MUD.Tests.Telnet.Tokenizer {
    [TestFixture]
    public class CommandTokenTest {
        [Test]
        public void HandleCallsCommandReceived() {
            var mockTelnetConnection = new Mock<TelnetConnection>( null, null );
            mockTelnetConnection.Setup( connection =>
                connection.ReceiveCommand( Option.SuppressLocalEcho, Command.Will ) );
            var token = TokenFactory.Instance.Command( Command.Will, Option.SuppressLocalEcho );

            token.Handle( mockTelnetConnection.Object );

            mockTelnetConnection.Verify( connection => 
                connection.ReceiveCommand( Option.SuppressLocalEcho, Command.Will ), Times.Once() );
        }
    }
}
