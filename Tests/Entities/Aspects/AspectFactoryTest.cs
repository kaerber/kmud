using Kaerber.MUD.Entities;
using Kaerber.MUD.Entities.Aspects;

using NUnit.Framework;


namespace Kaerber.MUD.Tests.Entities.Aspects {
    [TestFixture]
    public class AspectFactoryTest : BaseEntityTest {
        [Test]
        public void StaticCreate() {
            Assert.IsTrue( 
                AspectFactory.Test()
                .ReceiveEvent( Event.Create( "test_complete", EventReturnMethod.And ) ) );
        }

        [Test]
        public void ComplexTest() {
            Assert.IsNotNull( AspectFactory.Complex() );
        }

        [Test]
        public void HealthTest() {
            Assert.IsNotNull( AspectFactory.Health() );
        }

        [Test]
        public void ManaTest() {
            Assert.IsNotNull( AspectFactory.Mana() );
        }

        [Test]
        public void CombatTest() {
            Assert.IsNotNull( AspectFactory.Combat() );
        }

        [Test]
        public void Movement() {
            Assert.IsNotNull( AspectFactory.Movement() );
        }

        [Test]
        public void AI() {
            Assert.IsNotNull( AspectFactory.AI() );
        }

        [Test]
        public void Stats() {
            Assert.IsNotNull( AspectFactory.Stats() );
        }

        [Test]
        public void Money() {
            Assert.IsNotNull( AspectFactory.Money() );
        }

        [Test]
        public void Weapon() {
            Assert.IsNotNull( AspectFactory.Weapon() );
        }

        [Test]
        public void View() {
            Assert.IsNotNull( AspectFactory.View() );
        }

        [Test]
        public void StatQueryIsCreatedSuccessfully() {
            Assert.IsNotNull( AspectFactory.StatQuery() );
        }
    }
}
