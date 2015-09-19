using System.Collections.Generic;

using Kaerber.MUD.Entities;
using Kaerber.MUD.Entities.Aspects;

using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Kaerber.MUD.Tests.Entities.Aspects {
    [TestFixture]
    public class WeaponTest : BaseEntityTest {
        [SetUp]
        public void SetUp() {
            _weapon = AspectFactory.Weapon();
            _weapon.BaseDamage = 30;
        }

        [Test]
        public void SerializeTest() {
            var data = _weapon.Serialize();

            Assert.IsNotNull( data );
            Assert.AreEqual( _weapon.BaseDamage, data["BaseDamage"] );
        }

        [Test]
        public void DeserializeTest() {
            var weapon = AspectFactory.Weapon();

            var data = JsonConvert.DeserializeObject( "{ 'BaseDamage': 30 }" );

            var weaponReturned = weapon.Deserialize( data );

            Assert.AreEqual( 30, weapon.BaseDamage );

            Assert.IsNotNull( weaponReturned );
            Assert.AreEqual( weapon, weaponReturned );
        }

        [Test]
        public void AttackGathersAssulaterWeapon() {
            var mockAttack = new Mock<IAttack>();
            mockAttack.Setup( attack => attack.SetAssaulterWeaponBaseDamage( 30 ) );

            var e = AttackTest.ThisAttacksCh1( null, null, mockAttack.Object );
            _weapon.ReceiveEvent( e );

            mockAttack.VerifyAll();
        }

        private dynamic _weapon;
    }
}
