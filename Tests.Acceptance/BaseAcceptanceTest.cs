using Kaerber.MUD.Controllers;
using Kaerber.MUD.Entities;
using Kaerber.MUD.Entities.Aspects;
using Kaerber.MUD.Telnet;
using Kaerber.MUD.Tests.Entities;
using Kaerber.MUD.Views;

using Moq;

namespace Kaerber.MUD.Tests.Acceptance
{
    public class BaseAcceptanceTest : BaseEntityTest {
        protected Room Room;
        protected Character Model;
        protected CharacterController Controller;
        protected ICharacterView View;
        protected World World;

        protected virtual void CreateTestEnvironment() {
            World = new World();
            World.Instance = World;

            Room = AddTestRoom( "test", "Test", World );

            Controller = CreateTestCharacter( "test char", "test char", Room, World );

            Model = Controller.Model;

            View = Controller.View;
        }

        protected CharacterController CreateTestCharacter( string shortDescr, 
                string names, 
                Room room,
                World world ) {
            var model = new Character { ShortDescr = shortDescr, Names = names };
            model.SetRoom( room );
            model.World = world;
            model.Restore();

            var mockConnection = new Mock<TelnetConnection>( null, null );
            mockConnection.Setup( c => c.Write( It.IsAny<string>() ) );

            var mockRenderer = new Mock<TelnetRenderer>( model );

            var view = new TelnetCharacterView( mockConnection.Object, model, mockRenderer.Object );
            model.ViewEvent += e => View.ReceiveEvent( e );

            return new CharacterController( model, view );
        }


        protected void TestModelEvent( string name, params EventArg[] args ) {
            Model.ReceiveEvent( Event.Create( name, EventReturnMethod.None, args ) );
        }
        
        
        protected Room AddTestRoom( string id, string shortDescription, World world ) {
            var room = new Room {
                Id = id,
                ShortDescr = shortDescription,
                UpdateQueue = new TimedEventQueue( null )
            };
            
            world.Rooms.Add( id, room );
            return room;
        }
    }
}
