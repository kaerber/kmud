using System;
using System.Collections.Generic;
using System.Linq;

using Kaerber.MUD.Controllers.Commands.Editor;
using Kaerber.MUD.Entities;

namespace Kaerber.MUD.Controllers.Editors {
    public class RoomEditor : BaseEditor<Room> {
        public RoomEditor( CharacterController ch ) : base( ch ) {}

       
        public override string Name { get { return "room"; } }

        protected override Dictionary<string, Commands.ICommand> CommandSet {
            get {
                var set =  base.CommandSet;
                set.Add( "dig", new Dig() );
                set.Add( "createarea", new CreateArea() );

                return set;
            }
        }


        public override Room Current {
            get { return pc.Model.Room; }
            set { pc.Model.SetRoom( value ); }
        }

        public override Room Value { get { return Current; } }

        protected override Room LastEdited {
            get { return pc.Model.Room; }
            set {}
        }

        protected override IEnumerable<Room> List { get { return World.Instance.Rooms.Values; } }

        public override void PrintList() {
            foreach( var entity in List )
                pc.View.Write( string.Format( "{0:-30} {1}\n", "[" + entity.Id + "]", entity.ShortDescr ) );
        }

        public override Room Create( string vnum ) {
            Current = pc.Model.Room.Area.AddRoom( new Room { Id = vnum } );
            pc.View.WriteFormat( "{0} [{1}] created.\n", Name, Current.Id );
            return Current;
        }


        public override void Delete() {
            World.Instance.Rooms.Remove( Current.Id );
            Current.Area.Rooms.Remove( Current );
            foreach( var r in World.Instance.Rooms.Where( r => r.Value.Exits[Current] != null ) )
                r.Value.Exits.Remove( r.Value.Exits[Current] );
            var newRoom = World.Instance.Rooms.ToList()[0].Value;   //TODO: new room is first exit from old room
            
            foreach( var ch in Current.Characters )
                ch.SetRoom( newRoom );

            pc.View.WriteFormat( "Room [{0}] {1} deleted, now editing [{2}] {3}.\n", 
                Current.Id, Current.ShortDescr, newRoom.Id, newRoom.ShortDescr );
        }


        public override void ChangeTo( string vnum ) {
            Room target = null;
            if( !string.IsNullOrWhiteSpace( vnum ) ) {
                target = List.FirstOrDefault( entity => entity.Id.StartsWith( vnum,
                    StringComparison.CurrentCultureIgnoreCase ) );
            }

            if( target != null ) {
                Current = target;
                LastEdited = target;
            }
            else {
                if( !string.IsNullOrWhiteSpace( vnum ) )
                    pc.View.WriteFormat( "{0} [{1}] not found.\n", Name, vnum );
                Current = LastEdited;
            }

            pc.View.WriteFormat( "Now editing {0} [{1}] {2}.\n", Name,
                Current.Id, Current.ShortDescr );
        }

        public override void Save() {
            var area = World.Instance.Areas.Find( roomContainer => roomContainer.Rooms.Contains( Current ) );
            area.SetDirty();
            area.Save();
            pc.View.WriteFormat( "Area {0} saved.\n", area.Id );
        }
    }
}
