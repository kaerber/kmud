using Kaerber.MUD.Telnet.Handlers;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Telnet.Handlers {
    [TestFixture]
    public class HandlerStateRemoteYesTest : HandlerStateBaseTest {
        [SetUp]
        public void SetUp() {
            Prepare( State.Yes, local: false );
        }

        [Test]
        public void ReceiveWillTest() {
            OnReceiving( Will );
            StayIn( State.Yes );
            AndSend( Nothing );
        }

        [Test]
        public void ReceiveWontTest() {
            OnReceiving( Wont );
            ChangeTo( State.No );
            AndSend( Dont );
        }

        [Test]
        public void SendDoTest() {
            OnSending( Do );
            StayIn( State.Yes );
            AndSend( Nothing );
        }

        [Test]
        public void OnSendingDontChangeToWantNoEmptyAndSendDont() {
            OnSending( Dont );
            ChangeTo( State.WantNoEmpty );
            AndSend( Dont );
        }
    }
}