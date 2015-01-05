using System.Collections.Generic;

using NUnit.Framework;

using Kaerber.MUD.Entities;

namespace Kaerber.MUD.Tests.Entities
{
    [TestFixture]
    public class AffectInfoTest : BaseEntityTest {
        [Test]
        public void Name() {
            var affectData = new AffectInfo { Name = "sanctuary" };
            Assert.AreEqual( affectData.Name, "sanctuary" );
        }

        [Test]
        public void Serialize() {
            var data = new AffectInfo { Name = "horde_room", Target = AffectTarget.Room };

            var serialized = data.Serialize();
            Assert.AreEqual( 3, serialized.Count );
            
            Assert.IsTrue( serialized.ContainsKey( "Name" ) );
            Assert.AreEqual( data.Name, serialized["Name"] );

            Assert.IsTrue( serialized.ContainsKey( "Target" ) );
            Assert.AreEqual( data.Target, serialized["Target"] );

            Assert.IsTrue( serialized.ContainsKey( "Flags" ) );
            Assert.AreEqual( data.Flags, serialized["Flags"] );

        }

        [Test]
        public void Deserialize() {
            var data = new AffectInfo();
            var serialized = new Dictionary<string, object> {
                    { "Name", "horde_room" },
                    { "Target", AffectTarget.Room },
                    { "Flags", AffectFlags.Multiple }
                };

            data.Deserialize( serialized );

            Assert.AreEqual( data.Name, serialized["Name"] );
            Assert.AreEqual( data.Target, serialized["Target"] );
            Assert.AreEqual( data.Flags, AffectFlags.Multiple );
        }

        [Test]
        public void Get() {
            World.Instance = new World();

            World.Instance.Affects.Clear();

            Assert.Throws<EntityException>( () => AffectInfo.Get( "horde_mode" ) );
            World.Instance.Affects.Add(
                "horde_mode",
                new AffectInfo
                {
                    Name = "horde_mode",
                    Target = AffectTarget.Room
                }
            );
            Assert.AreEqual( World.Instance.Affects["horde_mode"], AffectInfo.Get( "horde_mode" ) );
        }
    }
}
