using System;

using Kaerber.MUD.Entities;

namespace Kaerber.MUD.Controllers.Commands.CharacterCommands {
    public class Say : ICommand {
        public string Name {
            get { return "Say"; }
            set { throw new NotSupportedException(); }
        }

        public string Code {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public void Execute( ICharacterController pc, PlayerInput input ) {
            // TODO: train-wreck
            pc.Model.Event( "ch_said_text",
                            EventReturnMethod.None,
                            new EventArg( "ch", pc.Model ),
                            new EventArg( "text", input.RawArguments ) );
        }

        public string ToString( string format, IFormatProvider formatProvider ) {
            return Name;
        }
    }
}
