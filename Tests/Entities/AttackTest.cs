using IronPython.Runtime;

using Kaerber.MUD.Entities;
using Kaerber.MUD.Entities.Aspects;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Entities
{
    [TestFixture]
    public class AttackTest
    {
        public static Event ThisAttacksCh1( Character ch, Character vch, dynamic attack  )
        {
            return Event.Create( "this_is_attacking_ch1",
                EventReturnMethod.None,
                new PythonDictionary { { "this", ch }, { "ch1", vch }, { "attack", attack } } );
        }

        public static Event ChAttacksThis( Character ch, Character vch, dynamic attack  )
        {
            return Event.Create( "ch_is_attacking_this",
                EventReturnMethod.None,
                new PythonDictionary { { "ch", ch }, { "this", vch }, { "attack", attack } } );
        }

        private dynamic _attack;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _attack = AspectFactory.Attack();

            _attack.SetAssaulterWeaponBaseDamage( 22 );

            _attack.AddAssaulterStrength( 22 );
            _attack.AddAssaulterIntellect( 20 );
            _attack.AddAssaulterDexterety( 21 );

            _attack.AddAssaulterAttack( 20 );
            _attack.AddAssaulterMagicAttack( 0 );
            _attack.AddAssaulterAccuracy( 20 );
            _attack.AddAssaulterCriticalChance( 15 );

            _attack.AddDefenderArmor( 15 );
            _attack.AddDefenderMagicArmor( 0 );
            _attack.AddDefenderEvasion( 25 );

            _attack.AddDefenderConstitution( 22 );
            _attack.AddDefenderWisdom( 21 );
       }


        [Test]
        public void AddAssaulterAttack()
        {
            var melee = AspectFactory.Attack();
            melee.AddAssaulterAttack( 100 );

            Assert.AreEqual( 90, melee.CalculateDamage( 60 ) );

            melee.AddAssaulterAttack( 100 );

            Assert.AreEqual( 120, melee.CalculateDamage( 60 ) );
        }


        [Test]
        public void AddAssaulterMagicAttack()
        {
            var attack = AspectFactory.Attack();

            attack.AddAssaulterMagicAttack( 100 );
            Assert.AreEqual( 300, attack.assaulterMagicAttack );

            attack.AddAssaulterMagicAttack( 120 );
            Assert.AreEqual( 420, attack.assaulterMagicAttack );
        }


        [Test]
        public void AddAssaulterAccuracy()
        {
            var melee = AspectFactory.Attack();

            melee.AddAssaulterAccuracy( 5 );
            Assert.AreEqual( 95, melee.CalculateHitChance() );

            melee.AddAssaulterAccuracy( 5 );
            Assert.AreEqual( 95, melee.CalculateHitChance() );
        }


        [Test]
        public void AddAttackerCriticalChance()
        {
            var attack = AspectFactory.Attack();

            attack.AddAssaulterCriticalChance( 10 );
            Assert.AreEqual( 10, attack.assaulterCriticalChance );

            attack.AddAssaulterCriticalChance( 4 );
            Assert.AreEqual( 14, attack.assaulterCriticalChance );
        }

        [Test]
        public void AddAssaulterStrength()
        {
            var melee = AspectFactory.Attack();
            melee.AddAssaulterStrength( 30 );
            melee.AddDefenderConstitution( 20 );

            Assert.AreEqual( 89, melee.CalculateDamage( 60 ) );
        }


        [Test]
        public void AddAssaulterIntellect()
        {
            var attack = AspectFactory.Attack();

            attack.AddAssaulterIntellect( 10 );
            Assert.AreEqual( 10, attack.assaulterIntellect );

            attack.AddAssaulterIntellect( 15 );
            Assert.AreEqual( 25, attack.assaulterIntellect );
        }


        [Test]
        public void AddAssaulterDexterety()
        {
            var attack = AspectFactory.Attack();
            attack.AddAssaulterDexterety( 20 );

            Assert.AreEqual( 20, attack.assaulterDexterety );

            attack.AddAssaulterDexterety( 5 );
            Assert.AreEqual( 25, attack.assaulterDexterety );
        }


        [Test]
        public void AddDefenderArmor()
        {
            var melee = AspectFactory.Attack();
            melee.AddDefenderArmor( 100 );

            Assert.AreEqual( 40, melee.CalculateDamage( 60 ) );

            melee.AddDefenderArmor( 100 );

            Assert.AreEqual( 30, melee.CalculateDamage( 60 ) );
        }


        [Test]
        public void AddDefenderMagicArmor()
        {
            var attack = AspectFactory.Attack();

            attack.AddDefenderMagicArmor( 40 );
            Assert.AreEqual( 240, attack.defenderMagicArmor );

            attack.AddDefenderMagicArmor( 50 );
            Assert.AreEqual( 290, attack.defenderMagicArmor );
        }


        [Test]
        public void AddDefenderConstitution()
        {
            var melee = AspectFactory.Attack();
            melee.AddAssaulterStrength( 20 );
            melee.AddDefenderConstitution( 30 );

            Assert.AreEqual( 30, melee.CalculateDamage( 60 ) );
        }


        [Test]
        public void AddDefenderWisdom()
        {
            var attack = AspectFactory.Attack();

            attack.AddDefenderWisdom( 10 );
            Assert.AreEqual( 10, attack.defenderWisdom );

            attack.AddDefenderWisdom( 15 );
            Assert.AreEqual( 25, attack.defenderWisdom );
        }


        [Test]
        public void AddDevenderEvasion()
        {
            var melee = AspectFactory.Attack();

            melee.AddDefenderEvasion( 5 );
            Assert.AreEqual( 5, melee.defenderEvasion );

            melee.AddDefenderEvasion( 5 );
            Assert.AreEqual( 10, melee.defenderEvasion );
        }

        [Test]
        public void NormalizeChance()
        {
            var melee = AspectFactory.Attack();
            Assert.AreEqual( 5, melee.NormalizeChance( 0 ) );
            Assert.AreEqual( 5, melee.NormalizeChance( 5 ) );
            Assert.AreEqual( 10, melee.NormalizeChance( 10 ) );
            Assert.AreEqual( 50, melee.NormalizeChance( 50 ) );
            Assert.AreEqual( 90, melee.NormalizeChance( 90 ) );
            Assert.AreEqual( 95, melee.NormalizeChance( 95 ) );
            Assert.AreEqual( 95, melee.NormalizeChance( 100 ) );
        }

        [Test]
        public void IsHitTest()
        {
            var attack = AspectFactory.Attack();
            attack.AddDefenderEvasion( 40 );

            var hit = attack.Hit();
            Assert.AreEqual( hit, attack.Hit() );
            Assert.AreEqual( hit, attack.Hit() );
            Assert.AreEqual( hit, attack.Hit() );
            Assert.AreEqual( hit, attack.Hit() );
            Assert.AreEqual( hit, attack.Hit() );
        }

        [Test]
        public void CheckCritical()
        {
            var attack = AspectFactory.Attack();
            attack.CheckCritical();
            Assert.IsFalse( attack.CriticalHit );

            attack.AddAssaulterCriticalChance( 100 );
            attack.CheckCritical();
            Assert.IsTrue( attack.CriticalHit );

        }

        [Test]
        public void CalculatePhysicalDamage()
        {
            _attack.CriticalHit = false;
            Assert.AreEqual( 22, _attack.CalculatePhysicalDamage() );

            _attack.CriticalHit = true;
            Assert.AreEqual( 44, _attack.CalculatePhysicalDamage() );
        }

        [Test]
        public void CalculateMagicalDamage()
        {
            _attack.CriticalHit = false;
            Assert.AreEqual( 22, _attack.CalculatePhysicalDamage() );

            _attack.CriticalHit = true;
            Assert.AreEqual( 44, _attack.CalculatePhysicalDamage() );
        }
    }
}
