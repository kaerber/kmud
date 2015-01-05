using System;

namespace Kaerber.MUD.Controllers.Commands.CharacterCommands {
    public class Reply : ICommand {
        public string Name {
            get { return "Reply"; }
            set { throw new NotSupportedException(); }
        }

        public string Code {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public void Execute( ICharacterController pc, PlayerInput input ) {
            // TODO fixx
//            if( string.IsNullOrWhiteSpace( ( ( PlayerController )pc ).LastTellFrom ) )
//            {
//                pc.View.Write( "You have noone to reply to.\n" );
//                return;
//            }
//
//            if( string.IsNullOrWhiteSpace( input.RawArguments ) )
//            {
//                pc.View.WriteFormat( "Tell {0} what?\n", ( ( PlayerController )pc ).LastTellFrom );
//                return;
//            }
//
//            var vpc = ( PlayerController )( World.ActivePlayers.Find(
//                player => player.Model.ShortDescr == ( ( PlayerController )pc ).LastTellFrom ) );
//
//            if( vpc == null )
//            {
//                pc.View.Write( "You do not see any player by this name.\n" );
//                return;
//            }
//
//            pc.View.WriteFormat( "You tell {0} '{1}'.\n", vpc.Model.ShortDescr, input.RawArguments );
//            vpc.View.WriteFormat( "{0} tells you '{1}'.\n", pc.Model.ShortDescr, input.RawArguments );
//            vpc.LastTellFrom = pc.Model.ShortDescr;
        }

        public string ToString( string format, IFormatProvider formatProvider ) {
            return Name;
        }
    }
}
