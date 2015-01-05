using System;

namespace Kaerber.MUD.Controllers.Commands.Editor {
    public class Look : ICommand {
        public string Name {
            get { return "Look"; }
            set { throw new NotImplementedException(); }
        }

        public string Code {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public void Execute( ICharacterController pc, PlayerInput input ) {
            new EditField().Execute( pc, PlayerInput.Parse( EditField.Command_Look ) );
        }

        public string ToString( string format, IFormatProvider formatProvider ) {
            return Name;
        }
    }
}
