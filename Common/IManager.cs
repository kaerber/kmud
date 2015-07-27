using System.Collections.Generic;

namespace Kaerber.MUD.Common {
    public interface IManager<T> {
        IList<string> List( string path );

        T Load( string path, string name );
        void Save( string path, T entity );
    }
}
