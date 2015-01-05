using System;

using Kaerber.MUD.Common;
using Kaerber.MUD.Telnet;

namespace Kaerber.MUD.Views {
    public class TelnetEditorView<T> : IEditorView<T> where T : IFormattable {
        public TelnetEditorView( IManager<T> manager, TelnetConnection connection ) {
            _manager = manager;
            _connection = connection;
        }

        public void List() {
            foreach( var name in _manager.List() )
                _connection.Write( string.Format( "{0:name}\n", name ) );
        }

        private readonly IManager<T> _manager;
        private readonly TelnetConnection _connection;
    }
}
