using System.Collections.Generic;
using System.IO;
using System.Linq;
using Kaerber.MUD.Common;
using Kaerber.MUD.Entities;
using Kaerber.MUD.Platform.Managers;
using NUnit.Framework;

namespace Kaerber.MUD.Tests.Platform.Managers {
    [TestFixture]
    public class CharacterManagerTest {
        [SetUp]
        public void Setup() {
            _abilityManager = new AbilityManager( new PythonManager(), @"E:\Dev\Kaerber.MUD\Python\abilities" );
            _characterManager = new CharacterManager( @"E:\Dev\Kaerber.MUD\Assets", _abilityManager );
            _userManager = new UserManager( @"E:\Dev\Kaerber.MUD\Assets\players", _characterManager );
        }

        [Test]
        public void LoadAllPlayersTest() {
            IList<Character> players;
            Assert.DoesNotThrow( () => {
                players = Directory.GetDirectories( @"E:\Dev\Kaerber.MUD\Assets\players" )
                                       .Select( Path.GetFileName )
                                       .Select( n => _userManager.Load( "", n ) )
                                       .SelectMany( u => u.Characters.Select(
                                           ch => _characterManager.Load( 
                                               Path.Combine( "players", u.Username ), ch ) ) )
                                       .ToList();

            } );
        }

        private IManager<IAbility> _abilityManager;
        private CharacterManager _characterManager;
        private UserManager _userManager;
    }
}
