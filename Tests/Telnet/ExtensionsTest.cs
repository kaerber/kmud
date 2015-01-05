using System.Text;

using Kaerber.MUD.Telnet;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Telnet {
    [TestFixture]
    class ExtensionsTest {
        [Test]
        public void GetLineRemovesLineWithCrLf() {
            var builder = new StringBuilder( "test string\nsecond string" );

            builder.GetLine();

            Assert.AreEqual( "second string", builder.ToString() );
        }

        [Test]
        public void GetLineRemovesBothCrAndLfFromTheEndOfTheLine() {
            var builder = new StringBuilder( "test string\r\nsecond string" );

            var line = builder.GetLine();

            Assert.AreEqual( "test string", line );
        }
    }
}
