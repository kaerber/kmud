using Kaerber.MUD.Entities;
using Kaerber.MUD.Server;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Entities {
    public class BaseEntityTest {
        [TestFixtureSetUp]
        public virtual void FixtureSetup() {
            Launcher.InitializeML();
            World.Serializer = Launcher.InitializeSerializer();
        }
    }
}
