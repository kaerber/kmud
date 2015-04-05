using System.Linq;

using Kaerber.MUD.Controllers;
using Kaerber.MUD.Entities;
using Kaerber.MUD.Entities.Actions;
using Kaerber.MUD.Entities.Aspects;

using NUnit.Framework;
using Moq;

namespace Kaerber.MUD.Tests.Acceptance.Entities {
    [TestFixture]
    public class CombatAcceptance : BaseAcceptanceTest {
        [SetUp]
        protected void Setup() {
            ConfigureTelnetEnvironment();
            _vch = CreateTestCharacter( "enemy", "enemy", TestRoom, World );
        }

        /// <summary>Characters targets enemy on Kill command </summary>
        [Test]
        public void TargetOnKillTest() {
            PlayerController.OnCommand( PlayerInput.Parse( "kill enem" ) );
            Assert.AreEqual( _vch, PlayerModel.Target );
        }

        [Test]
        public void AutoAttackTriggeredTest() {
            PlayerModel.Spec = SpecFactory.Warrior;

            var mockActionQueueSet = new Mock<ActionQueueSet>();
            mockActionQueueSet.Setup( s => s.EnqueueAction( "autoattack", It.IsAny<AutoAttackAction>() ) );
            PlayerModel.ActionQueueSet = mockActionQueueSet.Object;

            PlayerController.OnCommand( PlayerInput.Parse( "kill enem" ) );

            mockActionQueueSet.Verify( s => s.EnqueueAction( "autoattack", It.IsAny<AutoAttackAction>() ), 
                                       Times.Once() );
        }

        /// <summary>Character perform second auto-attack after the first</summary>
        [Test]
        public void CharRepeatsAutoAttack() {
            var logger = new EventLogger();
            TestRoom.Aspects["logger"] = logger;

            PlayerModel.Kill( _vch );
            TestRoom.Update();
            Assert.AreEqual( 1, logger.Log.Count( e => e.Name == "ch_attacks_ch1" && e["ch"] == PlayerModel ) );

            World.Pulse( ( World.Time + 3000 )*10000 );
            TestRoom.Update();
            Assert.AreEqual( 2, logger.Log.Count( e => e.Name == "ch_attacks_ch1" && e["ch"] == PlayerModel ) );
        }

        private Character _vch;
    }
}
