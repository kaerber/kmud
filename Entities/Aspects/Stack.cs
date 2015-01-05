using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

using Kaerber.MUD.Common;

namespace Kaerber.MUD.Entities.Aspects {
    public class Stack : ISerialized {
        private int _count;
        private int _max;

        public int MaxCount { get { return ( _max ); } }

        public int Count { get { return ( _count ); } }

        public bool IsFull { get { return ( Count >= MaxCount && MaxCount > 0 ); } }

        
        public Stack() {}

        public Stack( Stack template ) {
            _max = template.MaxCount;
        }

        public Stack( int maxCount ) {
            _max = maxCount;
        }

        public ISerialized Deserialize( IDictionary<string, object> data ) {
            _max = World.ConvertToType<int>( data["MaxCount"] );
            _count = World.ConvertToType<int>( data["Count"] );

            return( this );
        }

        public IDictionary<string, object> Serialize() {
            return( new Dictionary<string, object>()
                .AddEx( "MaxCount", MaxCount )
                .AddEx( "Count", Count ) );
        }

        public int Add( int volume ) {
            Contract.Requires( volume >= 0 );

            var diff = MaxCount > 0
                ? Math.Min( MaxCount - Count, volume )
                : volume;
            _count += diff;
            return ( diff );
        }

        public int Remove( int volume ) {
            Contract.Requires( volume >= 0 );

            var diff = Math.Min( _count, volume );
            _count -= diff;

            return ( diff );
        }
    }
}
