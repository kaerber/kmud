using System.Linq;
using System.Text;

using Kaerber.MUD.Telnet;
using Kaerber.MUD.Telnet.Tokenizer;

using NUnit.Framework;
using Moq;

namespace Kaerber.MUD.Tests.Acceptance.Telnet {
    [TestFixture]
    public class TokenStreamAcceptance {
        [Test]
        public void SimpleValueDecodeTest() {
            var mockSocket = new Mock<Socket>( null );
            mockSocket.Setup( socket => socket.Read() )
                .Returns( "test".ToBytes() );

            var stream = new TokenStream( mockSocket.Object );

            var result = stream.Read();
            Assert.AreEqual( "test", string.Join( "", result.Select( token => token.Content( Encoding.ASCII ) ) ) );
        }
    }
}
