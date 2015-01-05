using System;
using System.Collections.Generic;
using System.Linq;

using Kaerber.MUD.Entities;
using Kaerber.MUD.Views;

namespace Kaerber.MUD.Controllers.Commands {
    public delegate void CommandHandler( ICharacterController character, List<object> args );

    [Flags]
    public enum ArgType {
        Int      = 1,
        String   = 2,
        ObjRoom  = 16,
        ObjInv   = 32,
        ObjEq    = 64,
        ChRoom   = 256,
        ChWorld  = 512,
        ExitRoom = 4096

    }

    public abstract class Command : ICommand {
        protected Character Self;

        protected List<Tuple<List<ArgType>, CommandHandler>> _cmdForms;

        public List<Tuple<List<ArgType>, CommandHandler>> CmdForms { get { return ( _cmdForms ); } }

        public abstract string Name { get; }

        public string Code {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        protected string[] Messages = { null, null, null };

        public virtual void Execute( ICharacterController pc, PlayerInput input ) {
            Self = pc.Model;

            var executed = false;

            foreach( var cmdForm in CmdForms ) {
                List<object> argsParsed;
                if( !MatchArgSetToArgs( cmdForm.Item1, input.Arguments, out argsParsed ) )
                    continue;
                cmdForm.Item2( pc, argsParsed );
                executed = true;
                break;
            }

            if( !executed )
                PrintErrorMessage( pc.View, input.Arguments.Count() );
        }

        private bool MatchArgSetToArgs( List<ArgType> argSet, IEnumerable<string> args,
            out List<object> argsParsed ) {
            argsParsed = null;

            if( argSet.Count() != args.Count() )
                return false;

            var ap = new List<object>();
            for( var i = 0; i < argSet.Count; i++ )
            {
                object argParsed;
                if( ( argParsed = MatchArgToType( Self, args.ElementAt( i ), argSet[i] ) ) == null )
                    return false;
                ap.Add( argParsed );
            }

            argsParsed = ap;
            return true;
        }

        public static object MatchArgToType( Character self, string arg, ArgType type ) {
            object result;

            int nresult;
            if( type.HasFlag( ArgType.Int ) 
                && int.TryParse( arg, out nresult ) )
                return nresult;

            if( type.HasFlag( ArgType.String ) )
                return arg;

            if( type.HasFlag( ArgType.ObjEq )
                && ( result = self.Eq.Items.ToList().Find( obj => obj.MatchNames( arg ) ) ) != null )
                return result;

            if( type.HasFlag( ArgType.ObjInv )
                && ( result = self.Inventory.Find( obj => obj.MatchNames( arg ) ) ) != null )
                return result;

            if( type.HasFlag( ArgType.ObjRoom )
                && ( result = self.Room.Items.Find( obj => obj.MatchNames( arg ) ) ) != null )
                return result;

            if( type.HasFlag( ArgType.ChRoom )
                && ( result = self.Room.Characters.Find( vch => vch.MatchNames( arg ) ) ) != null )
                return result;

            if( type.HasFlag( ArgType.ExitRoom )
                && ( result = self.Room.Exits[arg] )!= null )
                return result;

            /*if( type.HasFlag( ArgType.ChWorld )
                && ( result = World.ActivePlayers.Find(
                    player => player.Model.ShortDescr.StartsWith( arg, StringComparison.CurrentCultureIgnoreCase ) ) ) != null )
                return result;*/ // TODO fix
            return null;
        }

        private void PrintErrorMessage( ICharacterView ch, int argCount ) {
            if( argCount >= 0 && argCount <= 2 && !string.IsNullOrWhiteSpace( Messages[argCount] ) ) {
                ch.Write( Messages[argCount] );
                return;
            }

            ch.Write( "Usages are:\n" );
            foreach( var cmdform in CmdForms ) {
                ch.Write( FormatUsage( cmdform ) + "\n" );
            }
        }

        private string FormatUsage( Tuple<List<ArgType>, CommandHandler> cmdform ) {
            string message = Name;
            foreach( var at in cmdform.Item1 ) {
                if( at.HasFlag( ArgType.ObjInv ) || at.HasFlag( ArgType.ObjRoom ) || at.HasFlag( ArgType.ObjEq ) )
                    message += " <object>";
                if( at.HasFlag( ArgType.ExitRoom ) )
                    message += " <exit>";
                if( at.HasFlag( ArgType.ChRoom ) || at.HasFlag( ArgType.ChWorld ) )
                    message += " <char>";
                if( at.HasFlag( ArgType.Int ) )
                    message += " <number>";
            }

            return message;
        }

        public string ToString( string format, IFormatProvider formatProvider ) {
            return Name;
        }
    }
}
