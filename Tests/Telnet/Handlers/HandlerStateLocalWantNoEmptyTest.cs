using Kaerber.MUD.Telnet.Handlers;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Telnet.Handlers {
    [TestFixture]
    public class HandlerStateLocalWantNoEmptyTest : HandlerStateBaseTest {
        [SetUp]
        public void SetUp() {
            Prepare( State.WantNoEmpty, local: true );
        }

        [Test]
        public void ReceiveDoTest() {
            OnReceiving( Do );
            StayIn( State.WantNoEmpty );
            AndSend( Nothing );
        }

        [Test]
        public void ReceiveDontTest() {
            OnReceiving( Dont );
            ChangeTo( State.No );
            AndSend( Nothing );
        }

        [Test]
        public void SendWillTest() {
            OnSending( Will );
            ChangeTo( State.WantNoOpposite );
            AndSend( Nothing );
        }

        [Test]
        public void SendWontTest() {
            OnSending( Wont );
            StayIn( State.WantNoEmpty );
            AndSend( Nothing );
        }
    }
}
