using System.Collections.Generic;
using System.Linq;

namespace Kaerber.MUD.Entities.Aspects {
    public class ExitSet : List<Exit> {
        public ExitSet() {}
        public ExitSet( IEnumerable<Exit> collection ) : base( collection ) {}

        public virtual Exit this[string name] {
            get {
                return ( this.SingleOrDefault( e => e.Name == name )
                    ?? this.FirstOrDefault( e => e.Name.StartsWith( name ) )
                );
            }
        }

        public virtual Exit this[Room to] {
            get { return ( this.SingleOrDefault( e => e.To == to ) ); }
        }

        public virtual new void Add( Exit item ) {
            base.Add( item );
        }
    }
}
