using Kaerber.MUD.Entities;
using Kaerber.MUD.Platform.Managers;
using Kaerber.MUD.Server;

using NUnit.Framework;


namespace Kaerber.MUD.Tests.Entities {
    [TestFixture]
    public class UserTest {
        [Test]
        public void LoadUserTest() {
            World.Serializer = Launcher.InitializeSerializer();

            const string userName = "Luch1";

            var user = new UserManager( World.UsersRootPath ).Load( string.Empty, userName );
            Assert.NotNull( user );
            Assert.AreEqual( user.Username, userName );
        }

        // Match password
    }
}
