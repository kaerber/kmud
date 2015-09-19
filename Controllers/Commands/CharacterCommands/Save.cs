using System;
using System.Linq;

namespace Kaerber.MUD.Controllers.Commands.CharacterCommands {
    public class Save : ICommand {
        public string Name => "save";

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
                pc.View.WriteFormat( "Area of room {0} NOT saved.\n", pc.Model.Room.Id );
            }
        }

        public string ToString( string format, IFormatProvider formatProvider ) {
            return Name;
        }
    }
}
