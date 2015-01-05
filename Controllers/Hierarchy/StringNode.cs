using System;
using System.Linq;

using Kaerber.MUD.Entities;
using Kaerber.MUD.Views;

namespace Kaerber.MUD.Controllers.Hierarchy {
    public class StringNode : HierarchyNode {
        public StringNode( string key, object value, HierarchyNode parent )
            : base( key, value, parent ) {}

        public override object DefaultValue { get { return ( string.Empty ); } }

        public override HierarchyNode this[string strIndex] {
            get {
                var index = GetIndex( strIndex );
                if( index == -1 )
                    return null;

                var list = ( ( string )Value ).Split( '\n' );
                return CreateNode( typeof( string ),
                    index.ToString(),
                    list[index],
                    this );
            }
        }

        public override HierarchyNode Add( string arg, ICharacterView view ) {
            var node = Set( Value + "\n" + arg, view )[( ( ( string )Value ).Split( '\n' ).Count() - 1 ).ToString()];
            return node;
        }

        public override HierarchyNode Set( string arg, ICharacterView view ) {
            if( Parent == null ) {
                Value = arg;
                return this;
            }

            Parent.SetMember( Key, arg );

            if( Parent is StringNode )
                view.WriteFormat( "{0} is set to:\n{1}\n", Parent.Path, Parent.Parent[ Parent.Key ].Value );
            else
                view.WriteFormat( "{0} is set to:\n{1}\n", Path, Parent[ Key ].Value );

            return Parent[Key];
        }

        public override void SetMember( string key, object value ) {
            var list = ( ( string )Value ).Split( '\n' );

            var index = GetIndex( key );
            if( index == -1 )
                throw new ArgumentException( "Invalid string index", "key" );

            list[index] = ( string )value;
            var newValue = string.Join( "\n", list );

            if( Parent == null ) {
                Value = newValue;
                return;
            }

            Parent.SetMember( Key, newValue );
        }

        public override HierarchyNode Remove( string key, ICharacterView view ) {
            var list = ( ( string )Value ).Split( '\n' ).ToList();

            var index = GetIndex( key );
            if( index == -1 )
                throw new ArgumentException( "Invalid string index", "key" );

            list.RemoveAt( index );
            var newValue = string.Join( "\n", list );

            if( Parent == null ) {
                Value = newValue;
                return null;
            }

            Parent.SetMember( Key, newValue );

            view.WriteFormat( "{0} is set to:\n{1}\n", Path, Parent[Key].Value );
            
            return this[Key];
        }

        private int GetIndex( string index ) {
            var str = Value as string;
            if( str.IndexOf( '\n' ) == -1 )
                return ( -1 );

            var list = str.Split( '\n' );

            int actualIndex;
            if( !int.TryParse( index, out actualIndex ) 
                    || actualIndex < 0 || list.Length <= actualIndex )
                return ( -1 );

            return actualIndex;
        }

        public override void Render( ICharacterView view, int level ) {
            var value = Value as string;
            if( string.IsNullOrWhiteSpace( value ) )
                return;

            RenderName( view, level );

            if( value.Contains( '\n' ) ) {
                view.Write( "\n" + string.Join( "\n",
                    value.Split( '\n' ).Select( ( line, index ) =>
                        string.Format( string.Format( "{{0, {0}}}: {{1}}", ( level + 1 )*2 + 4 ),
                            index.ToString(),
                            line ) ) ) + "\n" );
            }
            else
                view.Write( " " + value + " \n" );
        }

        public override void Describe( ICharacterView view ) {
            view.Write( "Commands:\n" );
            view.WriteFormat( "{0} - view this string\n", Path );
            view.WriteFormat( "{0}.<line number> - view the line\n", Path );
            view.WriteFormat( "{0} ? - this help\n", Path );
            view.WriteFormat( "{0} <value> - set value\n", Path );
            view.WriteFormat( "{0}.<line number> <value> - replace the line with a new one\n", Path );
            view.WriteFormat( "{0} + <value> - append a new line to the end of the string\n", Path );
            view.WriteFormat( "{0}.<line number> + <value> - insert a new line after the line\n", Path );
            view.WriteFormat( "{0} - <line number> - delete the line\n", Path );
        }
    }
}
