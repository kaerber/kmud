using System;

namespace Kaerber.MUD.Controllers.Commands.CharacterCommands {
    public class Sockets : ICommand {
        public string Name => "sockets";

        public string Code {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public void Execute( ICharacterController pc, PlayerInput input ) {
            // TODO fix
//            World.ActiveConnections.ForEach( conn => pc.View.WriteFormat( "{0}\t{1}\t{2}\n",
//                conn.IP,
//                ( World.ActiveUsers.Find( user => user.Session == conn ) ?? new Entities.User() { Username = "none" } )
//                    .Username,
//                ( World.ActivePlayers.Find( player => player.User.Session == conn ) != null
//                    ? World.ActivePlayers.Find( player => player.User.Session == conn ).Model.ShortDescr
//                    : "none"
//            ) ) );
        }

        public string ToString( string format, IFormatProvider formatProvider ) {
            return Name;
        }
    }
}
