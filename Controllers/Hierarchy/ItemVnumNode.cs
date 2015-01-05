using System;

using Kaerber.MUD.Entities;
using Kaerber.MUD.Views;

namespace Kaerber.MUD.Controllers.Hierarchy {
    public class ItemVnumNode : HierarchyNode {
        public ItemVnumNode( string key, object value, HierarchyNode parent ) 
            : base( key, value, parent ) {}

        public override object DefaultValue {
            get { return null; }
        }


        public override HierarchyNode this[ string index ] {
            get { throw new NotImplementedException( "Item vnum does not support members." ); }
        }


        public override HierarchyNode Set( string arg, ICharacterView view ) {
            ItemVnum value;
            try {
                value = ItemVnum.FromString( arg );
            }
            catch( EntityException ) {
                view.WriteFormat( "No item with vnum [{0}].", arg );
                return null;
            }

            if( Parent == null ) {
                Value = value;
                return this;
            }

            Parent.SetMember( Key, value );
            return Parent[Key];
        }


        public override void SetMember( string key, object value ) {
            throw new NotImplementedException( "Item vnums do not support members." );
        }


        public override HierarchyNode Add( string arg, ICharacterView view ) {
            throw new NotImplementedException( "Item vnums do not support members." );
        }


        public override HierarchyNode Remove( string arg, ICharacterView view ) {
            throw new NotImplementedException( "Item vnums do not support members." );
        }


        public override void Render( ICharacterView view, int level ) {
            RenderName( view, level );
            view.Write( " " + Value + " \n" );
        }


        public override void Describe( ICharacterView view ) {
            view.Write( "Commands:\n" );
            view.WriteFormat( "{0} - observe this field\n", Path );
            view.WriteFormat( "{0} ? - this help\n", Path );
            view.WriteFormat( "{0} <vnum> - set this field to value\n", Path );
        }
    }
}
