using Kaerber.MUD.Entities;
using Moq;
using NUnit.Framework;

namespace Kaerber.MUD.Tests.Acceptance.Entities {
    [TestFixture]
    public class CharacterCoreAcceptance {
        [Test]
        public void EveryAbilityCanOnlyBeAddedOnceToCharacter() {
            var e = Event.Create( "test_event" );
            dynamic core = new CharacterCore();

            var mockAbility1 = new Mock<IAbility>();
            mockAbility1.Setup( a => a.ReceiveEvent( e ) );

            var mockAbility2 = new Mock<IAbility>();
            mockAbility2.Setup( a => a.ReceiveEvent( e ) );

            core.Test = mockAbility1.Object;
            core.Test = mockAbility2.Object;

            core.ReceiveEvent( e );

            mockAbility1.Verify( a => a.ReceiveEvent( e ), Times.Never() );
            mockAbility2.Verify( a => a.ReceiveEvent( e ), Times.Once() );
        }
    }
}
