using Kaerber.MUD.Telnet.Handlers;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Telnet.Handlers {
    [TestFixture]
    public class HandlerStateLocalNoTest : HandlerStateBaseTest {
        [SetUp]
        public void SetUp() {
            Prepare( State.No, local: true );
        }

        [Test]
        public void ReceiveDoTest() {
            OnReceiving( Do );
            ChangeTo( State.Yes );
            AndSend( Will );
        }

        [Test]
        public void ReceiveDontTest() {
            OnReceiving( Dont );
            StayIn( State.No );
            AndSend( Nothing );
        }

        [Test]
        public void SendWillTest() {
            OnSending( Will );
            ChangeTo( State.WantYesEmpty );
            AndSend( Will );
        }

        [Test]
        public void SendWontTest() {
            OnSending( Wont );
            StayIn( State.No );
            AndSend( Nothing );
        }
    }
}
