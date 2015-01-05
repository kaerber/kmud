using Kaerber.MUD.Telnet.Handlers;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Telnet.Handlers {
    [TestFixture]
    public class HandlerStateRemoteWantNoOppositeTest : HandlerStateBaseTest {
        [SetUp]
        public void SetUp() {
            Prepare( State.WantNoOpposite, local: false );
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
            ChangeTo( State.WantYesEmpty );
            AndSend( Do );
        }

        [Test]
        public void SendDoTest() {
            OnSending( Do );
            StayIn( State.WantNoOpposite );
            AndSend( Nothing );
        }

        [Test]
        public void OnSendingDontChangeToWantNoEmptyAndSendNothing() {
            OnSending( Dont );
            ChangeTo( State.WantNoEmpty );
            AndSend( Nothing );
        }
    }
}
