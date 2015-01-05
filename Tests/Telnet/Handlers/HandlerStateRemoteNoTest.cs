using Kaerber.MUD.Telnet.Handlers;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Telnet.Handlers {
    [TestFixture]
    public class HandlerStateRemoteNoTest : HandlerStateBaseTest {
        [SetUp]
        public void SetUp() {
            Prepare( State.No, local: false );
        }

        [Test]
        public void ReceiveWillTest() {
            OnReceiving( Will );
            ChangeTo( State.Yes );
            AndSend( Do );
        }

        [Test]
        public void ReceivoWontTest() {
            OnReceiving( Wont );
            StayIn( State.No );
            AndSend( Nothing );
        }

        [Test]
        public void SendDoTest() {
            OnSending( Do );
            ChangeTo( State.WantYesEmpty );
            AndSend( Do );
        }

        [Test]
        public void SendDontTest() {
            OnSending( Dont );
            StayIn( State.No );
            AndSend( Nothing );
        }
    }
}
