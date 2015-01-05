using System;

namespace Kaerber.MUD.Controllers.Commands.CharacterCommands {
    public class ShowEquipment : ICommand {
        public string Name {
            get { return "Equipment"; }
            set { throw new NotSupportedException(); }
        }

        public string Code {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public void Execute( ICharacterController pc, PlayerInput input ) {
            pc.View.Write( "Your equipment:\n" );
            pc.View.RenderEquipment( pc.Model.Eq );
        }

        public string ToString( string format, IFormatProvider formatProvider ) {
            return Name;
        }
    }
}
