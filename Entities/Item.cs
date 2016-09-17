using System;
using System.Diagnostics;

using Kaerber.MUD.Entities.Aspects;

namespace Kaerber.MUD.Entities {
    [Flags]
    public enum ItemFlags {
        Merge = 1,
        Money = 2
    }

    [DebuggerDisplay( "Item {Id}" )]
    public class Item : Entity {
        public Stack Stack;

        public int Count => Stack?.Count ?? 1;

        public int MaxCount {
            get { return Stack?.MaxCount ?? 1; }
            set { Stack = value != 1 ? new Stack( value ) : null; }
        }

        public bool CanStack => Stack != null;
        public bool HasSpaceInStack => Count < MaxCount || MaxCount == 0;
        public int TotalValue => Count*Cost;

        public virtual WearLocation WearLoc { get; set; }

        public Container Container { get; set; }

        public dynamic Weapon { get; set; }

        public dynamic Stats { get; set; }

        public ItemFlags Flags { get; set; }

        public int Cost { get; set; }

        public Item() {}

        public Item( Item template ) : base( template.Id, template.Names, template.ShortDescr ) {
            WearLoc = template.WearLoc;
            Flags = template.Flags;

            if( template.Stats != null )
                Stats = template.Stats.Clone();
            if( template.Weapon != null )
                Weapon = template.Weapon.Clone();

            if( template.Stack != null )
                Stack = new Stack( template.Stack );

            Cost = template.Cost;

            if( template.Container != null )
                Container = new Container( template.Container );
        }

        public override void ReceiveEvent( Event e ) {
            base.ReceiveEvent( e );

            Stats?.ReceiveEvent( e );
            Weapon?.ReceiveEvent( e );
        }

        public int AddQuantity( int quantity ) {
            return Stack?.Add( quantity ) ?? 0;
        }

        public int RemoveQuantity( int quantity ) {
            return Stack?.Remove( quantity ) ?? 0;
        }
        
        public static Item Create( string vnum ) {
            Debug.Assert( World.Instance.Items.ContainsKey( vnum ) );
            return ( new Item( World.Instance.Items[vnum] ) );
        }
    }
}
