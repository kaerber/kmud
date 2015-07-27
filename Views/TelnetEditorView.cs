using System;
using System.Linq;

using Kaerber.MUD.Common;
using Kaerber.MUD.Telnet;

namespace Kaerber.MUD.Views {
    public class TelnetEditorView<T> : IEditorView<T> where T : IFormattable {
        public TelnetEditorView( IManager<T> manager, TelnetConnection connection ) {
            _manager = manager;
            _connection = connection;
        }

        public void List() {
            foreach( var entity in _manager.List( string.Empty )
                                           .Select( id => _manager.Load( string.Empty, id ) ) )
                _connection.Write( $"{entity:name}\n" );
        }

        private readonly IManager<T> _manager;
        private readonly TelnetConnection _connection;
    }
}
