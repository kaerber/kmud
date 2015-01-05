using System;

namespace Kaerber.MUD.Controllers.Commands.CharacterCommands {
    public class UnknownCommand : ICommand {
        public string Name {
            get { return "default"; }
            set { throw new NotImplementedException(); }
        }

        public string Code {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public void Execute( ICharacterController pc, PlayerInput input ) {
            pc.View.Write( "Huh?\n" );
        }

        public string ToString( string format, IFormatProvider formatProvider ) {
            return Name;
        }
    }
}
