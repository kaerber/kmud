using Ionic.Crc;

using Kaerber.MUD.Entities;
using Kaerber.MUD.Platform.Managers;
using Kaerber.MUD.Server;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Platform.Managers {
    [TestFixture]
    public class AreaManagerTest {
        [Test]
        public void LoadAreaTest() {
            World.Serializer = Launcher.InitializeSerializer();

            var characterManager = new CharacterManager( @"E:\Dev\Kaerber.MUD\Assets" );
            var manager = new AreaManager( @"E:\Dev\Kaerber.MUD\Assets",
                                           characterManager );
            var heaven = manager.Load( "areas", "heaven" );
        }
    }
}
