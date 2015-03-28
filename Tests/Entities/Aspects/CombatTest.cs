using System.Collections.Generic;

using IronPython.Runtime;

using Kaerber.MUD.Entities;
using Kaerber.MUD.Entities.Abilities;
using Kaerber.MUD.Entities.Aspects;

using Moq;

using NUnit.Framework;


namespace Kaerber.MUD.Tests.Entities.Aspects {
    public interface IMockAttack {
        bool Hit();
        int Damage();
    }

    [TestFixture]
    class CombatTest : BaseEntityTest {
        [Test]
        public void UseAbility() {
            var mockAbility = new Mock<ICombatAbility>();
            var mockTarget = new Mock<Character>();

            mockAbility.Setup( c => c.SelectMainTarget( null ) )
                .Returns( mockTarget.Object );
            mockAbility.Setup( c => c.Activate() );
            
            var mockSelf = new Mock<Character>();
            var combat = AspectFactory.Combat();
            combat.Host = mockSelf.Object;


            combat.UseAbility( mockAbility.Object );

            mockAbility.Verify( ability => ability.SelectMainTarget( null ), Times.Once() );
            mockAbility.Verify( ability => ability.Activate(), Times.Once() );
        }

        [Test]
        public void TargetedEventTest() {
            var mockSelf = new Mock<Character>();
            var mockAbility = new Mock<ICombatAbility>();
            var mockTarget = new Mock<Character>();

            mockAbility.Setup( c => c.SelectMainTarget( null ) )
                .Returns( mockTarget.Object );
            mockAbility.Setup( c => c.Activate() );

            mockSelf.Setup( self => self.Has( "targeted_ch1", It.IsAny<PythonDictionary>() ) );

            var combat = AspectFactory.Combat();
            combat.Host = mockSelf.Object;


            combat.UseAbility( mockAbility.Object );

            mockAbility.Verify( ability => ability.SelectMainTarget( null ), Times.Once() );
            mockAbility.Verify( ability => ability.Activate(), Times.Once() );
            mockSelf.VerifyAll();
        }

        [Test]
        public void CloneTest() {
            var combat = AspectFactory.Combat();
            var clone = combat.Clone();
            Assert.IsNotNull( clone );
        }

        [Test]
        public void DeserializeReturnsSelf() {
            var combat = AspectFactory.Combat();
            var self = combat.Deserialize( new Dictionary<string, object>() );
            Assert.IsNotNull( self );
        }

        [Test]
        public void MakeAttackChecksHitOrMiss() {
            var mockChar = new Mock<Character>();

            var mockAttack = new Mock<IMockAttack>();
            mockAttack.Setup( attack => attack.Hit() ).Returns( true );
            mockAttack.Setup( attack => attack.Damage() ).Returns( 100 );

            var combat = AspectFactory.Combat();
            combat.Host = mockChar.Object;
            combat.MakeAttack( mockAttack.Object );

            mockAttack.VerifyAll();
        }

        /// <summary> If attack misses, a ch_misses_ch1 event should be raised. </summary>
        [Test]
        public void EventRaisedOnMiss() {
            var mockChar = new Mock<Character>();
            mockChar.Setup( ch => ch.Has( "missed_ch1", It.IsAny<PythonDictionary>() ) );

            var mockAttack = new Mock<IMockAttack>();
            mockAttack.Setup( attack => attack.Hit() ).Returns( false );

            var combat = AspectFactory.Combat();
            combat.Host = mockChar.Object;

            combat.MakeAttack( mockAttack.Object );

            mockChar.VerifyAll();
        }
    }
}
