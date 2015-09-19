using System.Linq;
using Kaerber.MUD.Platform.Managers;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Platform.Managers {
    [TestFixture]
    public class AreaManagerTest {
        [Test]
        public void LoadAreaTest() {
            var characterManager = new CharacterManager( @"E:\Dev\Kaerber.MUD\Assets" );
            var itemManager = new ItemManager( @"E:\Dev\Kaerber.MUD\Assets" );

            var manager = new AreaManager( @"E:\Dev\Kaerber.MUD\Assets",
                                           characterManager,
                                           itemManager );
            var heaven = manager.Load( "areas", "heaven" );
        }

        [Test]
        public void ListAreasTest() {
            var characterManager = new CharacterManager( @"E:\Dev\Kaerber.MUD\Assets" );
            var itemManager = new ItemManager( @"E:\Dev\Kaerber.MUD\Assets" );

            var manager = new AreaManager( @"E:\Dev\Kaerber.MUD\Assets",
                                           characterManager,
                                           itemManager );
            var areaNames = manager.List( "areas" );
        }

        [Test]
        public void LoadAllAreasTest() {
            var characterManager = new CharacterManager( @"E:\Dev\Kaerber.MUD\Assets" );
            var itemManager = new ItemManager( @"E:\Dev\Kaerber.MUD\Assets" );

            var manager = new AreaManager( @"E:\Dev\Kaerber.MUD\Assets",
                                           characterManager,
                                           itemManager );
            var areas = manager.List( "areas" )
                               .Select( name => manager.Load( "areas", name ) )
                               .ToList();
        }
    }
}
