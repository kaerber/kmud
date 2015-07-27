using System.IO;
using System.Linq;

using Kaerber.MUD.Common;
using Kaerber.MUD.Entities;
using Kaerber.MUD.Views;

using Microsoft.Practices.Unity;

namespace Kaerber.MUD.Controllers {
    public class UserController : IUserController {
        public UserController( IUser model, 
                               IUserView view, 
                               IManager<Character> characterManager,
                               IUnityContainer container ) {
            _model = model;
            _view = view;
            _characterManager = characterManager;
            _container = container;
            _machine = new UserControllerStateMachine( this );
        }

        public IController Start() {
            _view.Start();
            _machine.State = _machine.Start;
            return this;
        }

        public IController Run() {
            return _view.Commands()
                .Select( cmd => _machine.State( cmd ) )
                .FirstOrDefault( controller => controller != this );
        }

        public IController GotCharacterName( string name ) {
            var character = _characterManager.Load( Path.Combine( "players", _model.Username ), name );
            return NextController( character );
        }

        public IController Stop() {
            _view.Stop();
            return this;
        }


        private IController NextController( Character character ) {
            _container.RegisterInstance( character );
            return _container.Resolve<ICharacterController>();
        }

        private readonly IUser _model;
        private readonly IUserView _view;
        private readonly IManager<Character> _characterManager;

        private readonly IUnityContainer _container;

        private readonly UserControllerStateMachine _machine;
    }
}
