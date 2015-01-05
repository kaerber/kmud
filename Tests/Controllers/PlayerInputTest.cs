using NUnit.Framework;

using Kaerber.MUD.Controllers;

namespace Kaerber.MUD.Tests.Controllers
{
    [TestFixture]
    public class PlayerInputTest
    {
        [Test]
        public void ParseTest()
        {
            var result = PlayerInput.Parse( "hand.ch_says_text.2 + from System.Collections.Generic import *" );
            Assert.AreEqual( "hand.ch_says_text.2 + from System.Collections.Generic import *", result.RawLine );
            Assert.AreEqual( "hand.ch_says_text.2", result.Command );
            Assert.AreEqual( "+ from System.Collections.Generic import *", result.RawArguments );
            Assert.AreEqual( 5, result.Arguments.Count );
            Assert.AreEqual( "+", result.Arguments[0] );
            Assert.AreEqual( "from", result.Arguments[1] );
            Assert.AreEqual( "System.Collections.Generic", result.Arguments[2] );
            Assert.AreEqual( "import", result.Arguments[3] );
            Assert.AreEqual( "*", result.Arguments[4] );
        }

        [Test]
        public void MultilineParseTest()
        {
            var result = PlayerInput.Parse( "say test\nmy\nballs" );
            Assert.AreEqual( "say", result.Command );
        }
    }
}
