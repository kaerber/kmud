using Kaerber.MUD.Telnet.Handlers;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Telnet.Handlers {
    [TestFixture]
    public class HandlerStateWantYesOppositeTest : HandlerStateBaseTest {
        [SetUp]
        public void SetUp() {
            Prepare( State.WantYesOpposite, local: false );
        }

        [Test]
        public void ReceiveWillTest() {
            OnReceiving( Will );
            ChangeTo( State.WantNoEmpty );
            AndSend( Dont );
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
            ChangeTo( State.WantYesEmpty );
            AndSend( Nothing );
        }

        [Test]
        public void SendDontTest() {
            OnSending( Dont );
            StayIn( State.WantYesOpposite );
            AndSend( Nothing );
        }
    }
}
