using Microsoft.Practices.Unity;

using Kaerber.MUD.Controllers;
using Kaerber.MUD.Entities;
using Kaerber.MUD.Entities.Aspects;
using Kaerber.MUD.Server;
using Kaerber.MUD.Server.Managers;
using Kaerber.MUD.Telnet;
using Kaerber.MUD.Tests.Entities;
using Kaerber.MUD.Views;

using Moq;

namespace Kaerber.MUD.Tests.Acceptance {
    public class BaseAcceptanceTest : BaseEntityTest {
        protected Room Room;
        protected Character Model;
        protected CharacterController Controller;
        protected ICharacterView View;
        protected World World;

        protected CommandManager CommandManager;

        protected Mock<TelnetConnection> MockConnection;
        protected Mock<TelnetRenderer> MockRenderer;
        protected Mock<ITelnetInputParser> MockParser;

        protected virtual void CreateTestEnvironment() {
            World = new World();
            World.Instance = World;
            var configurator = UnityConfigurator.Configure();
            configurator.RegisterInstance( new Clock( 0 ) );
            World.Initialize( configurator );

            Room = AddTestRoom( "test", "Test", World );

            Controller = CreateTestCharacter( "test char", "test char", Room, World );
        }

        protected CharacterController CreateTestCharacter( string shortDescr, 
                                                           string names, 
                                                           Room room,
                                                           World world ) {
            Model = new Character { ShortDescr = shortDescr, Names = names };
            Model.SetRoom( room );
            Model.World = world;
            Model.Restore();

            MockConnection = new Mock<TelnetConnection>( null, null );
            MockConnection.Setup( c => c.Write( It.IsAny<string>() ) );

            MockRenderer = new Mock<TelnetRenderer>( Model );

            MockParser = new Mock<ITelnetInputParser>();

            View = new TelnetCharacterView( MockConnection.Object,
                                            Model,
                                            MockRenderer.Object,
                                            MockParser.Object );
            Model.ViewEvent += e => View.ReceiveEvent( e );

            CommandManager = new CommandManager();
 
            return new CharacterController( Model, View, CommandManager, null );
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