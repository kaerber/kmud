using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Kaerber.MUD.Common;
using Kaerber.MUD.Views;

namespace Kaerber.MUD.Controllers.Hierarchy {
    public class ComplexNode : HierarchyNode {
        public ComplexNode( string key, object value, HierarchyNode parent )
            : base( key, value, parent ) {}

        public override object DefaultValue { get { return null; } }

        public override HierarchyNode this[string index] {
            get {
                if( Value == null || string.IsNullOrWhiteSpace( index ) )
                    return null;

                var property = GetProperty( index );
                if( property == null )
                    return null;

                var value = property.GetValue( Value, null );
                var propertyType = GetPropertyType( property );
                return HierarchyNode.CreateNode( propertyType, property.Name, value, this );
            }
        }

        public override HierarchyNode Set( string arg, ICharacterView view ) {
            throw new NotSupportedException( "Complex objects do not support setting directly." );
        }

        public override void SetMember( string key, object value ) {
            var property = GetProperty( key );
            if( property == null )
                throw new InvalidOperationException( string.Format( "No member {0} in {1}.", key, Path ) );

            property.SetValue( Value, value, null );
        }

        public override HierarchyNode Add( string arg, ICharacterView view ) {
            var child = this[arg];
            if( child == null )
                return ( null );
            if( child.Value != null )
                return ( child );

            var property = GetProperty( arg );
            var childValue = property.GetValue( Value, null );
            if( childValue == null ) {
                if( property.PropertyType != typeof( string ) )
                    childValue = property.PropertyType.GetConstructor( new Type[0] ).Invoke( new object[0] );
                else
                    childValue = string.Empty;
                property.SetValue( Value, childValue, null );
                view.WriteFormat( "{0} is created.\n", this[ property.Name ].Path );
            }
            return this[property.Name];
        }

        public override HierarchyNode Remove( string arg, ICharacterView view ) {
            var child = this[arg];
            if( child == null )
                return( null );
            SetMember( child.Key, child.DefaultValue );
            view.WriteFormat( "{0} is removed.\n", child.Path );
            return child;
        }

        private PropertyInfo GetProperty( string name ) {
            return EditableProperties.FirstOrDefault( 
                p => p.Name.StartsWith( name, StringComparison.CurrentCultureIgnoreCase ) );
        }

        public override void Render( ICharacterView view, int level ) {
            if( Value == null )
                return;

            /* Avoid
             * __:
             * ____<field>:<value>
             * 
             * render instead
             * __<field>:<value> */
           if( !string.IsNullOrWhiteSpace( Key ) ) {
                RenderName( view, level );
                view.Write( "\n" );
                level++;
            }

            foreach( var property in EditableProperties ) {
                this[property.Name].Render( view, level );
            }
        }

        public override void Describe( ICharacterView view ) {
            view.Write( "Commands:\n" );
            view.WriteFormat( "{0} - observe this obect\n", Path );
            view.WriteFormat( "{0}.<field name> - observe field\n", Path );
            view.WriteFormat( "{0} ? - this help\n", Path );
            view.WriteFormat( "{0}.<field name> ? - this help for the field\n", Path );
            view.WriteFormat( "{0}.<field name> <value> - set field to value (for numbers, strings and enums)\n", Path );
            view.WriteFormat( "{0} + <field name> - create field (for structures, lists and dictionaries)\n", Path );
            view.WriteFormat( "{0} - <field name> - delete field (for structures, lists and dictionaries)\n", Path );
            view.Write( "\n" );
            view.Write( "Available fields:\n" );
            DescribeFields( view );
        }

        private void DescribeFields( ICharacterView view ) {
            foreach( var prop in EditableProperties ) {
                var attrArr = prop.GetCustomAttributes( typeof( MudEditAttribute ), true );
                view.WriteFormat( "{0,-40} {1}\n", prop.Name, ( ( MudEditAttribute )attrArr[0] ).Description );
            }
            view.Write( "\n" );
        }

        private IEnumerable<PropertyInfo> EditableProperties
        {
            get {
                return ( ( object )Value ).GetType().GetProperties().Where(
                    property => property.GetCustomAttributes( typeof( MudEditAttribute ), true ).Length > 0 );
            }
        }

        private static Type GetPropertyType( PropertyInfo property ) {
            var attr = ( MudEditAttribute )property.GetCustomAttributes( typeof( MudEditAttribute ), true )[0];
            switch( attr.CustomType ) {
                case "PythonObject":
                    return typeof( IronPython.Runtime.Types.IPythonObject );
                default:
                    return property.PropertyType;
            }
        }
    }
}
