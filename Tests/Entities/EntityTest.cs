using System.Collections.Generic;

using Microsoft.Practices.Unity;

using Kaerber.MUD.Entities;
using Kaerber.MUD.Platform.Managers;
using Kaerber.MUD.Server;
using Newtonsoft.Json;
using NUnit.Framework;


namespace Kaerber.MUD.Tests.Entities {
    [TestFixture]
    public class EntityTest : BaseEntityTest {
        [Test]
        public void SerializeTest() {
            var container = UnityConfigurator.Configure();
            World.Instance = container.Resolve<World>();

            var entity = new Character {
                Id = "test_entity",
                Names = "test entity",
                ShortDescr = "test entity"
            };

            var data = EntitySerializer.Serialize( entity );
            Assert.AreEqual( "test_entity", data[ "Vnum" ], "Vnum" );
            Assert.AreEqual( "test entity", data[ "Names" ], "Names" );
            Assert.AreEqual( "test entity", data[ "ShortDescr" ], "ShortDescr" );
        }

        [Test]
        public void DeserializeTest() {
            var container = UnityConfigurator.Configure();
            World.Instance = container.Resolve<World>();

            var data = JsonConvert.DeserializeObject( 
                @"{'Vnum':'test_entity','Names':'test entity','ShortDescr':'test entity'," +
                 "'Stats':{'HP':0,'AC':{'Slash':0},'HitRoll':0}," +
                 "'NaturalWeapon':{'BaseDamage':40},'PowerLevel':0,'PowerPoints':0,'ToNextPowerLevel':0," +
                     "'Handlers':{'map':{'ch_says_text':{'Code':'import clr'}}}" +
                 "}" );

            var entity = CharacterManager.Deserialize( data );
            Assert.IsNotNull( entity ); 
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
