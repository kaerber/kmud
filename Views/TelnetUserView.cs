using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kaerber.MUD.Common;
using Kaerber.MUD.Telnet;

namespace Kaerber.MUD.Views {
    public class TelnetUserView : IUserView {
        public TelnetUserView( TelnetConnection connection, IUser model ) {
            _connection = connection;
            _model = model;
        }


        /// <summary>
        /// A blocking sequence of commands received from Telnet connection and pre-parsed.
        /// </summary>
        public IEnumerable<string> Commands() {
            foreach( var line in _connection.ReadLines( Encoding.ASCII ) ) {
                if( _mapping.ContainsKey( line ) )
                    yield return _mapping[line];
                else {
                    _fail();
                    _action();
                }
            }
        }

        public void Start() {
            ( _action = () =>
                Write( "\n" )
                    .Write( "Please, select your character to login:\n" )
                    .Write( string.Join( "\n", _model.Characters
                            .Select( ( ch, i ) => $"{i + 1}. {ch}" ) ) ) )();

            _fail = () => Write( "No such character.\n" )
                .Write( "\n" );

            _mapping = _model.Characters.Select( ( name, key ) => new { name, key } )
                .ToDictionary( p => ( p.key + 1 ).ToString(), p => p.name );
        }

        public void Stop() {}

        private TelnetUserView Write( string message ) {
            _connection.Write( message );
            return this;
        }

        private readonly TelnetConnection _connection;
        private readonly IUser _model;
        private Dictionary<string, string> _mapping;
        private Action _action;
        private Action _fail;
    }
}
