using System.Collections.Generic;

using Kaerber.MUD.Entities;
using Kaerber.MUD.Entities.Aspects;

using Moq;

using NUnit.Framework;


namespace Kaerber.MUD.Tests.Entities.Aspects
{
    [TestFixture]
    public class StatsTest : BaseEntityTest {
        public static dynamic CreateTestStats() {
            var stats = AspectFactory.Stats();
            stats.Health = 100;
            stats.Mana = 50;
            stats.Attack = 10;
            stats.Armor = 11;
            stats.MagicAttack = 30;
            stats.MagicArmor = 31;
            stats.Accuracy = 20;
            stats.Evasion = 16;
            stats.CriticalHitChance = 3;

            stats.Strength = 25;
            stats.Dexterety = 26;
            stats.Constitution = 27;
            stats.Intellect = 28;
            stats.Wisdom = 29;

            return stats;
        }

        public static void AssertEqualToTest( dynamic actual ) {
            var test = CreateTestStats();

            Assert.AreEqual( test.Health, actual.Health );
            Assert.AreEqual( test.Mana, actual.Mana );
            Assert.AreEqual( test.Attack, actual.Attack );
            Assert.AreEqual( test.Armor, actual.Armor );
            Assert.AreEqual( test.MagicAttack, actual.MagicAttack );
            Assert.AreEqual( test.MagicArmor, actual.MagicArmor );
            Assert.AreEqual( test.Accuracy, actual.Accuracy );
            Assert.AreEqual( test.Evasion, actual.Evasion );
            Assert.AreEqual( test.CriticalHitChance, actual.CriticalHitChance );

            Assert.AreEqual( test.Strength, actual.Strength );
            Assert.AreEqual( test.Dexterety, actual.Dexterety );
            Assert.AreEqual( test.Constitution, actual.Constitution );
            Assert.AreEqual( test.Intellect, actual.Intellect );
            Assert.AreEqual( test.Wisdom, actual.Wisdom );
            
        }

        [Test]
        public void DefenderArmorAddsUp() {
            var stats = AspectFactory.Stats();
            stats.Armor = 100;

            var ev = Event.Create( "ch_attacks_this",
                EventReturnMethod.None,
                new EventArg( "ch", null ),
                new EventArg( "ch1", null ),
                new EventArg( "attack", AspectFactory.Attack() ) );
            stats.ReceiveEvent( ev );

            Assert.AreEqual( 40, ev["attack"].CalculateDamage( 60 ) );
        }

        [Test]
        public void AssaulterArmorIgnored() {
            var stats = AspectFactory.Stats();
            stats.Armor = 100;

            var ev = Event.Create( "this_attacks_ch1",
                EventReturnMethod.None,
                new EventArg( "ch", null ),
                new EventArg( "ch1", null ),
                new EventArg( "attack", AspectFactory.Attack() ) );
            stats.ReceiveEvent( ev );
            
            Assert.AreEqual( 60, ev["attack"].CalculateDamage( 60 ) );
        }

        [Test]
        public void DefenderAttackIgnored() {
            var stats = AspectFactory.Stats();
            stats.Attack = 100;

            var ev = Event.Create( "ch_attacks_this",
                EventReturnMethod.None,
                new EventArg( "ch", null ),
                new EventArg( "ch1", null ),
                new EventArg( "attack", AspectFactory.Attack() ) );
            stats.ReceiveEvent( ev );

            Assert.AreEqual( 60, ev["attack"].CalculateDamage( 60 ) );
        }

        [Test]
        public void AssaulterAttackAddsUp() {
            var stats = AspectFactory.Stats();
            stats.Attack = 100;

            var ev = Event.Create( "this_attacks_ch1",
                EventReturnMethod.None,
                new EventArg( "ch", null ),
                new EventArg( "ch1", null ),
                new EventArg( "attack", AspectFactory.Attack() ) );
            stats.ReceiveEvent( ev );
            
            Assert.AreEqual( 90, ev["attack"].CalculateDamage( 60 ) );
        }


