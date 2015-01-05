using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Kaerber.MUD.Entities;
using Kaerber.MUD.Entities.Aspects;


namespace Kaerber.MUD.Controllers.Editors {
    public class AspectEditor : BaseEditor<AspectInfo> {
        private const string LastEditedKey = "aspect_edit_last";

        public AspectEditor( CharacterController pc ) : base( pc ) {}

        public override string Name { get { return ( "aspect" ); } }


        public override AspectInfo Current {
            get { return base.Current; }
            set {
                if( !World.Instance.Aspects.ContainsKey( value.Name ) )
                    throw new ArgumentException( string.Format( "No such {0} as '{1}' exists.", Name, value ) );

                base.Current = value;
                pc.Model.Data[LastEditedKey] = value.Name;
            }
        }

        public override AspectInfo Value {
            get {
                return Current;
            }
        }

        protected override AspectInfo LastEdited {
            get {
                if( !pc.Model.Data.ContainsKey( LastEditedKey ) )
                    return null;

                var lastEdited = pc.Model.Data[LastEditedKey];

                if( !World.Instance.Aspects.ContainsKey( lastEdited ) )
                    return null;
                return World.Instance.Aspects[lastEdited];
            }
            set {
               if( !pc.Model.Data.ContainsKey( LastEditedKey ) )
                    pc.Model.Data.Add( LastEditedKey, string.Empty );
                pc.Model.Data[LastEditedKey] = value.Name;
            }
        }

        protected override IEnumerable<AspectInfo> List { 
            get { return World.Instance.Aspects.Values; } 
        }


        public override AspectInfo Create( string name ) {
            if( string.IsNullOrWhiteSpace( name ) ) {
                pc.View.WriteFormat( "Invalid {0} name.\n", Name );
                return null;
            }

            Current = new AspectInfo { Name = name };
            World.Instance.Aspects.Add( name, Current );
            pc.View.WriteFormat( "{0} '{1}' created.\n", Name, name );
            return Current;
        }


        public override void Delete() {
            var old = Current;
            World.Instance.Aspects.Remove( Current.Name );
            Current = World.Instance.Aspects.ToList()[0].Value;

            pc.View.WriteFormat( "{0} '{1}' deleted, now editing '{2}'.\n", Name, old, Current );
        }


        public override void ChangeTo( string name ) {
            AspectInfo target = null;
            if( !string.IsNullOrWhiteSpace( name ) )
                target = List.FirstOrDefault( a => 
                    a.Name.StartsWith( name, StringComparison.InvariantCultureIgnoreCase ) );

            if( target != null ) {
                Current = target;
                LastEdited = target;

                pc.View.WriteFormat( "Now editing {0} '{1}'.\n", Name, Current );
            }
            else {
                if( !string.IsNullOrWhiteSpace( name ) )
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
