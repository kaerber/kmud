using System.Dynamic;
using System.Linq;

using Kaerber.MUD.Common;
using Kaerber.MUD.Views;

using Microsoft.Practices.Unity;

namespace Kaerber.MUD.Controllers {
    public class ConnectionController : IConnectionController {

        public ConnectionController( IUnityContainer container, 
                                     IConnectionView view, 
                                     IUserManager userManager ) {
            _container = container;
            _model = new ExpandoObject();
            _view = view;
            _machine = new ConnectionControllerStateMachine( this );
            _userManager = userManager;
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

        public IController Login() {
            _view.Login();
            return this;
        }

        public IController LoginGotUsername( string username ) {
            _view.LoginGotUsername();
            _model.Username = username;
            return this;
        }

        public IController LoginGotPassword( string password ) {
            _view.LoginGotPassword();
            _model.Password = password;

            if( _userManager.UserExists( _model.Username ) ) {
                var user = _userManager.LoadUser( ( string )_model.Username );
                if( user.CheckPassword( _model.Password ) )
                    return NextController( user );
            }

            _view.LoginFailed();
            return Start();
        }

        public IController Register() {
            _view.Register();
            return this;
        }

        public IController RegisterGotUsername( string username ) {
            if( _userManager.UserExists( username ) ) {
                _view.RegisterUsernameExists();
                return Start();
            }

            _model.Username = username;
            _view.RegisterGotUsername();
            return this;
        }

        public IController RegisterGotPassword( string password ) {
            _model.Password = password;
            _view.RegisterGotPassword();
            return this;
        }

        public IController RegisterGotPasswordConfirmed( string password ) {
            if( password != ( string )_model.Password ) {
                _view.RegisterPasswordNotConfirmed();
                return Start();
            }

            _view.RegisterGotPasswordConfirmed();
            return this;
        }

        public IController RegisterGotEmail( string email ) {
            var user = _userManager.CreateUser( ( string )_model.Username, ( string )_model.Password, email );
            _userManager.SaveUser( user );
            return NextController( user );
        }

        public IController Stop() {
            _view.Stop();
            return null;
        }


        private IController NextController( IUser user ) {
            _view.WelcomeUser( user );
            _container.RegisterInstance( user );
            return _container.Resolve<IUserController>();
        }

        private readonly dynamic _model;
        private readonly IConnectionView _view;
        private readonly ConnectionControllerStateMachine _machine;
        private readonly IUnityContainer _container;
        private readonly IUserManager _userManager;
    }
}
