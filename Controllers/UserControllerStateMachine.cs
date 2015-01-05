namespace Kaerber.MUD.Controllers {
    public class UserControllerStateMachine {
        public UserControllerStateMachine( IUserController controller ) {
            _controller = controller;
        }

        public StateCall State { get; set; }

        public IController Start( string command ) {
            return _controller.GotCharacterName( command );
        }

        private readonly IUserController _controller;
    }
}
