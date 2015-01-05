using System;
using System.Collections.Generic;
using System.Linq;

using Kaerber.MUD.Entities;

namespace Kaerber.MUD.Controllers.Editors
{
    public class MobEditor : BaseEditor<Character> {
        private const string LastEditedKey = "mob_edit_last_vnum";

        public MobEditor( CharacterController pc ) : base( pc ) {}
       
        public override string Name { get { return ( "mob" ); } }

        public override Character Current
        {
            get { return base.Current; }
            set {
                if( value == null )
                    return;

                base.Current = value;
                pc.Model.Data[LastEditedKey] = value.Id;
            }
        }

        public override Character Value { get { return ( Current ); } }

        protected override Character LastEdited {
            get {
                if( !pc.Model.Data.ContainsKey( LastEditedKey ) )
                    return ( null );
                if( !World.Instance.Mobs.ContainsKey( pc.Model.Data[LastEditedKey] ) )
                    return ( null );
                return ( World.Instance.Mobs[pc.Model.Data[LastEditedKey]] );
            }
            set {
                if( !pc.Model.Data.ContainsKey( LastEditedKey ) )
                    pc.Model.Data.Add( LastEditedKey, string.Empty );
                pc.Model.Data[LastEditedKey] = value.Id;
            }
        }

        protected override IEnumerable<Character> List { get { return ( World.Instance.Mobs.Values ); } }

        public override void PrintList() {
            foreach( var entity in List )
                pc.View.Write( string.Format( "{0:-30} {1}\n", "[" + entity.Id + "]", entity.ShortDescr ) );
        }


        public override Character Create( string vnum ) {
            var ch = pc.Model.Room.Area.AddMob( Character.Create() );
            ch.Id = vnum;
            Current = ch;
            
            pc.View.WriteFormat( "{0} [{1}] created.\n", Name, ch.Id );
            return Current;
        }


        public override void Delete() {
            World.Instance.Mobs.Remove( Current.Id );
            foreach( var area in World.Instance.Areas.Where( a => a.Mobs.Contains( Current ) ) )
                area.Mobs.Remove( Current );
            var newMob = World.Instance.Mobs.ToList()[0].Value;
            Current = newMob;
            pc.View.WriteFormat( "Mob [{0}] {1} deleted, now editing [{2}] {3}.\n", 
                Current.Id, Current.ShortDescr, newMob.Id, newMob.ShortDescr );
        }


        public override void ChangeTo( string vnum ) {
            Character target = null;
            if( !string.IsNullOrWhiteSpace( vnum ) ) {
                target = List.FirstOrDefault( entity => entity.Id.StartsWith( vnum,
                     StringComparison.CurrentCultureIgnoreCase ) );
            }

            if( target != null ) {
                Current = target;
                LastEdited = target;

                pc.View.WriteFormat( "Now editing {0} [{1}] {2}.\n", Name,
                    Current.Id, Current.ShortDescr );
            }
            else {
                pc.View.WriteFormat( "{0} [{1}] not found.\n", Name, vnum );
                Current = LastEdited;
            }
        }

        public override void Save() {
            var area = World.Instance.Areas.Find( mobContainer => mobContainer.Mobs.Contains( Current ) );
            area.SetDirty();
            area.Save();
            pc.View.WriteFormat( "Area {0} saved.\n", area.Id );
        }
    }
}
