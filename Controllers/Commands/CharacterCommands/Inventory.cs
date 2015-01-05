using System;

namespace Kaerber.MUD.Controllers.Commands.CharacterCommands {
    public class Inventory : ICommand {
        public string Name {
            get { return "Inventory"; }
            set { throw new NotSupportedException(); }
        }

        public string Code {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public void Execute( ICharacterController pc, PlayerInput input ) {
            pc.View.Write( "Your inventory:\n" );
            pc.View.RenderInventory( pc.Model );
        }

        public string ToString( string format, IFormatProvider formatProvider ) {
            return Name;
        }
    }
}
