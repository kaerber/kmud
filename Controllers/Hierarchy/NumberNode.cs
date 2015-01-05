using System;

using Kaerber.MUD.Views;

namespace Kaerber.MUD.Controllers.Hierarchy {
    public class NumberNode : HierarchyNode {
        public NumberNode( string key, object value, HierarchyNode parent )
            : base( key, value, parent ) {}

        public override object DefaultValue { get { return 0; } }

        public override HierarchyNode this[string index] {
            get { throw new NotSupportedException( "Numbers do not support members." ); }
        }

        public override HierarchyNode Add( string name, ICharacterView view ) {
            throw new NotSupportedException( "Numbers do not support members." );
        }

        public override HierarchyNode Set( string arg, ICharacterView view ) {
            int value;
            if( !int.TryParse( arg, out value ) )
                return null;

            if( Parent == null ) {
                Value = value;
                return this;
            }

            Parent.SetMember( Key, value );

            view.WriteFormat( "{0} is set to {1}.\n", Path, value );
            return Parent[Key];
        }

        public override void SetMember( string key, object value ) {
            throw new NotSupportedException( "Numbers do not support members." );
        }

        public override HierarchyNode Remove( string key, ICharacterView view ) {
            throw new NotSupportedException( "Numbers do not support members." );
        }

        public override void Render( ICharacterView view, int level ) {
            RenderName( view, level );
            view.Write( " " + Value + " \n" );
        }

        public override void Describe( ICharacterView view ) {
            view.Write( "Commands:\n" );
            view.WriteFormat( "{0} - observe this field\n", Path );
            view.WriteFormat( "{0} ? - this help\n", Path );
            view.WriteFormat( "{0} <number> - set this field to value\n", Path );
        }
    }
}
