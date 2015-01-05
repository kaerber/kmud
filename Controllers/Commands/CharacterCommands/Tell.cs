using System;

namespace Kaerber.MUD.Controllers.Commands.CharacterCommands {
    public class Tell : ICommand {
        public string Name { get { return "Tell"; } }

        public string Code {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public void Execute( ICharacterController pc, PlayerInput input ) {
            // TODO fix
//            if( input.Arguments.Count() == 0 )
//            {
//                pc.View.Write( "Tell whom what?\n" );
//                return;
//            }
//
//            var vpc = ( PlayerController )( World.ActivePlayers.Find(
//                player => player.Model.ShortDescr.StartsWith( input.Arguments.ElementAt( 0 ), StringComparison.CurrentCultureIgnoreCase ) ) );
//            
//            if( vpc == null )
//            {
//                pc.View.Write( "You do not see any player by this name.\n" );
//                return;
//            }
//
//            if( input.Arguments.Count() < 2 )
//            {
//                pc.View.WriteFormat( "Tell {0} what?\n", vpc.Model.ShortDescr );
//                return;
//            }
//
//            pc.View.WriteFormat( "You tell {0} '{1}'.\n", vpc.Model.ShortDescr, string.Join( " ", input.Arguments.Skip( 1 ) ) );
//            vpc.View.WriteFormat( "{0} tells you '{1}'.\n", pc.Model.ShortDescr, string.Join( " ", input.Arguments.Skip( 1 ) ) );
//            vpc.LastTellFrom = pc.Model.ShortDescr;
        }

        public string ToString( string format, IFormatProvider formatProvider ) {
            return Name;
        }
    }
}
