using System;
using System.Collections.Generic;
using System.Linq;

using Kaerber.MUD.Entities;
using Kaerber.MUD.Entities.Aspects;

namespace Kaerber.MUD.Views {
    public class TelnetRenderer : ITelnetRenderer {
        public TelnetRenderer( Character pointOfView ) {
            _pointOfView = pointOfView;
        }

        public void Render( Item item, Action<string> write ) {
            write( string.Format( "\t{0}\n", item.ShortDescr ) );
        }

        public void Render( Equipment equipment, Action<string> write ) {
            foreach( var location in Enum.GetValues( typeof( WearLocation ) )
                                         .Cast<WearLocation>().Where( equipment.Have ) )
                write( string.Format( "{0,-30} {1}\n", WearLocationStrings.Format( location ),
                                      equipment.Get( location ).ShortDescr ) );
            
        }

        public void Render( IEnumerable<Item> items, bool longDescription, Action<string> write ) {
            var template = longDescription
                ? "    {0}{1} {2}{3} {4} lying here.\n"
                : "    {0}{1} {2}{3}\n";
            foreach( var item in items ) {
                var markerList = _pointOfView.Room.Event(
                    "ch_sees_markers_of_item", EventReturnMethod.List,
                    new EventArg( "ch", _pointOfView ), new EventArg( "item", item ) );
                var markers = ( ( List<dynamic> )markerList )
                    .Aggregate( string.Empty, ( current, marker ) => marker + " " + current );
                
                var multiple = item.Count > 1;
                write( string.Format( template,
                                      markers,
                                      multiple ? item.Count.ToString() : "a",
                                      item.ShortDescr,
                                      multiple ? "s" : string.Empty,
                                      multiple ? "are" : "is"
                ) );
            }
        }

        public void Render( Room room, Action<string> write ) {
            write( string.Format( "\t{0}\n", room.ShortDescr ) );
            write( room.Description + "\n" );

            write( string.Format( "[Exits: {0}]\n", string.Join( " ", room.Exits.Select( r => r.Name ) ) ) );

            Render( room.Items, true, write );

            room.Characters.Where( vch => vch != _pointOfView ).ToList().ForEach( 
                vch => write( string.Format( "{0,4}{1} is here.\n", string.Empty, vch.ShortDescr ) ) );
            
        }

        public void Render( Character character, Action<string> write ) {
            Render( character.Eq, write );
            write( "Inventory:\n" );
            Render( character.Inventory, false, write );
        }

        public void RenderPrompt( Action<string> write ) {
            var stats = AspectFactory.StatQuery();
            _pointOfView.Event( "query_stats", EventReturnMethod.None, 
                new EventArg( "stats", stats ) );

            write( "\n" );
            write( string.Format( "<{0}/{1}hp {2}/{3}mn>", stats.HP, stats.MaxHP, stats.MP, stats.MaxMP ) );
        }

        private readonly Character _pointOfView;
    }
}
