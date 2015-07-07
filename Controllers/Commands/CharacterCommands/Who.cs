using System;

namespace Kaerber.MUD.Controllers.Commands.CharacterCommands {
    public class Who : ICommand {
        public string Name => "who";

        public string Code {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public void Execute( ICharacterController pc, PlayerInput input )
        {
            //World.ActivePlayers.ForEach( pl => pc.View.WriteFormat( "{0}\n", pl.Model.ShortDescr ) );
        }

        public string ToString( string format, IFormatProvider formatProvider ) {
            return Name;
        }
    }
}
