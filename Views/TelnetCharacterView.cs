using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kaerber.MUD.Entities;
using Kaerber.MUD.Entities.Aspects;
using Kaerber.MUD.Telnet;

namespace Kaerber.MUD.Views {
    public class TelnetCharacterView : ICharacterView {
        public TelnetCharacterView( TelnetConnection connection, 
                                    Character model,
                                    ITelnetRenderer renderer ) {
            _model = model;
            _model.ViewEvent += ReceiveEvent;

            _view = AspectFactory.View();
            _view.view = this;
            _view.model = _model;

            _connection = connection;
            _renderer = renderer;
        }

        public bool Command { get; set; }

        /// <summary>
        /// A blocking sequence of commands received from Telnet connection and pre-parsed.
        /// </summary>
        public IEnumerable<string> Commands() {
            return _connection.ReadLines( Encoding.ASCII );
        }

        public void Start() {
            RenderRoom( _model.Room );
        }

        public void Stop() {}

        public void Quit() {
            _connection.Close();
        }

        public void WritePrompt() {
            _renderer.RenderPrompt( Write );
            _outputDirty = false;
        }

        public void Write( string message ) {
            _connection.Write( message );
            _outputDirty = true;
        }

        public void WriteFormat( string message, params object[] args ) {
            Write( string.Format( message, args ) );
        }

        public void WriteMformat( string format, params object[] args ) {
            Write( mformat( format, args ) );
        }

        public void RenderItem( Item item ) {
            _renderer.Render( item, Write );
        }

        public void RenderCharacter( Character character ) {
            _renderer.Render( character, Write );
        }

        public void RenderEquipment( Equipment equipment ) {
            _renderer.Render( equipment, Write );
        }

        public void RenderInventory( Character character ) {
            _renderer.Render( character.Inventory, false, Write );
        }

        public void RenderRoom( Room room ) {
            _renderer.Render( room, Write );
        }


        public void ReceiveEvent( Event e ) {
            _view.ReceiveEvent( e );

            if( e.Name == "round" && _outputDirty || e.Name == "prompt" )
                WritePrompt();

            if( e.Name == "before_enter_password" )
                _connection.SetOutputVisibility( false );

            if( e.Name == "after_enter_password" )
                _connection.SetOutputVisibility( true );
       }

        private string mformat( string format, params object[] args ) {
            var result = format;

            for( var i = 0; i < args.Length; i++ ) {
                var index = i + 1;

                var ch = args[i] as Character;
                if( ch != null )
                {
                    var isSelf = _model == ch;
                    result = result
                        .Replace( IndexedParam( "$ch", index ), isSelf ? "you" : ch.ShortDescr )
                        .Replace( IndexedParam( "$s", index ), isSelf ? "" : "s" )
                        .Replace( IndexedParam( "$chs", index ), isSelf ? "your" : ch.ShortDescr + "'s" )
                        .Replace( IndexedParam( "$is", index ), isSelf ? "are" : "is" )
                        .Replace( IndexedParam( "$has", index ), isSelf ? "have" : "has" );
                    continue;
                }

                var item = args[i] as Item;
                if( item != null )
                {
                    var descr = item.ShortDescr;
                    if( item.Count > 1 )
                        descr = item.Count + " " +descr + "s";
                    result = result
                        .Replace( IndexedParam( "$item", index ), descr )
                        .Replace( IndexedParam( "$s", index ), "s" )
                        .Replace( IndexedParam( "$items", index ), descr + "'s" );
                    continue;
                }

                result = result.Replace( IndexedParam( "$str", index ), args[i].ToString() );
            }

            return ( result );
        }

        private static string IndexedParam( string paramName, int index ) {
            return ( paramName + "(" + index + ")" );
        }


        public string FormatMoney( IEnumerable<Item> money ) {
            return( string.Join( " and ",
                money.OrderBy( coin => coin.Cost ).Select( coin => string.Format( "{0} {1}{2}",
                    coin.Count,
                    coin.ShortDescr,
                    coin.Count == 1 ? "" : "s" ) )
                )
            );
        }

        private readonly Character _model;
        private bool _outputDirty;

        private readonly dynamic _view;
        private readonly TelnetConnection _connection;
        private readonly ITelnetRenderer _renderer;
    }
}
