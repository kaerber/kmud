using System.Linq;
using Kaerber.MUD.Common;
using Kaerber.MUD.Entities;
using Kaerber.MUD.Platform.Managers;
using Kaerber.MUD.Server;
using NUnit.Framework;

namespace Kaerber.MUD.Tests.Platform.Managers {
    [TestFixture]
    public class AreaManagerTest {
        [SetUp]
        public void Setup() {
            UnityConfigurator.Configure();
            _abilityManager = new AbilityManager();
            _characterManager = new CharacterManager( @"E:\Dev\Kaerber.MUD\Assets", _abilityManager );
            _itemManager = new ItemManager( @"E:\Dev\Kaerber.MUD\Assets" );
            _manager = new AreaManager( @"E:\Dev\Kaerber.MUD\Assets",
                                         _characterManager,
                                         _itemManager );
        }

        [Test]
        public void LoadAreaTest() {
            var heaven = _manager.Load( "areas", "heaven" );
        }

        [Test]
        public void ListAreasTest() {
            var areaNames = _manager.List( "areas" );
        }

        [Test]
        public void LoadAllAreasTest() {
            var areas = _manager.List( "areas" )
                                .Select( name => _manager.Load( "areas", name ) )
                                .ToList();
        }

        private IManager<IAbility> _abilityManager;
        private IManager<Character> _characterManager;
        private IManager<Item> _itemManager;
        private IManager<Area> _manager;
    }
}
