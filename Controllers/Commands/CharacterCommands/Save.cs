using System;
using System.Linq;

namespace Kaerber.MUD.Controllers.Commands.CharacterCommands {
    public class Save : ICommand {
        public string Name {
            get { return "Save"; }
            set { throw new NotSupportedException(); }
        }

        public string Code {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public void Execute( ICharacterController pc, PlayerInput input ) {
            if( !input.Arguments.Any() ) {
                ( ( CharacterController )pc ).SaveCharacter();
                pc.View.Write( "Saved.\n" );
            }
            else if( input.Arguments.ElementAt( 0 ) == "area" ) {
                pc.Model.Room.Area.Save();
                pc.View.WriteFormat( "Area {0} saved.\n", pc.Model.Room.Area.Id );
            }
        }

        public string ToString( string format, IFormatProvider formatProvider ) {
            return Name;
        }
    }
}
