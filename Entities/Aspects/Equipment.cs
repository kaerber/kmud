using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

using Kaerber.MUD.Common;

namespace Kaerber.MUD.Entities.Aspects
{
    public class Equipment : ISerialized
    {
        private Dictionary<WearLocation, Item> _stuff;


        public Character Host { get; set; }

        public Equipment()
        {
            _stuff = new Dictionary<WearLocation, Item>();
        }

        [Pure]public virtual bool Have( WearLocation location ) { return ( _stuff.ContainsKey( location ) ); }

        [Pure]public virtual bool Have( Item item ) { return ( _stuff.ContainsValue( item ) ); }


        public virtual void Equip( Item item )
        {
            Contract.Requires( !Have( item.WearLoc ) );

            _stuff.Add( item.WearLoc, item );
        }

        public virtual void Remove( Item item )
        {
            if( !_stuff.ContainsValue( item ) )
                return;
            Host.Room.Event( "ch_removed_item",
                EventReturnMethod.None,
                new EventArg( "ch", Host ),
                new EventArg( "item", item )
            );

            _stuff.Remove( item.WearLoc );
            Host.Inventory.Add( item );
        }

        public void Clear()
        {
            _stuff.Clear();
        }

        [Pure]public int Count { get { return ( _stuff.Keys.Count ); } }

        [Pure]public IEnumerable<Item> Items { get { return ( _stuff.Values ); } }

        [Pure]
        public virtual Item Get( WearLocation location )
        {
            return( _stuff.ContainsKey( location ) ? _stuff[location] : null );
        }

        [Pure]
        public Item RightHand { get { return ( Get( WearLocation.RightHand ) ); } }
        [Pure]
        public Item Weapon { get { return ( RightHand != null && RightHand.Weapon != null ? RightHand : null ); } }


        public virtual void ReceiveEvent( Event e )
        {
            foreach( var item in Items )
                item.ReceiveEvent( e );
        }

        #region ISerialized members
        public ISerialized Deserialize( IDictionary<string, object> data )
        {
            _stuff = World.ConvertToType<Dictionary<string, Item>>( data )
                .ToDictionary(
                    pair => ( WearLocation )Enum.Parse( typeof( WearLocation ), pair.Key ),
                    pair => pair.Value );
            return ( this );
        }

        public IDictionary<string, object> Serialize()
        {
            return( 
                _stuff.Keys.Count > 0 
                    ? _stuff.ToDictionary(
                        pair => Enum.GetName( typeof( WearLocation ), pair.Key ),
                        pair => ( object )pair.Value )
                    : null
            );
        }
        #endregion
    }
}
