using Kaerber.MUD.Telnet;
using Kaerber.MUD.Telnet.Tokenizer;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Telnet.Tokenizer {
    [TestFixture]
    public class ContentTokenTest {
        [Test]
        public void OnEncodingCr_EncodeCrLf() {
            var token = new ContentToken( '\n'.ToByte() );

            Assert.AreEqual( token.Encode(), new[] { '\r'.ToByte(), '\n'.ToByte() } );
        }
    }
}
