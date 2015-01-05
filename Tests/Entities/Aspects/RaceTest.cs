using Kaerber.MUD.Entities;
using Kaerber.MUD.Entities.Aspects;

using Moq;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Entities.Aspects {
    [TestFixture]
    public class RaceTest : BaseEntityTest {
        [Test]
        public void EventIsForwardedToStats() {
            var testEvent = Event.Create( "test" );

            var mockStats = new Mock<IAspect>();
            mockStats.Setup( stats => stats.ReceiveEvent( testEvent ) );

            var race = new Race { Stats = mockStats.Object };

            race.ReceiveEvent( testEvent );

            mockStats.VerifyAll();
        }
    }
}
