namespace Kaerber.MUD.Controllers {
    public delegate IController StateCall( string command );

    public class ConnectionControllerStateMachine {
        public ConnectionControllerStateMachine( IConnectionController controller ) {
            _controller = controller;
        }

        public StateCall State { get; set; }

        public IController Start( string command ) {
            switch( command ) {
                case "login":
                    State = LoginGotUsername;
                    return _controller.Login();
                case "register":
                    State = RegisterGotUsername;
                    return _controller.Register();
                default:
                    State = null;
                    return null;
            }
        }

        public IController LoginGotUsername( string command ) {
            State = LoginGotPassword;
            return _controller.LoginGotUsername( command );
        }

        public IController LoginGotPassword( string command ) {
            return _controller.LoginGotPassword( command );
        }

        
        public IController RegisterGotUsername( string command ) {
            State = RegisterGotPassword;
            return _controller.RegisterGotUsername( command );
        }

        public IController RegisterGotPassword( string command ) {
            State = RegisterGotPasswordConfirmed;
            return _controller.RegisterGotPassword( command );
        }

        public IController RegisterGotPasswordConfirmed( string command ) {
            State = RegisterGotEmail;
            return _controller.RegisterGotPasswordConfirmed( command );
        }

        public IController RegisterGotEmail( string command ) {
            return _controller.RegisterGotEmail( command );
        }
        

        private readonly IConnectionController _controller;
    }
}
