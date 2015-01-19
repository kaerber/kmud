using System.Collections.Generic;
using System.Linq;

using Kaerber.MUD.Controllers;
using Kaerber.MUD.Controllers.Commands.CharacterCommands;
using Kaerber.MUD.Controllers.Commands.Editor;
using Kaerber.MUD.Entities;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Acceptance.Entities
{
    [TestFixture]
    public class CharacterAcceptance : BaseAcceptanceTest
    {
        [SetUp]
        protected override void CreateTestEnvironment() {
            base.CreateTestEnvironment();
        }

        /// <summary>
        /// Logged in character should load into room where he logged off, and room
        /// should have him in its character list.
        /// His update queue should be attached (to a room).
        /// </summary>
        [Test]
        public void CharIsDeserializedIntoRoom()
        {
            Model.SetRoom( null );

            var serialized = new Dictionary<string, object>
            {
                { "Vnum", "test-char" },
                { "Names", "test char" },
                { "ShortDescr", "test char" },
                { "LoginAt", Room.Id }
            };
            Model.Deserialize( serialized );

            Assert.AreEqual( Room, Model.Room );
            Assert.IsTrue( Room.Characters.Contains( Model ) );
            Assert.IsTrue( Model.UpdateQueue.IsAttached );
        }

        /// <summary>
        /// Character should login with the same health with which he logged out previously.
        /// </summary>
        [Test]
        public void CharacterLoginsWithStoredCurrentHealth()
        {
            Model.Restore();
            var wounds = Model.Health.Wounds;
            Assert.AreEqual( 0, wounds );

            var data = Model.Serialize();
            var strdata = World.Serializer.Serialize( data );
            data = World.Serializer.Deserialize<Dictionary<string, object>>( strdata );

            Model.SetRoom( null );
            Model = new Character { World = this.World };

            Model.Deserialize( data );
            Assert.AreEqual( wounds, Model.Health.Wounds );
        }

        /// <summary>
        /// When character moves from room to room, event ch_went_from_room_to_room must be
        /// fired in both rooms.
        /// </summary>
        [Test]
        public void MoveEventIsFiredWhenCharacterMoves()
        {
            var logFrom = new EventLogger();
            Room.Aspects["logger"] = logFrom;

            var roomTo = AddTestRoom( "room_to", "Room To", World );
            Dig.Connect( Room, roomTo, "south" );

            var logTo = new EventLogger();
            roomTo.Aspects["logger"] = logTo;

            var command = new Go();
            command.Execute( Controller, PlayerInput.Parse( "go south" ) );

            Assert.IsTrue( logFrom.Log.Count( e => e.Name == "ch_went_from_room_to_room") == 1 );
            Assert.IsTrue( logTo.Log.Count( e => e.Name == "ch_went_from_room_to_room") == 1 );
        }
    }
}
