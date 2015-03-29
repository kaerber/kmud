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
        public class TestCharacter {
            public Character Model;
            public CharacterController Controller;
            public ICharacterView View;
        }

        protected World World;
        protected Room TestRoom;
        protected TestCharacter TestChar;

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

            CommandManager = new CommandManager();
            CommandManager.Load();
 
            TestRoom = AddTestRoom( "test", "Test", World );

            TestChar = CreateTestCharacter( "test char", "test char", TestRoom, World );
        }

        protected TestCharacter CreateTestCharacter( string shortDescr, 
                                                string names, 
                                                Room room,
                                                World world ) {
            var model = new Character { ShortDescr = shortDescr, Names = names };
            model.SetRoom( room );
            model.World = world;
            model.Restore();

            MockConnection = new Mock<TelnetConnection>( null, null );
            MockConnection.Setup( c => c.Write( It.IsAny<string>() ) );

            MockRenderer = new Mock<TelnetRenderer>( model );

            MockParser = new Mock<ITelnetInputParser>();

            var view = new TelnetCharacterView( MockConnection.Object,
                                            model,
                                            MockRenderer.Object,
                                            MockParser.Object );
            model.ViewEvent += e => view.ReceiveEvent( e );

            var controller = new CharacterController( model, view, CommandManager, null );

            return new TestCharacter {
                Model = model,
                View = view,
                Controller = controller
            };
        }

        protected void TestModelEvent( string name, params EventArg[] args ) {
            TestChar.Model.ReceiveEvent( Event.Create( name, EventReturnMethod.None, args ) );
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