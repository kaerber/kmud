using System;

namespace Kaerber.MUD.Controllers.Commands.CharacterCommands {
    public class Recall : ICommand {
        public string Name => "recall";

        public string Code {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public void Execute( ICharacterController pc, PlayerInput input ) {
            var self = pc.Model;
            if( self.Can( "recall" ) && self.TeleportToRoom( self.RespawnAt ) )
                self.Has( "recalled" );
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return Name;
        }
    }
}
