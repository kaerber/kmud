using System.IO;
using System.Linq;
using Kaerber.MUD.Platform.Managers;
using NUnit.Framework;

namespace Kaerber.MUD.Tests.Platform.Managers {
    [TestFixture]
    public class CharacterManagerTest {
        [Test]
        public void LoadAllPlayersTest() {
            var characterManager = new CharacterManager( @"E:\Dev\Kaerber.MUD\Assets" );
            var userManager = new UserManager( @"E:\Dev\Kaerber.MUD\Assets\players", characterManager );

            var players = Directory.GetDirectories( @"E:\Dev\Kaerber.MUD\Assets\players" )
                                   .Select( Path.GetFileName )
                                   .Select( n => userManager.Load( "", n ) )
                                   .SelectMany( u => u.Characters.Select( 
                                       ch => characterManager.Load( Path.Combine( "players", u.Username ), ch ) ) )
                                   .ToList();
        }
    }
}
