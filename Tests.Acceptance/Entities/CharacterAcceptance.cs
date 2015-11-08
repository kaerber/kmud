using Kaerber.MUD.Entities;
using Kaerber.MUD.Platform.Managers;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Kaerber.MUD.Tests.Acceptance.Entities
{
    [TestFixture]
    public class CharacterAcceptance : BaseAcceptanceTest
    {
        [SetUp]
        protected void Setup() {
            ConfigureTelnetEnvironment();
        }

        /// <summary>
        /// Character should login with the same health with which he logged out previously.
        /// </summary>
        [Test]
        public void CharacterLoginsWithStoredCurrentHealth()
        {
            PlayerModel.Restore();
            var wounds = PlayerModel.Health.Wounds;
            Assert.AreEqual( 0, wounds );

            var serializedData = CharacterManager.Serialize( PlayerModel );
            var strdata = JsonConvert.SerializeObject( serializedData );
            var deserializedData = JsonConvert.DeserializeObject( strdata );

            PlayerModel.SetRoom( null );
            PlayerModel = new Character { World = this.World };

            PlayerModel = CharacterManager.Deserialize( deserializedData, new CharacterCore() );
            Assert.AreEqual( wounds, PlayerModel.Health.Wounds );
        }
    }
}
