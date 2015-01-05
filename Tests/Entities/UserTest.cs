using Kaerber.MUD.Entities;
using Kaerber.MUD.Server;
using Kaerber.MUD.Server.Managers;

using NUnit.Framework;


namespace Kaerber.MUD.Tests.Entities {
    [TestFixture]
    public class UserTest {
        [Test]
        public void LoadUserTest() {
            World.Serializer = Launcher.InitializeSerializer();

            const string userName = "Luch1";

            var user = UserManager.Instance.LoadUser( userName );
            Assert.NotNull( user );
            Assert.AreEqual( user.Username, userName );
        }

        // Match password
    }
}
