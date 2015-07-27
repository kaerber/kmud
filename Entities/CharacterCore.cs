using System;
using System.Collections.Generic;
using System.Dynamic;

using Microsoft.Practices.ObjectBuilder2;

namespace Kaerber.MUD.Entities {
    public class CharacterCore : DynamicObject {
        public CharacterCore() {
            _dict = new Dictionary<string, IAbility>();
            EventSink = e => {};
        }

        public Action<Event> EventSink { get; set; }

        public override bool TryGetMember( GetMemberBinder binder, out object result ) {
            result = _dict.ContainsKey( binder.Name ) ? _dict[binder.Name] : null;
            return true;
        }

        public override bool TrySetMember( SetMemberBinder binder, object value ) {
            if( !( value is IAbility ) )
                return false;

            var ability = ( IAbility )value;
            _dict[binder.Name] = ability;
            ability.EventSink = e => EventSink( e );
            return true;
        }

        public void ReceiveEvent( Event e ) {
            _dict.Values.ForEach( a => a.ReceiveEvent( e ) );
        }

        private readonly Dictionary<string, IAbility> _dict;
    }
}
