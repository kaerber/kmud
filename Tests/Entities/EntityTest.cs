using System.Collections.Generic;

using NUnit.Framework;

using Kaerber.MUD.Entities;

namespace Kaerber.MUD.Tests.Entities
{
    [TestFixture]
    public class EntityTest : BaseEntityTest {
        [Test]
        public void SerializeTest() {
            World.Instance = new World();

            var entity = new Character {
                Id = "test_entity",
                Names = "test entity",
                ShortDescr = "test entity"
            };

            entity.Affects.Add( new Affect(
                    new AffectInfo {
                        Name = "test_affect",
                        Target = AffectTarget.Character
                    }
                )
            );

            var data = entity.Serialize();
            Assert.AreEqual( "test_entity", data[ "Vnum" ], "Vnum" );
            Assert.AreEqual( "test entity", data[ "Names" ], "Names" );
            Assert.AreEqual( "test entity", data[ "ShortDescr" ], "ShortDescr" );
            Assert.IsInstanceOf<IEnumerable<Affect>>( data["Affects"], "Affects" );

            World.Serializer.Serialize( entity );
        }

        [Test]
        public void DeserializeTest() {
            const string data = 
                @"{'Vnum':'test_entity','Names':'test entity','ShortDescr':'test entity'," +
                 "'Affects':[{'Name':'test_affect','Duration':-10}],'Stats':{'HP':0,'AC':{'Slash':0},'HitRoll':0}," +
                 "'NaturalWeapon':{'BaseDamage':40},'PowerLevel':0,'PowerPoints':0,'ToNextPowerLevel':0," +
                     "'Handlers':{'map':{'ch_says_text':{'Code':'import clr'}}}" +
                 "}";

            World.Instance = new World();

            Assert.Throws<EntityException>( () => World.Serializer.Deserialize<Character>( data ) );

            World.Instance.Affects.Add(
                "test_affect",
                new AffectInfo
                {
                    Name = "test_affect",
                    Target = AffectTarget.Character
                }
            );

            var entity2 = World.Serializer.Deserialize<Character>( data );
            Assert.IsInstanceOf<Character>( entity2 ); 
        }

        [Test]
        public void MatchNamesTest() {
            var entity = new Entity { Names = "name test x2" };

            Assert.IsTrue( entity.MatchNames( "name" ) );
            Assert.IsTrue( entity.MatchNames( "na" ) );
            Assert.IsFalse( entity.MatchNames( "x3" ) );
        }
    }
}
