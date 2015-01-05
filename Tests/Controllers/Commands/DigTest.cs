using Kaerber.MUD.Controllers.Commands.Editor;
using Kaerber.MUD.Entities;

using Moq;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Controllers.Commands
{
    [TestFixture]
    public class DigTest
    {
        [Test]
        public void ConnectLinksTwoRoomsWithMatchingExits()
        {
            var mockRoom1 = new Mock<Room>();
            var mockRoom2 = new Mock<Room>();

            mockRoom1.Setup( room => room.AddExit( "west", mockRoom2.Object ) );
            mockRoom2.Setup( room => room.AddExit( "east", mockRoom1.Object ) );

            Dig.Connect( mockRoom1.Object, mockRoom2.Object, "west" );

            mockRoom1.VerifyAll();
            mockRoom2.VerifyAll();
        }
    }
}
