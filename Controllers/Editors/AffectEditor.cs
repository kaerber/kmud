using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Kaerber.MUD.Entities;

namespace Kaerber.MUD.Controllers.Editors {
    public class AffectEditor : BaseEditor<AffectInfo> {
        private const string LastEditedKey = "aff_edit_last";

        public AffectEditor( CharacterController pc ) : base( pc ) {}

        public override string Name { get { return ( "affect" ); } }


        public override AffectInfo Current {
            get { return base.Current; }
            set {
                base.Current = value;
                pc.Model.Data[LastEditedKey] = value.Name;
            }
        }

        public override AffectInfo Value {
            get {
                return Current;
            }
        }

        protected override AffectInfo LastEdited {
            get {
                if( !pc.Model.Data.ContainsKey( LastEditedKey ) )
                    return ( null );

                var lastEdited = pc.Model.Data[LastEditedKey];

                if( !World.Instance.Affects.ContainsKey( lastEdited ) )
                    return null;
                return World.Instance.Affects[lastEdited];
            }
            set {
                if( !pc.Model.Data.ContainsKey( LastEditedKey ) )
                    pc.Model.Data.Add( LastEditedKey, string.Empty );
                pc.Model.Data[LastEditedKey] = value.Name;
            }
        }

        protected override IEnumerable<AffectInfo> List  { 
            get { return World.Instance.Affects.Values; } 
        }

        public override AffectInfo Create( string name ) {
            if( string.IsNullOrWhiteSpace( name ) ) {
                pc.View.Write( "Invalid affect name.\n" );
                return( null );
            }

            Current = new AffectInfo { Name = name };
            World.Instance.Affects.Add( name, Current );
            pc.View.WriteFormat( "{0} '{1}' created.\n", Name, name );
            return Current;
        }


        public override void Delete() {
            var old = Current;
            World.Instance.Affects.Remove( Current.Name );
            Current = World.Instance.Affects.ToList()[0].Value;

            pc.View.WriteFormat( "Affect '{0}' deleted, now editing '{1}'.\n", old.Name, Current.Name );
        }


        public override void ChangeTo( string name ) {
            AffectInfo target = null;
            if( !string.IsNullOrWhiteSpace( name ) )
                target = List.FirstOrDefault( item => 
                    item.Name.StartsWith( name, StringComparison.InvariantCultureIgnoreCase ) );

            if( target != null ) {
                Current = target;
                LastEdited = target;

                pc.View.WriteFormat( "Now editing {0} '{1}'.\n", Name, Current.Name );
            }
            else {
                pc.View.WriteFormat( "{0} [{1}] not found.\n", Name, name );
                Current = LastEdited;
            }
        }

        public override void Save() {
            World.Instance.SetDirty();
            World.Instance.Save();
            pc.View.WriteFormat( "Area {0} saved.\n", World.Instance.Id );
        }
    }
}
