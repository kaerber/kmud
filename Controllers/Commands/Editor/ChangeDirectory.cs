using System;
using System.Linq;

namespace Kaerber.MUD.Controllers.Commands.Editor {
    public class ChangeDirectory : ICommand {
        public string Name => "changedirectory";

        public string Code {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public void Execute( ICharacterController pc, PlayerInput input )
        {
            if( input.Arguments.Count() != 1 )
            {
//                pc.View.WriteFormat( "Usage:\n\tcd <{0}>\n", ( ( CharacterController )pc ).Editor.Name );
                return;
            }

//            ( ( CharacterController )pc ).Editor.ChangeTo( input.Arguments.ElementAt( 0 ) );
        }

        public string ToString( string format, IFormatProvider formatProvider ) {
            return Name;
        }
    }
}
