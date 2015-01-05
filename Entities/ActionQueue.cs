using System.Collections.Generic;
using System.Diagnostics.Contracts;

using log4net;


namespace Kaerber.MUD.Entities
{
    public class ActionQueue : Queue<CharacterAction>
    {
        private static readonly ILog _logger = LogManager.GetLogger( typeof( ActionQueue ) );

        private readonly string _name;
        private readonly Character _host;

        private bool _onCooldown;

        public ActionQueue( string name, Character host )
        {
            Contract.Requires( host != null );
            Contract.Requires( !string.IsNullOrWhiteSpace( name ) );
            Contract.Ensures( _host != null );
            Contract.Ensures( !string.IsNullOrWhiteSpace( _name ) );

            _name = name;
            _host = host;
        }

        public virtual ActionQueue Add( CharacterAction action )
        {
            if( _onCooldown )
                return this;
            action.Setup( _host );
            Enqueue( action );
            _host.SetTimedEvent( 0, Execute );
            return this;
        }

        public void Execute()
        {
            if( _onCooldown ) return;

            if( Count == 0 )
            {
                _host.ReceiveEvent(
                    Event.Create( string.Format( "ch_is_able_to_perform_{0}", _name ),
                        EventReturnMethod.None,
                        new EventArg( "ch", _host ) )
                );
                return;
            }
            var action = Dequeue();
            _onCooldown = true;
            action.Execute();
            _host.SetTimedEvent( action.SharedCooldown, () =>
            {
                _onCooldown = false;
                Execute();
            } );
        }
    }
}
