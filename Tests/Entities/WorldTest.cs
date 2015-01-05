using Kaerber.MUD.Entities;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Entities {
    [TestFixture]
    public class WorldTest : BaseEntityTest {
        [Test]
        public void ConvertToTypeTest() {
            Assert.AreEqual( 10, World.ConvertToType<int>( "10" ) );
        }

        private static World CreateTestWorld() {
            var world = new World { Id = "world", Names = "world", ShortDescr = "world" };

            world.Areas.Add( new Area( "world_test_area", "test area", "test area" ) );

            world.Skills.Add( "world_test_skill",
                new SkillData {
                    Name = "world_test_skill",
                    Rating = 5
                }
            );

            world.Affects.Add( "world_test_affect",
                new AffectInfo {
                    Name = "world_test_affect",
                    Target = AffectTarget.Character
                }
            );

            return ( world );
        }

        [Test]
        public void SerializeTest() {
            var data = World.Serializer.Serialize( CreateTestWorld() );
        }

        [Test]
        public void DeserializeTest() {
            var world = World.Serializer.Deserialize<World>(
                World.Serializer.Serialize( CreateTestWorld() )
            );

            Assert.AreEqual( "world", world.Id );
            Assert.AreEqual( "world", world.Names );
            Assert.AreEqual( "world", world.ShortDescr );

            Assert.AreEqual( 1, world.Areas.Count );
            Assert.AreEqual( "world_test_area", world.Areas[ 0 ].Id );

            Assert.AreEqual( 1, world.Skills.Count );
            Assert.IsTrue( world.Skills.ContainsKey( "world_test_skill" ) );
            Assert.AreEqual( 5, world.Skills[ "world_test_skill" ].Rating );

            Assert.AreEqual( 1, world.Affects.Count );
            Assert.IsTrue( world.Affects.ContainsKey( "world_test_affect" ) );
            Assert.AreEqual( AffectTarget.Character, world.Affects[ "world_test_affect" ].Target );
        }

        [Test]
        public void GetRoom() {
            CreateTestWorldAndRoom();

            Assert.AreEqual( _room, _world.GetRoom( "test" ) );
        }

        [Test]
        public void GetRoomOfNullIdReturnsNull() {
            CreateTestWorldAndRoom();

            Assert.IsNull( _world.GetRoom( null ) );
        }


        private void CreateTestWorldAndRoom() {
            _world = new World();
            _room = new Room { Id = "test", ShortDescr = "test room" };
            _world.Rooms.Add( _room.Id, _room );
            
        }


        private World _world;
        private Room _room;
    }
}
