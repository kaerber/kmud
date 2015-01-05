using Kaerber.MUD.Telnet.Handlers;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Telnet.Handlers {
    [TestFixture]
    public class HandlerStateLocalWantNoOppositeTest : HandlerStateBaseTest {
        [SetUp]
        public void SetUp() {
            Prepare( State.WantNoOpposite, local: true );
        }

        [Test]
        public void ReceiveDoTest() {
            OnReceiving( Do );
            StayIn( State.WantNoOpposite );
            AndSend( Nothing );
        }

        [Test]
        public void ReceiveDontTest() {
            OnReceiving( Dont );
            ChangeTo( State.WantYesEmpty );
            AndSend( Do );
        }

        [Test]
        public void SendWillTest() {
            OnSending( Will );
            StayIn( State.WantNoOpposite );
            AndSend( Nothing );
        }

        [Test]
        public void SendWontTest() {
            OnSending( Wont );
            ChangeTo( State.WantNoEmpty );
            AndSend( Nothing );
        }
    }
}
