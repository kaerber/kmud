using Kaerber.MUD.Telnet.Handlers;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Telnet.Handlers {
    [TestFixture]
    public class HandlerStateLocalWantYesEmptyTest : HandlerStateBaseTest {
        [SetUp]
        public void SetUp() {
            Prepare( State.WantYesEmpty, local: true );
        }

        [Test]
        public void ReceiveDoTest() {
            OnReceiving( Do );
            ChangeTo( State.Yes );
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
            StayIn( State.WantYesEmpty );
            AndSend( Nothing );
        }

        [Test]
        public void SendWontTest() {
            OnSending( Wont );
            ChangeTo( State.WantYesOpposite );
            AndSend( Nothing );
        }
    }
}
