using Kaerber.MUD.Telnet.Handlers;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Telnet.Handlers {
    [TestFixture]
    public class HandlerStateLocalWantYesOppositeTest : HandlerStateBaseTest {
        [SetUp]
        public void SetUp() {
            Prepare( State.WantYesOpposite, local: true );
        }

        [Test]
        public void ReceiveDoTest() {
            OnReceiving( Do );
            ChangeTo( State.WantNoEmpty );
            AndSend( Dont );
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
            ChangeTo( State.WantYesEmpty );
            AndSend( Nothing );
        }

        [Test]
        public void SendWontTest() {
            OnSending( Wont );
            StayIn( State.WantYesOpposite );
            AndSend( Nothing );
        }
    }
}
