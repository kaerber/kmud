using Kaerber.MUD.Telnet.Handlers;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Telnet.Handlers {
    [TestFixture]
    public class HandlerStateRemoteWantYesEmptyTest : HandlerStateBaseTest {
        [SetUp]
        public void SetUp() {
            Prepare( State.WantYesEmpty, local: false );
        }

        [Test]
        public void ReceiveWillTest() {
            OnReceiving( Will );
            ChangeTo( State.Yes );
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
            StayIn( State.WantYesEmpty );
            AndSend( Nothing );
        }

        [Test]
        public void OnSendingDontChangeToWantYesOppositeAndSendNothing() {
            OnSending( Dont );
            ChangeTo( State.WantYesOpposite );
            AndSend( Nothing );
        }
    }
}
