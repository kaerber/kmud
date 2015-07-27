using System;

using Kaerber.MUD.Controllers;
using Kaerber.MUD.Entities;
using Kaerber.MUD.Entities.Aspects;
using Kaerber.MUD.Platform.Managers;
using Kaerber.MUD.Server;
using Kaerber.MUD.Telnet;
using Kaerber.MUD.Tests.Entities;
using Kaerber.MUD.Views;

using Microsoft.Practices.Unity;
using Microsoft.Practices.ObjectBuilder2;

using NUnit.Framework;
using Moq;

namespace Kaerber.MUD.Tests.Acceptance {
    public class BaseAcceptanceTest : BaseEntityTest {
        public void Test<T>( Action with, Func<T> on, IAssertConstructor<T> assertConstructor ) {
            with();
            assertConstructor.ConstructAssert()( on() );
        }

        public Action With( params Action[] actions ) {
            return () => actions.ForEach( action => action() );
        }

        public Func<T> On<T>( Func<T> action ) {
            return action;
        }


        protected void ConfigureTelnetEnvironment() {
            ConfigureTelnetContainer();
            CreateTestEnvironment();
        }

        protected void ConfigureTelnetContainer() {
            Container = TelnetSession.TelnetContainer( 
                UnityConfigurator.Configure() );
            Container.RegisterInstance( new Clock( 0 ) );

            MockConnection = new Mock<TelnetConnection>( null, null );
            Container.RegisterInstance( MockConnection.Object );

        }

        protected void CreateTestEnvironment() {
            World = new World();
            World.Instance = World;
            World.Initialize( Container );

            MUD.Server.Server.InitializeCommandManager( Container );
 
            TestRoom = AddTestRoom( "test", "Test", World );

            CreateTestPlayer();
        }

        protected void CreateTestPlayer() {
            PlayerModel = CreateTestCharacter( "test char", "test char", TestRoom, World );
            Container.RegisterInstance( PlayerModel );

            PlayerView = Container.Resolve<ICharacterView>();
            PlayerModel.ViewEvent += e => PlayerView.ReceiveEvent( e );

            PlayerController = Container.Resolve<ICharacterController>();
        }

        protected Character CreateTestCharacter( string shortDesc,
                                                 string names,
                                                 Room room,
                                                 World world ) {
            var model = new Character { ShortDescr = shortDesc, Names = names };
            model.SetRoom( room );
            model.World = world;
            model.Restore();
            return model;
        }

        protected Item CreateItem( string shortDescr, string names ) {
            return new Item {
                ShortDescr = shortDescr,
                Names = names
            };
        }

        protected void TestModelEvent( string name, params EventArg[] args ) {
            PlayerModel.ReceiveEvent( Event.Create( name, EventReturnMethod.None, args ) );
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

        protected void ItemsInRoom( params Item[] items ) {
            items.ForEach( item => TestRoom.Items.Add( item ) );
        }

        public Character PlayerModel;
        public ICharacterController PlayerController;
        public ICharacterView PlayerView;

        protected IUnityContainer Container;

        protected World World;
        protected Room TestRoom;

        protected CommandManager CommandManager;

        protected Mock<TelnetConnection> MockConnection;
        protected ITelnetRenderer Renderer;
        protected ITelnetInputParser Parser;
    }
}