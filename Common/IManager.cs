using System.Collections.Generic;

namespace Kaerber.MUD.Common {
    public interface IManager<T> {
        T Get( string name );

        IEnumerable<T> List();
        void Save( T entity );
    }
}
