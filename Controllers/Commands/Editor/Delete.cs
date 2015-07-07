using System;

namespace Kaerber.MUD.Controllers.Commands.Editor {
    public class Delete : ICommand {
        public string Name => "Delete";

        public string Code {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public void Execute( ICharacterController pc, PlayerInput input ) {
//            ( pc as CharacterController ).Editor.Delete();
        }

        public string ToString( string format, IFormatProvider formatProvider ) {
            return Name;
        }
    }
}
