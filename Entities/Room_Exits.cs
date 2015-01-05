using System.Diagnostics.Contracts;

using Kaerber.MUD.Common;
using Kaerber.MUD.Entities.Aspects;

namespace Kaerber.MUD.Entities {
    public partial class Room {
        private ExitSet _exits;

        [MudEdit( "Exits" )]
        public virtual ExitSet Exits {
            get { return _exits; }
            set { _exits = value; }
        }

        public virtual void AddExit( string name, Room to ) {
            Contract.Requires( !string.IsNullOrWhiteSpace( name ) );
            Contract.Requires( to != null );
            Contract.Ensures( Exits[name] != null );

            _exits.Add( new Exit( name, to ) );
        }
    }
}
