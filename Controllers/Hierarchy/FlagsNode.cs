using System;
using System.Collections.Generic;
using System.Linq;

using Kaerber.MUD.Entities;
using Kaerber.MUD.Views;

namespace Kaerber.MUD.Controllers.Hierarchy {
    public class FlagsNode : HierarchyNode {
        public FlagsNode( string key, object value, HierarchyNode parent )
            : base( key, value, parent ) {}

        public override object DefaultValue {
            get { return 0; }
        }

        public override HierarchyNode this[string index] {
            get { throw new NotSupportedException( "Flags do not support members." ); }
        }

        public override HierarchyNode Set( string arg, ICharacterView view ) {
            foreach( var value in ParseValues( arg ) ) {
                if( ( ( int )Value&value ) == 0 )
                    SetFlag( value );
                else
                    ClearFlag( value );
            }
            view.WriteFormat( "{0} is set to {1}.\n", Path, Value );

            return this;
        }

        public override void SetMember( string key, object value ) {
            throw new NotSupportedException( "Flags do not support members." );
        }

        public override HierarchyNode Add( string arg, ICharacterView view ) {
            foreach( var value in ParseValues( arg ).Where( value => ( ( int )Value&value ) == 0 ) )
                SetFlag( value );

            view.WriteFormat( "{0} is set to {1}.\n", Path, Value );
            return this;
        }


        public override HierarchyNode Remove( string arg, ICharacterView view ) {
            foreach( var value in ParseValues( arg ).Where( value => ( ( int )Value & value ) != 0 ) )
                ClearFlag( value );

            view.WriteFormat( "{0} is set to {1}.\n", Path, Value );
            return this;
        }

        public override void Render( ICharacterView view, int level ) {
            RenderName( view, level );
            view.Write( " " + Value.ToString().Replace( ",", "" ) + " \n" );
        }

        public override void Describe( ICharacterView view ) {
            view.Write( "Commands:\n" );
            view.WriteFormat( "{0}.<field name> <flag1> <flag2> <flag3> - switch listed flags\n", Path );
            view.WriteFormat( "{0}.<field name> + <flag1> <flag2> <flag3> - set listed flags\n", Path );
            view.WriteFormat( "{0}.<field name> - <flag1> <flag2> <flag3> - clear listed flags\n", Path );
            view.Write( "\n" );
            view.Write( "Available values:\n" );
            view.Write( string.Join( " ", Enum.GetNames( Value.GetType() ) ) + "\n" );
        }


        private int MatchEnumValue( string arg ) {
            var strVal = ( ( string[] )Enum.GetNames( Value.GetType() ) ).Match( arg );
            if( string.IsNullOrWhiteSpace( strVal ) )
                return 0;
            return ( int )Enum.Parse( Value.GetType(), strVal );
        }


        private IEnumerable<int> ParseValues( string arg ) {
            return arg.Split( ' ' )
                    .Where( f => !string.IsNullOrWhiteSpace( f ) )
                    .Select( MatchEnumValue )
                    .Where( v => v != 0 );
        }

        private void SetFlag( int value ) {
            Value = ( int )Value|value;

            if( Parent == null )
                return;

            Parent.SetMember( Key, Value );
            Value = Parent[ Key ].Value;
        }


        private void ClearFlag( int value ) {
            Value = ( int )Value&~value;

            if( Parent == null )
                return;

            Parent.SetMember( Key, Value );
            Value = Parent[Key].Value;
        }
    }
}
