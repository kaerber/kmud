using System;

namespace Kaerber.MUD.Controllers.Commands.CharacterCommands {
    public class Quit : ICommand {
        public string Name {
            get { return "Quit"; }
            set { throw new NotImplementedException(); }
        }

        public string Code {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public void Execute( ICharacterController pc, PlayerInput input ) {
            pc.Quit();
        }

        public string ToString( string format, IFormatProvider formatProvider ) {
            return Name;
        }
    }
}
