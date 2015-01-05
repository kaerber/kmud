using Kaerber.MUD.Telnet.Handlers;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Telnet.Handlers {
    [TestFixture]
    public class HandlerStateLocalYesTest : HandlerStateBaseTest {
        [SetUp]
        public void SetUp() {
            Prepare( State.Yes, local: true );
        }

        [Test]
        public void ReceiveDoTest() {
            OnReceiving( Do );
            StayIn( State.Yes );
            AndSend( Nothing );
        }

        [Test]
        public void ReceiveDontTest() {
            OnReceiving( Dont );
            ChangeTo( State.No );
            AndSend( Wont );
        }

        [Test]
        public void SendWillTest() {
            OnSending( Will );
            StayIn( State.Yes );
            AndSend( Nothing );
        }

        [Test]
        public void SendWontTest() {
            OnSending( Wont );
            ChangeTo( State.WantNoEmpty );
            AndSend( Wont );
        }
    }
}
