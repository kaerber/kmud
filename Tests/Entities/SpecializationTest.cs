using Kaerber.MUD.Entities;
using Kaerber.MUD.Entities.Abilities;

using Moq;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Entities {
    [TestFixture]
    public class SpecializationTest {
        [Test]
        public void ReceivedEventIsForwardedToAbilities() {
            var eventForwarded = Event.Create( "null" );

            var mockAbility = new Mock<IPassiveAbility>();
            mockAbility.Setup( ability => ability.ReceiveEvent( It.IsAny<Event>() ) )
                .Callback<Event>( e => { eventForwarded = e; } );

            var specialization = new Specialization( "test", new[] { mockAbility.Object } );

            specialization.ReceiveEvent( Event.Create( "test_event" ) );

            mockAbility.Verify( ability => ability.ReceiveEvent( It.IsAny<Event>() ) );
            Assert.AreEqual( "test_event", eventForwarded.Name );
        }
    }
}
