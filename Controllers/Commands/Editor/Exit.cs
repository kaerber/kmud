using System;

namespace Kaerber.MUD.Controllers.Commands.Editor {
    public class Exit : ICommand {
        public string Name {
            get { return "Exit"; }
            set { throw new NotImplementedException(); }
        }

        public string Code {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public void Execute( ICharacterController pc, PlayerInput input ) {
            pc.View.Write( "Switching back to normal mode.\n" );
        }

        public string ToString( string format, IFormatProvider formatProvider ) {
            return Name;
        }
    }
}
