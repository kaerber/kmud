using System.Linq;

using Kaerber.MUD.Controllers;
using Kaerber.MUD.Entities;
using Kaerber.MUD.Entities.Aspects;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Acceptance.Entities
{
    [TestFixture]
    public class CombatAcceptance : BaseAcceptanceTest {
        [SetUp]
        protected override void CreateTestEnvironment() {
 	         base.CreateTestEnvironment();
            _vch = CreateTestCharacter( "enemy", "enemy", Room, World ).Model;
        }


        /// <summary>
        /// Characters targets enemy on Kill command
        /// </summary>
        [Test]
        public void TargetOnKillTest() {
            Controller.OnCommand( PlayerInput.Parse( "kill enem" ) );
            Assert.AreEqual( _vch, Model.Target );
        }

        [Test]
        public void AutoAttackTriggeredTest() {
            Model.Spec = SpecFactory.Warrior;
            Assert.IsNotNull( Model.Spec );
            
            Controller.OnCommand( PlayerInput.Parse( "kill enem" ) );
            Assert.Fail();
        }

        /// <summary>Character perform second auto-attack after the first</summary>
        [Test]
        public void CharRepeatsAutoAttack() {
            var logger = new EventLogger();
            Room.Aspects["logger"] = logger;

            Model.Kill( _vch );
            Room.Update();
            Assert.AreEqual( 1, logger.Log.Count( e => e.Name == "ch_attacks_ch1" && e["ch"] == Model ) );

            World.Time += 3000;
            Room.Update();
            Assert.AreEqual( 2, logger.Log.Count( e => e.Name == "ch_attacks_ch1" && e["ch"] == Model ) );
        }

        private Character _vch;
    }
}
