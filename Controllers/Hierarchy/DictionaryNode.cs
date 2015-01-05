using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;

using Kaerber.MUD.Entities;
using Kaerber.MUD.Views;

namespace Kaerber.MUD.Controllers.Hierarchy {
    public class DictionaryNode : HierarchyNode {
        public DictionaryNode( string key, object value, HierarchyNode parent )
            : base( key, value, parent ) {}

        public override object DefaultValue { get { return null; } }

        public override HierarchyNode this[string index] {
            get {
                var dictionary = Value as IDictionary;
                var key = dictionary.Keys.Cast<string>().Match( index );
                return !string.IsNullOrEmpty( key )
                    ? CreateNode( dictionary[key].GetType(), key, dictionary[key], this )
                    : null;
            }
        }

        public override HierarchyNode Set( string arg, ICharacterView view ) {
            throw new NotSupportedException( "Dictionaries do not support setting directly." );
        }

        public override void SetMember( string key, object value ) {
            var list = Value as IDictionary;
            if( list == null )
                return;

            var index = GetKey( key );
            if( string.IsNullOrWhiteSpace( index ) )
                throw new ArgumentException( "Invalid dictionary key", "key" );

            list[index] = value;
        }

        public override HierarchyNode Add( string key, ICharacterView view ) {
            var dictionary = Value as IDictionary;
            if( dictionary == null )
                return null;

            var index = GetKey( key );
            if( !string.IsNullOrWhiteSpace( index ) )
                return null;

            Type itemType = Value.GetType().GetInterface( "IDictionary`2" ).GetGenericArguments()[1];
            if( itemType != typeof( string ) )
                dictionary.Add( key, itemType.GetConstructor( new Type[0] ).Invoke( new object[0] ) );
            else
                dictionary.Add( key, string.Empty );
            view.WriteFormat( "{0} is created.\n", this[ key ].Path );

            return this[key];
        }

        public override HierarchyNode Remove( string key, ICharacterView view )
        {
            var dictionary = Value as IDictionary;
            if( dictionary == null )
                return null;

            var index = GetKey( key );
            if( string.IsNullOrWhiteSpace( index ) )
                return null;

            var oldNode = this[index];
            dictionary.Remove( index );

            view.WriteFormat( "{0} is deleted.\n", oldNode.Path );
            return oldNode;
        }

        private string GetKey( string key ) {
            Debug.Assert( Value is IDictionary );

            if( string.IsNullOrWhiteSpace( key ) )
                return string.Empty;

            var dictionary = ( IDictionary )Value;
            return dictionary.Keys.Cast<string>().Match( key ) ?? string.Empty;
        }

        public override void Render( ICharacterView view, int level ) {
            var dictionary = Value as IDictionary;
            if( dictionary == null )
                return;

            RenderName( view, level );
            view.Write( "\n" );

            foreach( var key in dictionary.Keys.Cast<string>() )
                this[key].Render( view, level + 1 );
        }

        public override void Describe( ICharacterView view ) {
            view.Write( "Commands for dictionary:\n" );
            view.WriteFormat( "{0} - observe this obect\n", Path );
            view.WriteFormat( "{0}.<key> - observe element\n", Path );
            view.WriteFormat( "{0} ? - this help\n", Path );
            view.WriteFormat( "{0}.<key> ? - this help for an element\n", Path );
            view.WriteFormat( "{0} + <key> - create element\n", Path );
            view.WriteFormat( "{0} - <index> - delete element\n", Path );
            view.WriteFormat( "{0}.<index> <value> - set element to value (for numbers, strings and enums)\n", Path );
        }
    }
}
