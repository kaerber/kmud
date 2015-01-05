using Kaerber.MUD.Entities;
using Kaerber.MUD.Entities.Abilities;
using Kaerber.MUD.Entities.Aspects;

using Moq;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Entities.Aspects
{
    [TestFixture]
    class SpecTest
    {
        [Test]
        public void EventIsForwardedToAutoAttack()
        {
            Event testEvent = null;
            var mockAutoAttack = new Mock<AutoAttackAbility>();
            mockAutoAttack.Setup( autoattack => autoattack.ReceiveEvent( It.IsAny<Event>() ) )
                .Callback<Event>( e => { testEvent = e; } );
            var spec = new Spec( "test_spec" ) { AutoAttack = mockAutoAttack.Object };
            spec.ReceiveEvent( Event.Create( "test_event" ) );

            mockAutoAttack.VerifyAll();
            Assert.AreEqual( "test_event", testEvent.Name );
        }
    }
}
