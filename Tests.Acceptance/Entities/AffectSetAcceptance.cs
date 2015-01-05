using System.Reflection;

using NUnit.Framework;

using Kaerber.MUD.Entities;

namespace Kaerber.MUD.Tests.Acceptance.Entities
{
    [TestFixture( Category = "acceptance" )]
    public class AffectSetAcceptance : BaseAcceptanceTest {
        [Test]
        public void AcceptanceTest() {
            const string data = @"{'Vnum':'test_entity','Names':'test entity','ShortDescr':'test entity'," +
                "'Affects':[{'Name':'test_affect','Duration':-10}],'Stats':{'HP':0,'AC':{'Slash':0},'HitRoll':0}," +
                "'NaturalWeapon':{'BaseDamage':1},'PowerLevel':0,'PowerPoints':0,'ToNextPowerLevel':0," +
                "'Handlers':{'map':{'ch_says_text':{'Code':'import clr'}}}" +
                "}";

            World.Instance = new World();

            World.Instance.Affects.Add(
                "test_affect",
                new AffectInfo {
                    Name = "test_affect",
                    Target = AffectTarget.Character
                }
            );

            var entity = World.Serializer.Deserialize<Character>( data );

            var field = typeof( AffectSet ).GetField(
                "_host",
                BindingFlags.Instance|BindingFlags.NonPublic|BindingFlags.Public );
            Assert.IsNotNull( field );
            Assert.IsNotNull( field.GetValue( entity.Affects ) );
        }
    }
}
