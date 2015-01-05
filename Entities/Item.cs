using System;
using System.Collections.Generic;
using System.Diagnostics;

using Kaerber.MUD.Common;
using Kaerber.MUD.Entities.Aspects;

namespace Kaerber.MUD.Entities {
    [Flags]
    public enum ItemFlags {
        Merge = 1,
        Money = 2
    }

    public class Item : Entity {
        private Stack _stack;
        private WearLocation _wearLocation;

        public int Count {
            get { return ( _stack != null ? _stack.Count : 1 ); }
        }

        [MudEdit( "Max number of items in a stack, 0 is infinite, 1 is no stacking, X is max stack of X" )]
        public int MaxCount {
            get { return ( _stack != null ? _stack.MaxCount : 1 ); }
            set { _stack = value != 1 ? new Stack( value ) : null; }
        }

        public bool CanStack { get { return ( _stack != null ); } }
        public bool HasSpaceInStack { get { return ( Count < MaxCount || MaxCount == 0 ); } }
        public int TotalValue { get { return ( Count*Cost ); } }

        [MudEdit( "Where this item is worn" )]
        public virtual WearLocation WearLoc {
            get { return _wearLocation; }
            set { _wearLocation = value; }
        }

        [MudEdit( "Container properties of an item" )]
        public Container Container { get; set; }

        [MudEdit( "Weapon properties of an item", CustomType = "PythonObject", CustomTypeKey = "weapon" )]
        public dynamic Weapon { get; set; }

        [MudEdit( "Stats", CustomType = "PythonObject" )]
        public dynamic Stats { get; set; }

        [MudEdit( "Item flags " )]
        public ItemFlags Flags { get; set; }

        [MudEdit( "Cost of the item" )]
        public int Cost { get; set; }

        public Item() {}

        public Item( Item template ) : base( template.Id, template.Names, template.ShortDescr ) {
            _wearLocation = template.WearLoc;
            Flags = template.Flags;

            if( template.Stats != null )
                Stats = template.Stats.Clone();
            if( template.Weapon != null )
                Weapon = template.Weapon.Clone();

            if( template._stack != null )
                _stack = new Stack( template._stack );

            Cost = template.Cost;

            if( template.Container != null )
                Container = new Container( template.Container );
        }


        public override ISerialized Deserialize( IDictionary<string, object> data ) {
            WearLoc = World.ConvertToType<WearLocation>( data["WearLoc"] );
            Flags = World.ConvertToTypeExs<ItemFlags>( data, "Flags" );

            if( data.ContainsKey( "Stats" ) )
                Stats = AspectFactory.Stats().Deserialize( data["Stats"] );
            if( data.ContainsKey( "Weapon" ) )
                Weapon = AspectFactory.Weapon().Deserialize( data["Weapon"] );

            _stack = World.ConvertToTypeEx<Stack>( data, "Stack" );
            Cost = World.ConvertToTypeExs<int>( data, "Cost" );

            Container = World.ConvertToTypeEx<Container>( data, "Container" );

            return( base.Deserialize( data ) );
        }

        public override IDictionary<string, object> Serialize() {
            var data = base.Serialize()
                .AddEx( "WearLoc", WearLoc )
                .AddIf( "Container", Container, Container != null )
                .AddEx( "Flags", Flags )
                .AddIf( "Stack", _stack, _stack != null )
                .AddEx( "Cost", Cost );

            if( Stats != null )
                data.Add( "Stats", Stats.Serialize() );
            if( Weapon != null )
                data.Add( "Weapon", Weapon.Serialize() );
            return data;
        }

        public override void ReceiveEvent( Event e ) {
            base.ReceiveEvent( e );

            if( Stats != null )
                Stats.ReceiveEvent( e );
            if( Weapon != null )
                Weapon.ReceiveEvent( e );
        }

        public int AddQuantity( int quantity ) {
            return( _stack == null ? 0 : _stack.Add( quantity ) );
        }

        public int RemoveQuantity( int quantity ) {
            return ( _stack == null ? 0 : _stack.Remove( quantity ) );
        }
        
        public static Item Create( string vnum ) {
            Debug.Assert( World.Instance.Items.ContainsKey( vnum ) );
            return ( new Item( World.Instance.Items[vnum] ) );
        }
    }
}
