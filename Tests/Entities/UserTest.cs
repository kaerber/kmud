using Kaerber.MUD.Entities;
using Kaerber.MUD.Platform.Managers;
using Kaerber.MUD.Server;

using Microsoft.Practices.Unity;

using NUnit.Framework;


namespace Kaerber.MUD.Tests.Entities {
    [TestFixture]
    public class UserTest {
        [Test]
        public void LoadUserTest() {
            var container = UnityConfigurator.Configure();
            World.Instance = container.Resolve<World>();

            const string userName = "Luch1";

            var user = new UserManager( World.UsersRootPath, new CharacterManager( World.AssetsRootPath ) )
                .Load( string.Empty, userName );
            Assert.NotNull( user );
            Assert.AreEqual( user.Username, userName );
        }

        // Match password
    }
}
