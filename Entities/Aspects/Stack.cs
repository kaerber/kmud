using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Kaerber.MUD.Entities.Aspects {
    public class Stack {
        public Stack() {}

        public int MaxCount => _max;
        public int Count => _count;
        public bool IsFull => Count >= MaxCount && MaxCount > 0;

        public Stack( Stack template ) {
            _max = template.MaxCount;
        }

        public Stack( int maxCount ) {
            _max = maxCount;
        }

        public int Add( int volume ) {
            Contract.Requires( volume >= 0 );

            var diff = MaxCount > 0
                ? Math.Min( MaxCount - Count, volume )
                : volume;
            _count += diff;
            return diff;
        }

        public int Remove( int volume ) {
            Contract.Requires( volume >= 0 );

            var diff = Math.Min( _count, volume );
            _count -= diff;

            return diff;
        }


        public static Stack Deserialize( dynamic data ) {
            return new Stack {
                _max = data.MaxCount,
                _count = data.Count
            };
        }

        public static IDictionary<string, object> Serialize( Stack stack ) {
            return new Dictionary<string, object> {
                ["MaxCount"] = stack.MaxCount,
                ["Count"] = stack.Count
            };
        }


        private int _count;
        private int _max;
    }
}
