using Kaerber.MUD.Entities;
using Kaerber.MUD.Entities.Aspects;

using Moq;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Entities {
    [TestFixture]
    public class CharacterRaceTest : BaseEntityTest {
       [Test]
        public void EventIsForwardedToRace() {
            var mockRace = new Mock<Race>();
            mockRace.Setup( race => race.ReceiveEvent( It.IsAny<Event>() ) );

            var ch = new Character { Race = mockRace.Object };

           var e = Event.Create( "test" );
            ch.ReceiveEvent( e );

            mockRace.VerifyAll();
        }
    }
}
