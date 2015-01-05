using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Kaerber.MUD.Common;

namespace Kaerber.MUD.Entities
{
    public class HandlerSet : IDictionary<string, string>, IDictionary, ISerialized
    {
        public HandlerSet()
        {
            _map = new Dictionary<string, MLFunction>();
        }

        private Dictionary<string, MLFunction> _map;

        public void Execute( Event e, Func<IList<EventArg>> getArgs = null )
        {
            if( !ContainsKey( e.Name ) )
                return;
            var args = getArgs != null ? getArgs() : new List<EventArg>();
            var value = _map[ e.Name ].Execute( e.MergeParameters( args ) );
            if( value != null )
                e.ReturnValue = value;
        }

        #region IDictionary
        public void Add( string key, string value )
        {
            _map.Add( key, new MLFunction { Code = value } );
        }

        public bool ContainsKey( string key )
        {
            return ( _map.ContainsKey( key ) );
        }

        public ICollection<string> Keys
        {
            get { return ( _map.Keys ); }
        }

        public bool Remove( string key )
        {
            return( _map.Remove( key ) );
        }

        public bool TryGetValue( string key, out string value )
        {
            MLFunction function;
            if( _map.TryGetValue( key, out function ) )
            {
                value = function.Code;
                return( true );
            }
            value = string.Empty;
            return( false );
        }

        public ICollection<string> Values
        {
            get { return ( _map.Values.Select( func => func.Code ).ToList() ); }
        }

        public string this[string key]
        {
            get { return ( _map[key].Code ); }
            set { _map[key].Code = value; }
        }

        public void Add( KeyValuePair<string, string> item )
        {
            Add( item.Key, item.Value );
        }

        public void Clear()
        {
            _map.Clear();
        }

        public bool Contains( KeyValuePair<string, string> item )
        {
            if( !_map.ContainsKey( item.Key ) )
                return ( false );
            if( _map[item.Key].Code != item.Value )
                return ( false );
            return ( true );
        }

        public void CopyTo( KeyValuePair<string, string>[] array, int arrayIndex )
        {
            _map.ToDictionary( pair => pair.Key, pair => pair.Value.Code ).ToList().CopyTo( array, arrayIndex );
        }

        public int Count
        {
            get { return ( _map.Count ); }
        }

        public bool IsReadOnly
        {
            get { return ( false ); }
        }

        public bool Remove( KeyValuePair<string, string> item )
        {
            return ( _map.Remove( item.Key ) );
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return ( _map.ToDictionary( pair => pair.Key, pair => pair.Value.Code ).GetEnumerator() );
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ( GetEnumerator() );
        }
        #endregion

        #region IDictionary members
        public void Add( object key, object value )
        {
            Add( ( string )key, ( string )value );
        }

        public bool Contains( object key )
        {
            return ( ( ( IDictionary )_map ).Contains( key ) );
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return ( ( IDictionaryEnumerator )GetEnumerator() );
        }

        public bool IsFixedSize
        {
            get { return ( ( ( IDictionary )_map ).IsFixedSize ); }
        }

        ICollection IDictionary.Keys
        {
            get { return ( ( ( IDictionary )_map ).Keys ); }
        }

        public void Remove( object key )
        {
            ( ( IDictionary )_map ).Remove( key );
        }

        ICollection IDictionary.Values
        {
            get { return ( ( ICollection )Values ); }
        }

        public object this[ object key ]
        {
            get
            {
                return ( this[ ( string )key ] );
            }
            set
            {
                this[ ( string )key ] = ( string )value;
            }
        }

        public void CopyTo( Array array, int index )
        {
            throw new NotImplementedException();
        }

        public bool IsSynchronized
        {
            get { return ( ( ( IDictionary )_map ).IsSynchronized ); }
        }

        public object SyncRoot
        {
            get { return ( ( ( IDictionary )_map ).SyncRoot ); }
        }
        #endregion

        #region ISerialized members
        public ISerialized Deserialize( IDictionary<string, object> data )
        {
            _map = World.ConvertToType<Dictionary<string, MLFunction>>( data[ "map" ] );
            return ( this );
        }

        public IDictionary<string, object> Serialize()
        {
            return (
                new Dictionary<string, object>()
                .AddEx( "map", _map )
            );
        }
        #endregion
    }
}
