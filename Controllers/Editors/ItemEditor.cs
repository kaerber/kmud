using System;
using System.Collections.Generic;
using System.Linq;

using Kaerber.MUD.Entities;

namespace Kaerber.MUD.Controllers.Editors {
    public class ItemEditor : BaseEditor<Item> {
        private const string LastEditedKey = "item_edit_last_vnum";

        public ItemEditor( CharacterController pc ) : base( pc ) {}

        public override string Name { get { return ( "item" ); } }

        public override Item Current {
            get { return base.Current; }
            set {
                if( value == null )
                    return;

                base.Current = value;
                pc.Model.Data[LastEditedKey] = value.Id;
            }
        }


        public override Item Value { get { return Current; } }

        protected override Item LastEdited {
            get {
                if( !pc.Model.Data.ContainsKey( LastEditedKey ) )
                    return ( null );
                if( !World.Instance.Items.ContainsKey( pc.Model.Data[LastEditedKey] ) )
                    return ( null );
                return ( World.Instance.Items[pc.Model.Data[LastEditedKey]] );
            }
            set {
                if( !pc.Model.Data.ContainsKey( LastEditedKey ) )
                    pc.Model.Data.Add( LastEditedKey, string.Empty );
                pc.Model.Data[LastEditedKey] = value.Id;
            }
        }

        protected override IEnumerable<Item> List { get { return ( World.Instance.Items.Values ); } }

        public override void PrintList() {
            foreach( var entity in List )
                pc.View.Write(
                    string.Format( "{0:-30} {1}\n", string.Format( "[{0}]", entity.Id ), entity.ShortDescr ) );
        }


        public override Item Create( string vnum ) {
            Current = pc.Model.Room.Area.AddItem( new Item { Id = vnum } );
            pc.View.WriteFormat( "{0} [{1}] created.\n", Name, Current.Id );
            return Current;
        }


        public override void Delete() {
            var item = Current;
            World.Instance.Items.Remove( item.Id );
            foreach( var area in World.Instance.Areas.Where( a => a.Items.Contains( item ) ) )
                area.Items.Remove( item );
            var newItem = World.Instance.Items.ToList()[ 0 ].Value;
            Current = newItem;
            pc.View.WriteFormat( "Item [{0}] {1} deleted, now editing [{2}] {3}.\n", item.Id, item.ShortDescr, newItem.Id, newItem.ShortDescr );
        }


        public override void ChangeTo( string vnum ) {
            Item target = null;
            if( !string.IsNullOrWhiteSpace( vnum ) ) {
                target = List.FirstOrDefault( entity => entity.Id.StartsWith( vnum,
                   StringComparison.CurrentCultureIgnoreCase ) );
            }

            if( target != null ) {
                Current = target;
                LastEdited = target;

                pc.View.WriteFormat( "Now editing {0} [{1}] {2}.\n", Name,
                    Current.Id, Current.ShortDescr );
            }
            else {
                pc.View.WriteFormat( "{0} [{1}] not found.\n", Name, vnum );
                Current = LastEdited;
            }
        }

        public override void Save() {
            var area = World.Instance.Areas.Find( itemContainer => itemContainer.Items.Contains( Value ) );
            area.SetDirty();
            area.Save();
            pc.View.WriteFormat( "Area {0} saved.\n", area.Id );
        }
    }
}
