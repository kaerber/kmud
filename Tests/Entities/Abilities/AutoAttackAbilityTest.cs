using Kaerber.MUD.Entities;
using Kaerber.MUD.Entities.Abilities;
using Kaerber.MUD.Entities.Actions;

using Moq;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Entities.Abilities
{
    [TestFixture]
    public class AutoAttackAbilityTest
    {
        [Test]
        public void EnqueAbilityToCooldownOnTargetedEvent()
        {
            CharacterAction action = null;
            var mockSelf = new Mock<Character>();
            mockSelf.Setup( self => self.EnqueueAction( "autoattack", It.IsAny<CharacterAction>() ) )
                .Callback<string, CharacterAction>( ( name, a ) => { action = a; } );

            var _event = Event.Create( "this_targeted_ch1", EventReturnMethod.None , new EventArg( "this", mockSelf.Object ) );

            var ability = new AutoAttackAbility();
            ability.ReceiveEvent( _event );

            mockSelf.VerifyAll();

            Assert.IsTrue( action is AutoAttackAction );
        }
    }
}
