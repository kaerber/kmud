using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Kaerber.MUD.Entities.Aspects {
    public class Equipment {
        public Character Host { get; set; }

        public Equipment() {
            Stuff = new Dictionary<WearLocation, Item>();
        }

        public Equipment( Dictionary<WearLocation, Item> source ) {
            Stuff = source;
        }

        public Dictionary<WearLocation, Item> Stuff { get; private set; }

        [Pure]
        public virtual bool Have( WearLocation location ) {
            return Stuff.ContainsKey( location );
        }

        [Pure]
        public virtual bool Have( Item item ) {
            return Stuff.ContainsValue( item );
        }

        public virtual void Equip( Item item ) {
            Stuff.Add( item.WearLoc, item );
        }

        public virtual void Remove( Item item ) {
            if( !Stuff.ContainsValue( item ) )
                return;
            Host.Room.Event( "ch_removed_item",
                             EventReturnMethod.None,
                             new EventArg( "ch", Host ),
                             new EventArg( "item", item )
                );

            Stuff.Remove( item.WearLoc );
            Host.Inventory.Add( item );
        }

        public void Clear() {
            Stuff.Clear();
        }

        [Pure]
        public int Count => Stuff.Keys.Count;

        [Pure]
        public IEnumerable<Item> Items => Stuff.Values;

        [Pure]
        public virtual Item Get( WearLocation location ) {
            return Stuff.ContainsKey( location ) ? Stuff[location] : null;
        }

        [Pure]
        public Item RightHand => Get( WearLocation.RightHand );

        public virtual void ReceiveEvent( Event e ) {
            foreach( var item in Items )
                item.ReceiveEvent( e );
        }
    }
}
