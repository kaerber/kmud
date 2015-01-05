using System;

using Kaerber.MUD.Entities;
using Kaerber.MUD.Views;

namespace Kaerber.MUD.Controllers.Hierarchy {
    public class EnumNode : HierarchyNode {
        public EnumNode( string key, object value, HierarchyNode parent )
            : base( key, value, parent ) {}

        public override object DefaultValue {
            get { return 0; }
        }

        public override HierarchyNode this[string index] {
            get { throw new NotSupportedException( "Enumerations do not support members." ); }
        }

        public override HierarchyNode Add( string name, ICharacterView view ) {
            throw new NotSupportedException( "Enumerations do not support members." );
        }

        public override HierarchyNode Set( string arg, ICharacterView view ) {
            var strVal = ( ( string[] )Enum.GetNames( Value.GetType() ) ).Match( arg );
            if( string.IsNullOrWhiteSpace( strVal ) )
                return null;

            var value = Enum.Parse( Value.GetType(), strVal );

            if( Parent == null ) {
                Value = value;
                return this;
            }

            Parent.SetMember( Key, value );

            view.WriteFormat( "{0} is set to {1}.\n", Path, strVal );
            return Parent[Key];
        }

        public override void SetMember( string key, object value ) {
            throw new NotSupportedException( "Enumerations do not support members." );
        }

        public override HierarchyNode Remove( string key, ICharacterView view ) {
            throw new NotSupportedException( "Enumerations do not support members." );
        }

        public override void Render( ICharacterView view, int level ) {
            RenderName( view, level );
            view.Write( " " + Value + " \n" );
        }

        public override void Describe( ICharacterView view ) {
            view.Write( "Commands:\n" );
            view.WriteFormat( "{0}.<field name> <value> - set field to value\n", Path );
            view.Write( "\n" );
            view.Write( "Available values:\n" );
            view.Write( string.Join( " ", Enum.GetNames( Value.GetType() ) ) + "\n" );
        }
    }
}
