using Kaerber.MUD.Telnet.Handlers;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Telnet.Handlers {
    [TestFixture]
    public class HandlerStateRemoteWantNoEmptyTest : HandlerStateBaseTest {
        [SetUp]
        public void SetUp() {
            Prepare( State.WantNoEmpty, local: false );
        }

        [Test]
        public void ReceiveWillTest() {
            OnReceiving( Will );
            ChangeTo( State.No );
            AndSend( Nothing );
        }

        [Test]
        public void ReceiveWontTest() {
            OnReceiving( Wont );
            ChangeTo( State.No );
            AndSend( Nothing );
        }

        [Test]
        public void SendDoTest() {
            OnSending( Do );
            ChangeTo( State.WantNoOpposite );
            AndSend( Nothing );
        }

        [Test]
        public void SendDontTest() {
            OnSending( Dont );
            StayIn( State.WantNoEmpty );
            AndSend( Nothing );
        }
    }
}
