using System.IO;

using Kaerber.MUD.Entities;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Acceptance.Entities {
    [TestFixture]
    public class WorldAcceptance : BaseAcceptanceTest {
        [Test]
        public void Deserialize_IsWorking() {
            var world = World.Serializer.Deserialize<World>(
                File.ReadAllText( World.AssetsRootPath + "world.data" ) );
            
            Assert.IsNotNull( world );
        }

        [Test]
        public void LoadAreas_IsWorking() {
            World.Instance = World.Serializer.Deserialize<World>(
                File.ReadAllText( World.AssetsRootPath + "world.data" ) );
            World.Instance.LoadAreas();

            Assert.Greater( World.Instance.Areas.Count, 0 );
        }
    }
}
