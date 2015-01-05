using System;
using System.Linq;

using Kaerber.MUD.Controllers.Editors;

namespace Kaerber.MUD.Controllers.Commands.CharacterCommands {
    public class Edit : ICommand {
        public string Name { get { return "edit"; } }

        public string Code {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public void Execute( ICharacterController pc, PlayerInput input ) {
            EditorFactory.Instance.StartEditor( ( ( CharacterController )pc ), 
                input.Arguments.ElementAt( 0 ), 
                input.Arguments.Count() > 1 ? input.Arguments.ElementAt( 1 ) : null );
        }

        public string ToString( string format, IFormatProvider formatProvider ) {
            return Name;
        }
    }
}