        [Test]
        public void Serialize() {
            var stats = CreateTestStats();

            var data = stats.Serialize();

            Assert.IsNotNull( data );
            Assert.AreEqual( 100, data["hp"] );
            Assert.AreEqual( 50, data["mana"] );
            Assert.AreEqual( stats.Attack, data["attack"] );
            Assert.AreEqual( stats.Armor, data["armor"] );
            Assert.AreEqual( 30, data["mattack"] );
            Assert.AreEqual( 31, data["marmor"] );
            Assert.AreEqual( 20, data["accuracy"] );
            Assert.AreEqual( 16, data["evasion"] );
            Assert.AreEqual( 3, data["critchance"] );

            Assert.AreEqual( 25, data["strength"] );
            Assert.AreEqual( 26, data["dexterety"] );
            Assert.AreEqual( 27, data["constitution"] );
            Assert.AreEqual( 28, data["intellect"] );
            Assert.AreEqual( 29, data["wisdom"] );
        }


        [Test]
        public void Deserialize() {
            var data = new Dictionary<string, dynamic> {
                { "hp", 200 }, { "mana", 50 },
                { "attack", 15 }, { "armor", 14 },
                { "mattack", 30 }, { "marmor", 31 },
                { "accuracy", 21 }, { "evasion", 17 }, { "critchance", 3 },
                { "strength", 25 }, { "dexterety", 26 }, { "constitution", 27 }, { "intellect", 28 }, { "wisdom", 29 }
            };

            var stats = AspectFactory.Stats();
            stats.Deserialize( data );

            Assert.AreEqual( 200, stats.Health );
            Assert.AreEqual( 50, stats.Mana );
            Assert.AreEqual( 15, stats.Attack );
            Assert.AreEqual( 14, stats.Armor );
            Assert.AreEqual( 30, stats.MagicAttack );
            Assert.AreEqual( 31, stats.MagicArmor );
            Assert.AreEqual( 21, stats.Accuracy );
            Assert.AreEqual( 17, stats.Evasion );
            Assert.AreEqual( 3, stats.CriticalHitChance );

            Assert.AreEqual( 25, stats.Strength );
            Assert.AreEqual( 26, stats.Dexterety );
            Assert.AreEqual( 27, stats.Constitution );
            Assert.AreEqual( 28, stats.Intellect );
            Assert.AreEqual( 29, stats.Wisdom );
        }

        [Test]
        public void AttackGathersAttackerStats() {
            var stats = CreateTestStats();

            var mockAttack = new Mock<IAttack>();
            mockAttack.Setup( attack => attack.AddAssaulterStrength( 25 ) );
            mockAttack.Setup( attack => attack.AddAssaulterIntellect( 28 ) );
            mockAttack.Setup( attack => attack.AddAssaulterDexterety( 26 ) );

            mockAttack.Setup( attack => attack.AddAssaulterAttack( 10 ) );
            mockAttack.Setup( attack => attack.AddAssaulterMagicAttack( 30 ) );
            mockAttack.Setup( attack => attack.AddAssaulterAccuracy( 33 ) );
            mockAttack.Setup( attack => attack.AddAssaulterCriticalChance( 16 ) );

            var e = AttackTest.ThisAttacksCh1( null, null, mockAttack.Object );
            stats.ReceiveEvent( e );

            mockAttack.VerifyAll();
        }

        [Test]
        public void AttackGathersDefenderStats() {
            var stats = CreateTestStats();

            var mockAttack = new Mock<IAttack>();
            mockAttack.Setup( attack => attack.AddDefenderArmor( 11 ) );
            mockAttack.Setup( attack => attack.AddDefenderConstitution( 27 ) );
            mockAttack.Setup( attack => attack.AddDefenderMagicArmor( 31 ) );
            mockAttack.Setup( attack => attack.AddDefenderWisdom( 29 ) );
            mockAttack.Setup( attack => attack.AddDefenderEvasion( 42 ) );

            var e = AttackTest.ChAttacksThis( null, null, mockAttack.Object );
            stats.ReceiveEvent( e );

            mockAttack.VerifyAll();
        }

        [Test]
        public void CloneStats() {
            var stats = CreateTestStats();

            var clone = stats.Clone();

            Assert.IsNotNull( clone );

            AssertEqualToTest( clone );
        }

        [Test]
        public void QueryMaxHealthGathersHealthStats() {
            var stats = CreateTestStats();

            var e = Event.Create( "query_max_health", EventReturnMethod.Sum );

            stats.ReceiveEvent( e );

            Assert.AreEqual( stats.Health + stats.Constitution*25, e.ReturnValue );
        }

        [Test]
        public void QueryMaxManaGathersManaStats() {
            var stats = CreateTestStats();

            var e = Event.Create( "query_max_mana", EventReturnMethod.Sum );

            stats.ReceiveEvent( e );

            Assert.AreEqual( stats.Mana + stats.Intellect*25, e.ReturnValue );
        }
    }
}
