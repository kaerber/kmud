using System;
using System.Collections;

using Kaerber.MUD.Entities;
using Kaerber.MUD.Views;

namespace Kaerber.MUD.Controllers.Hierarchy {
    public class ListNode : HierarchyNode {
        public ListNode( string key, object value, HierarchyNode parent )
            : base( key, value, parent ) {}

        public override object DefaultValue { get { return null; } }

        public override HierarchyNode this[string index] {
            get {
                var list = Value as IList;

                int actualIndex;
                if( !int.TryParse( index, out actualIndex ) 
                        || actualIndex < 0 
                        || list.Count <= actualIndex )
                    return ( null );
                return CreateNode( list[actualIndex].GetType(),
                    actualIndex.ToString(),
                    list[actualIndex],
                    this );
            }
        }

        public override HierarchyNode Set( string arg, ICharacterView view ) {
            throw new NotSupportedException( "Lists do not support setting directly." );
        }

        public override void SetMember( string key, object value ) {
            var list = Value as IList;
            if( list == null )
                return;

            var index = GetIndex( key );
            if( index == -1 )
                throw new ArgumentException( "Invalid list index", "key" );

            list[index] = value;
        }

        public override HierarchyNode Add( string key, ICharacterView view ) {
            var list = Value as IList;
            if( list == null )
                return null;

            var index = GetIndex( key );
            if( index == -1 )
                index = list.Count;

            Type listType = Value.GetType().GetGenericArguments()[0];
            if( listType != typeof( string ) )
                list.Insert( index, listType.GetConstructor( new Type[0] ).Invoke( new object[0] ) );
            else
                list.Insert( index, string.Empty );

            view.WriteFormat( "{0} is created.", this[ index.ToString() ].Path );
            return this[index.ToString()];
        }

        public override HierarchyNode Remove( string key, ICharacterView view ) {
            var list = Value as IList;
            if( list == null )
                return null;

            var index = GetIndex( key );
            if( index == -1 )
                return null;

            var oldNode = this[index.ToString()];
            list.RemoveAt( index );

            view.WriteFormat( "{0} is removed.", oldNode.Path );
            return oldNode;
        }

        private int GetIndex( string index ) {
            var list = Value as IList;

            int actualIndex;
            if( !int.TryParse( index, out actualIndex ) 
                    || actualIndex < 0 || list.Count <= actualIndex )
                return -1;

            return actualIndex;
        }

        public override void Render( ICharacterView view, int level ) {
            var list = Value as IList;
            if( list == null )
                return;

            RenderName( view, level );
            view.Write( "\n" );

            for( var i = 0; i < list.Count; i++ )
                this[i.ToString()].Render( view, level + 1 );
        }

        public override void Describe( ICharacterView view ) {
            view.Write( "Commands for list:\n" );
            view.WriteFormat( "{0} - observe this obect\n", Path );
            view.WriteFormat( "{0}.<index> - observe element\n", Path );
            view.WriteFormat( "{0} ? - this help\n", Path );
            view.WriteFormat( "{0}.<index> ? - this help for an element\n", Path );
            view.WriteFormat( "{0} + <index> - create element\n", Path );
            view.WriteFormat( "{0} - <index> - delete element\n", Path );
            view.WriteFormat( "{0}.<index> <value> - set element to value (for numbers, strings and enums)\n", Path );
        }
    }
}
