using System;
using System.Collections;
using System.Linq;

using Kaerber.MUD.Entities;
using Kaerber.MUD.Views;

namespace Kaerber.MUD.Controllers.Hierarchy {
    public class PythonNode : HierarchyNode {
        public PythonNode( string key, object value, HierarchyNode parent ) 
            : base( key, value, parent ) {}

        public override object DefaultValue {
            get { return ( null ); }
        }

        public override HierarchyNode this[string key] {
            get {
                if( Value == null || string.IsNullOrWhiteSpace( key ) )
                    return null;

                var editables = ( IEnumerable )Value.Editables();
                key = editables.Cast<string>().Match( key );
                if( string.IsNullOrWhiteSpace( key ) )
                    return null;

                var childValue = Value.GetMember( key );
                if( childValue == null )
                    return null;

                var type = childValue.GetType();
                return HierarchyNode.CreateNode( type, key, childValue, this );
            }
        }


        public override HierarchyNode Set( string arg, ICharacterView view ) {
            return Value.SetValue( arg, view );
        }


        public override void SetMember( string key, object value ) {
            Value.SetMember( key, value );
        }


        public override HierarchyNode Add( string arg, ICharacterView view ) {
            Value.AddMember( arg, view );
            return this[arg];
        }


        public override HierarchyNode Remove( string arg, ICharacterView view ) {
            var result = this[arg];
            Value.RemoveMember( arg );
            return result;
        }


        public override void Render( ICharacterView view, int level ) {
            if( Value == null )
                return;

            if( !string.IsNullOrWhiteSpace( Key ) ) {
                RenderName( view, level );
                view.Write( "\n" );
                level++;
            }

            foreach( var member in Value.Members() )
                this[member].Render( view, level );
        }


        public override void Describe( ICharacterView view ) {
            throw new NotImplementedException();
        }
    }
}
