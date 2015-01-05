using System;
using System.Collections;
using System.Linq;

using Kaerber.MUD.Common;
using Kaerber.MUD.Entities;
using Kaerber.MUD.Views;

namespace Kaerber.MUD.Controllers.Hierarchy
{
    public abstract class HierarchyNode
    {
        #region Static methods
        public static HierarchyNode CreateNode( string key, dynamic value, HierarchyNode parent )
        {
            return ( value != null
                ? CreateNode( value.GetType(), key, value, parent )
                : null );
        }

        protected static HierarchyNode CreateNode( Type type, string key, dynamic value, HierarchyNode parent )
        {
            if( IsEnum( type ) )
                return ( new EnumNode( key, value, parent ) );

            if( IsFlags( type ) )
                return ( new FlagsNode( key, value, parent ) );

            if( IsNumber( type ) )
                return ( new NumberNode( key, value, parent ) );

            if( type == typeof( string ) )
                return ( new StringNode( key, value, parent ) );

            if( IsList( type ) )
                return ( new ListNode( key, value, parent ) );

            if( IsDictionary( type ) )
                return ( new DictionaryNode( key, value, parent ) );

            if( IsComplexObject( type ) )
                return ( new ComplexNode( key, value, parent ) );

            if( IsPythonObject( type ) )
                return ( new PythonNode( key, value, parent ) );

            if( IsItemVnumNode( type ) )
                return ( new ItemVnumNode( key, value, parent ) );

            throw new NotSupportedException( 
                string.Format( "Unknown type {0} in {1}: {2}.",
                    type.Name, key, value ?? "null" ) );
        }

        public static bool IsEnum( Type type )
        {
            return ( type.IsEnum 
                && !Array.Exists( type.GetCustomAttributes( false ), a => a is FlagsAttribute )
            );
        }

        public static bool IsFlags( Type type )
        {
            return ( type.IsEnum
                && Array.Exists( type.GetCustomAttributes( false ), a => a is FlagsAttribute )
            );
        }

        public static bool IsNumber( Type type )
        {
            return ( type == typeof( int ) || type == typeof( long ) );
        }

        public static bool IsString( Type type )
        {
            return ( type == typeof( string ) );
        }

        public static bool IsList( Type type )
        {
            return ( type.GetInterfaces().Contains( typeof( IList ) ) );
        }

        public static bool IsDictionary( Type type )
        {
            return ( type.GetInterfaces().Contains( typeof( IDictionary ) ) );
        }

        public static bool IsComplexObject( Type type )
        {
            return (
                type.GetCustomAttributes( typeof( MudComplexTypeAttribute ), true ).Length > 0
                || type == typeof( Exit )
                || type.IsSubclassOf( typeof( Entity ) )
                || type == typeof( MLFunction )
                || type == typeof( SkillData ) );
        }

        public static bool IsPythonObject( Type type )
        {
            return ( type.Namespace == "IronPython.NewTypes.System" || type == typeof( IronPython.Runtime.Types.IPythonObject ) );
        }

        public static bool IsItemVnumNode( Type type )
        {
            return ( type == typeof( ItemVnum ) );
        }
        #endregion


        protected HierarchyNode( string key, object value, HierarchyNode parent )
        {
            Key = key;
            Value = value;
            Parent = parent;
        }

        public string Key { get; set; }
        public dynamic Value { get; set; }
        public abstract object DefaultValue { get; }
        public HierarchyNode Parent { get; set; }
        public string Path
        {
            get
            {
                return (
                    ( Parent != null && !string.IsNullOrWhiteSpace( Parent.Path )
                        ? Parent.Path + "."
                        : string.Empty
                    )
                    + Key );
            }
        }

        public abstract HierarchyNode this[string index] { get; }

        public abstract HierarchyNode Set( string arg, ICharacterView view );
        public abstract void SetMember( string key, object value );

        public abstract HierarchyNode Add( string arg, ICharacterView view );
        public abstract HierarchyNode Remove( string arg, ICharacterView view );

        public void Look( ICharacterView view )
        {
            Render( view, 1 );
        }

        public abstract void Render( ICharacterView view, int level );

        protected void RenderName( ICharacterView view, int level )
        {
            view.Write(
                string.Format(
                    string.Format( "{{0, -{0}}}{{1}}:", level*2 ),
                    string.Empty,
                    Key
                )
            );
        }

        public abstract void Describe( ICharacterView view );
    }
}
