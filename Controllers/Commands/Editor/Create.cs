using System;

namespace Kaerber.MUD.Controllers.Commands.Editor {
    public class Create : ICommand {
        public string Name => "create";

        public string Code {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public void Execute( ICharacterController pc, PlayerInput input ) {
//            ( ( CharacterController )pc ).Editor.Create( input.Arguments.Any() 
//                ? input.Arguments.ElementAt( 0 ) 
//                : string.Empty );
        }

        public string ToString( string format, IFormatProvider formatProvider ) {
            return Name;
        }
    }
}
