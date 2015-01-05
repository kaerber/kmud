using System.Collections.Generic;
using System.Linq;

using Kaerber.MUD.Telnet;
using Kaerber.MUD.Telnet.Tokenizer;
using Kaerber.MUD.Telnet.Tokenizer.TokenParser;

using Moq;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Telnet {
    [TestFixture]
    public class TokenStreamTest {
        [SetUp]
        public void SetUp() {
            _mockSocket = new Mock<Socket>( null );
            _mockSocket.Setup( socket => socket.Read() )
                .Returns( "Test".ToBytes() );
            Token token;
            _mockNormal = new Mock<Normal>();
            _mockNormal.Setup( state => state.Parse( It.IsAny<byte>(), out token ) )
                .Returns( _mockNormal.Object );
            _mockStateFactory = new Mock<StateFactory>();
            _mockStateFactory.Setup( factory => factory.Normal() )
                .Returns( _mockNormal.Object );
            StateFactory.Instance = _mockStateFactory.Object;
            _stream = new TokenStream( _mockSocket.Object );
            
        }


        [Test]
        public void EncodeContentToken() {
            var testResult = TestEncodeToken( TokenFactory.Instance.Content( 'T'.ToByte() ) );

            Assert.IsTrue( testResult.SequenceEqual( new [] { 'T'.ToByte() } ) );
        }

        [Test]
        public void EncodeCommandToken() {
            var testResult = TestEncodeToken( TokenFactory.Instance.Command( Command.Dont, Option.Echo ) );

            Assert.IsTrue( testResult.SequenceEqual( new[] { Command.Iac, Command.Dont, ( byte )Option.Echo } ) );
        }

        [Test]
        public void EncodeFunctionToken() {
            var testResult = TestEncodeToken( TokenFactory.Instance.Function( Command.Brk ) );

            Assert.IsTrue( testResult.SequenceEqual( new byte[] { Command.Iac, Command.Brk } ) );
        }

        [Test]
        public void EncodeSubNegotiationToken() {
            var testResult = TestEncodeToken( TokenFactory.Instance.SubNegotiation( Option.Echo,
                new ByteBuffer( new byte[] { 1 } ) ) );

            Assert.IsTrue( testResult.SequenceEqual( new byte[] {
                Command.Iac, Command.Sb, ( byte )Option.Echo,
                1,
                Command.Iac, Command.Se
            } ) );
        }

        [Test]
        public void ReadGetsAllTheAvailableDataFromSocket() {
            var result = new List<Token>( _stream.Read() );

            _mockSocket.Verify( socket => socket.Read(), Times.Once() );
        }
    
        [Test]
        public void ReadStartsWithNormalState() {
            var result = new List<Token>( _stream.Read() );

            _mockStateFactory.Verify( factory => factory.Normal(), Times.Once() );
        }

        [Test]
        public void ReadGetsNewStateFromOldAfterParsing() {
            Token token;

            var result = new List<Token>( _stream.Read() );

            _mockNormal.Verify( state => state.Parse( It.IsAny<byte>(), out token ), Times.Exactly( 4 ) );
        }

        private IEnumerable<byte> TestEncodeToken( Token token ) {
            IEnumerable<byte> testResult = null;
            var mockSocket = new Mock<Socket>( null );
            mockSocket.Setup( socket => socket.Write( It.IsAny<IEnumerable<byte>>() ) )
                .Callback<IEnumerable<byte>>( test => testResult = test );
            var tokenStream = new TokenStream( mockSocket.Object );

            tokenStream.Write( token );

            return testResult;
        }

        private Mock<Socket> _mockSocket;
        private Mock<Normal> _mockNormal;
        private Mock<StateFactory> _mockStateFactory;
 
        private TokenStream _stream;
    }
}
