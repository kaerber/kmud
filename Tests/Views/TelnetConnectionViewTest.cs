using System.Linq;
using System.Text;

using Kaerber.MUD.Telnet;
using Kaerber.MUD.Views;

using NUnit.Framework;
using Moq;

namespace Kaerber.MUD.Tests.Views {
    [TestFixture]
    public class TelnetConnectionViewTest {
        [Test]
        public void InStart_OnReceiving1_SendLogin() {
            var mockConnection = new Mock<TelnetConnection>( null, null );
            mockConnection.Setup( c => c.ReadLines( It.IsAny<Encoding>() ) )
                .Returns( () => new[] { "1" } );

            var view = new TelnetConnectionView( mockConnection.Object );
            view.Start();

            Assert.IsTrue( new[] { "login" }.SequenceEqual( view.Commands().ToList() ) );
        }
    }
}
