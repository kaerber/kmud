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
        protected override void CreateTestEnvironment() {
 	         base.CreateTestEnvironment();
            _vch = CreateTestCharacter( "enemy", "enemy", Room, World ).Model;
        }

        /// <summary>Characters targets enemy on Kill command </summary>
        [Test]
        public void TargetOnKillTest() {
            Controller.OnCommand( PlayerInput.Parse( "kill enem" ) );
            Assert.AreEqual( _vch, Model.Target );
        }

        [Test]
        public void AutoAttackTriggeredTest() {
            Model.Spec = SpecFactory.Warrior;

            var mockActionQueueSet = new Mock<ActionQueueSet>();
            mockActionQueueSet.Setup( s => s.EnqueueAction( "autoattack", It.IsAny<AutoAttackAction>() ) );
            Model.ActionQueueSet = mockActionQueueSet.Object;

            Controller.OnCommand( PlayerInput.Parse( "kill enem" ) );

            mockActionQueueSet.Verify( s => s.EnqueueAction( "autoattack", It.IsAny<AutoAttackAction>() ), 
                                       Times.Once() );
        }

        /// <summary>Character perform second auto-attack after the first</summary>
        [Test]
        public void CharRepeatsAutoAttack() {
            var logger = new EventLogger();
            Room.Aspects["logger"] = logger;

            Model.Kill( _vch );
            Room.Update();
            Assert.AreEqual( 1, logger.Log.Count( e => e.Name == "ch_attacks_ch1" && e["ch"] == Model ) );

            World.Pulse( ( World.Time + 3000 )*10000 );
            Room.Update();
            Assert.AreEqual( 2, logger.Log.Count( e => e.Name == "ch_attacks_ch1" && e["ch"] == Model ) );
        }

        private Character _vch;
    }
}
