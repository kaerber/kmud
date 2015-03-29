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
            _vch = CreateTestCharacter( "enemy", "enemy", TestRoom, World );
        }

        /// <summary>Characters targets enemy on Kill command </summary>
        [Test]
        public void TargetOnKillTest() {
            TestChar.Controller.OnCommand( PlayerInput.Parse( "kill enem" ) );
            Assert.AreEqual( _vch.Model, TestChar.Model.Target );
        }

        [Test]
        public void AutoAttackTriggeredTest() {
            TestChar.Model.Spec = SpecFactory.Warrior;

            var mockActionQueueSet = new Mock<ActionQueueSet>();
            mockActionQueueSet.Setup( s => s.EnqueueAction( "autoattack", It.IsAny<AutoAttackAction>() ) );
            TestChar.Model.ActionQueueSet = mockActionQueueSet.Object;

            TestChar.Controller.OnCommand( PlayerInput.Parse( "kill enem" ) );

            mockActionQueueSet.Verify( s => s.EnqueueAction( "autoattack", It.IsAny<AutoAttackAction>() ), 
                                       Times.Once() );
        }

        /// <summary>Character perform second auto-attack after the first</summary>
        [Test]
        public void CharRepeatsAutoAttack() {
            var logger = new EventLogger();
            TestRoom.Aspects["logger"] = logger;

            TestChar.Model.Kill( _vch.Model );
            TestRoom.Update();
            Assert.AreEqual( 1, logger.Log.Count( e => e.Name == "ch_attacks_ch1" && e["ch"] == TestChar.Model ) );

            World.Pulse( ( World.Time + 3000 )*10000 );
            TestRoom.Update();
            Assert.AreEqual( 2, logger.Log.Count( e => e.Name == "ch_attacks_ch1" && e["ch"] == TestChar.Model ) );
        }

        private TestCharacter _vch;
    }
}
