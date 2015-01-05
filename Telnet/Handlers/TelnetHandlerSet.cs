using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

using Kaerber.MUD.Telnet.Tokenizer;

namespace Kaerber.MUD.Telnet.Handlers {
    public class TelnetHandlerSet {
        public TelnetHandlerSet( TokenStream tokenStream,
                                 Func<Option, TelnetHandler> createDefaultHandler,
                                 params TelnetHandler[] handlers ) {
            Contract.Requires( createDefaultHandler != null );

            _tokenStream = tokenStream;
            _createDefaultHandler = createDefaultHandler;

            _handlers = new Dictionary<Option, TelnetHandler>();
            foreach( var handler in handlers )
                AddHandler( handler );
        }
        public virtual void SetRemote( Option option, bool state, TokenStream stream ) {
            if( !_handlers.ContainsKey( option ) )
                AddHandler( _createDefaultHandler( option ) );
            _handlers[option].SetRemote( state );
        }

        public virtual void Set( Option option, bool state, TokenStream stream ) {
            if( !_handlers.ContainsKey( option ) )
                AddHandler( _createDefaultHandler( option ) );
            _handlers[option].Set( state );
        }

        public virtual void ReceiveCommand( Option option, Command command ) {
            if( !_handlers.ContainsKey( option ) )
                AddHandler( _createDefaultHandler( option ) );
            _handlers[option].ReceiveCommand( command );
        }

        public TelnetHandler GetHandler( Option option ) {
            if( !_handlers.ContainsKey( option ) )
                return null;
            return _handlers[option];
        }

        private void AddHandler( TelnetHandler handler ) {
            _handlers.Add( handler.Option, handler );
            handler.TokenStream = _tokenStream;
        }


        private readonly TokenStream _tokenStream;
        private readonly Dictionary<Option, TelnetHandler> _handlers;
        private readonly Func<Option, TelnetHandler> _createDefaultHandler;

        public static TelnetHandlerSet Default( TokenStream tokenStream ) {
            return new TelnetHandlerSet( tokenStream,
                TelnetHandler.QMethodDisagreeingHandler,
                TelnetHandler.QMethodAgreeingHandler( Option.SuppressLocalEcho ),
                new EchoHandler() );
        }
    }
}
