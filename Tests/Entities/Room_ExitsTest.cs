using Kaerber.MUD.Entities;
using Kaerber.MUD.Entities.Aspects;

using Moq;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Entities
{
    [TestFixture]
    public class RoomExitsTest
    {
        [Test]
        public void AddAnExit()
        {
            var result = new Exit();
            var mockRoomTo = new Mock<Room>();
            mockRoomTo.Setup( r => r.Id )
                .Returns( "destination" );

            var mockExitSet = new Mock<ExitSet>();
            mockExitSet.Setup( exitSet => exitSet.Add( It.IsAny<Exit>() ) )
                .Callback<Exit>( exit => { result = exit; } );
            mockExitSet.Setup( exitSet => exitSet["west"] )
                .Returns( result );

            var room = new Room { Exits = mockExitSet.Object };
            room.AddExit( "west", mockRoomTo.Object );

            Assert.AreEqual( "west", result.Name );
            Assert.AreEqual( mockRoomTo.Object, result.To );
        }
    }
}
