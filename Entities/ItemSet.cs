using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Kaerber.MUD.Entities
{
    public class ItemSet : IList<Item>
    {
        private readonly List<Item> _list;

        
        public ItemSet() {
            _list = new List<Item>();
        }

        public ItemSet( IEnumerable<Item> src ) : this() {
            if( src != null )
                _list = new List<Item>( src );
        }

        #region Implementation of IEnumerable
        public IEnumerator<Item> GetEnumerator() {
            return ( _list.GetEnumerator() );
        }


        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
        #endregion


        #region Implementation of ICollection<Item>
        public void Add( Item item ) {
            Contract.Assert( item.Count > 0 );

            if( Contains( item ) )
                return;
                
            if( !item.CanStack )
                _list.Add( item );
            else
                Add( item, item.Count );
        }


        public void Clear() {
            _list.Clear();
        }


        public virtual bool Contains( Item item ) {
            return _list.Contains( item );
        }


        public void CopyTo( Item[] array, int arrayIndex ) {
            _list.CopyTo( array, arrayIndex );
        }


        public virtual bool Remove( Item item ) {
            if( item.CanStack )
                return Remove( item.Id, item.Count ) != null;
            return _list.Remove( item );
        }


        public int Count => _list.Count;

        public bool IsReadOnly => false;
        #endregion


        #region Implementation of IList<Item>
        public int IndexOf( Item item ) {
            return _list.IndexOf( item );
        }


        public void Insert( int index, Item item ) {
            _list.Insert( index, item );
        }


        public void RemoveAt( int index ) {
            _list.RemoveAt( index );
        }


        public Item this[ int index ] {
            get { return _list[index]; }
            set { throw new NotImplementedException( "Use add instead." ); }
        }
        #endregion

        public ItemSet Add( Item item, int count ) {
            Debug.Assert( item != null );
            Debug.Assert( count >= 1 );

            if( !item.CanStack ) {
                for( var i = 0; i < count; i++ ) {
                    _list.Insert( 0, item );
                    item = new Item( item );
                }
            }
            else {
                var stack = _list.Find( i => i.Id == item.Id && i.HasSpaceInStack )
                    ?? new Item( item );
                _list.Remove( stack );

                while( count > 0 ) {
                    count -= stack.AddQuantity( count );
                    _list.Insert( 0, stack );
                    stack = new Item( stack );
                }
            }

            return this;
        }

        public ItemSet Remove( string vnum, int count ) {
            Debug.Assert( !string.IsNullOrWhiteSpace( vnum ) );
            Debug.Assert( count >= 1 );

            var items = _list.FindAll( i => i.Id == vnum );
            if( items.Count == 0 )
                return null;

            var result = new ItemSet();

            var enumerator = items.GetEnumerator();
            while( count > 0 && enumerator.MoveNext() ) {
                var item = enumerator.Current;
                if( item.Count > count ) {  // item.count > count means this is a stack, and we should split it
                                            // after that, the job is done
                    var moved = new Item( item );
                    moved.AddQuantity( item.RemoveQuantity( count ) );
                    result.Add( moved );
                    break;
                }

                count -= item.Count;
                _list.Remove( item );
                result.Add( item );
            }
            return result;
        }

        public Item Find( Func<Item, bool> predicate ) {
            return this.FirstOrDefault( predicate );
        }

        public Item Find( string partialName, Func<Item, bool> filter = null ) {
            return Find( item => item.MatchNames( partialName ) 
                                 && ( filter == null || filter( item ) ) );
        }

        public Item Load( string vnum ) {
            var item = Item.Create( vnum );
            Add( item );
            return item;
        }

        public int CountItems( string vnum = "" ) {
            return this.Where( item => vnum == "" || item.Id == vnum )
                       .Sum( item => item.Count );
        }
    }
}
