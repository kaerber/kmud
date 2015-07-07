using System;

using Kaerber.MUD.Controllers.Hierarchy;
using Kaerber.MUD.Views;

namespace Kaerber.MUD.Controllers.Commands.Editor {
    public class EditField : ICommand {
        public const string Command_Describe = "?";
        public const string Command_Add = "+";
        public const string Command_Remove = "-";
        public const string Command_Set = "=";
        public const string Command_Look = "%";

        public enum CommandType {
            InvalidCommand,
            Look,
            Describe,
            Set,
            Add,
            Remove
        }

        public struct ParsedEditorInput {
            public HierarchyNode Node;
            public CommandType Command;
            public string Argument;
        }

        public static ParsedEditorInput ParseInput( PlayerInput input, object root ) {
            var result = new ParsedEditorInput();

            var node = HierarchyNode.CreateNode( string.Empty, root, null );
            if( string.IsNullOrWhiteSpace( input.Command ) ) {
                result.Node = node;
                result.Command = CommandType.Look;
                result.Argument = string.Empty;
                return result;
            }

            if( IsCommand( input.Command ) ) {
                result.Node = node;
                result.Command = ParseCommand( input.Command );
                result.Argument = input.RawArguments;
                return result;
            }

            var path = input.Command.Split( '.' );

            foreach( var t in path ) {
                var currentPath = node.Path + "." + t;
                node = node[t];
                if( node == null ) {
                    result.Node = null;
                    result.Command = CommandType.InvalidCommand;
                    result.Argument = currentPath;
                    return result;
                }
            }

            if( input.Arguments.Count == 0 ) {
                result.Node = node;
                result.Command = CommandType.Look;
                result.Argument = string.Empty;
                return result;
            }

            if( IsCommand( input.Arguments[0] ) ) {
                result.Node = node;
                result.Command = ParseCommand( input.Arguments[0] );
                var commandPosition = input.RawArguments.IndexOf( input.Arguments[0] );
                result.Argument = commandPosition + 2 < input.RawArguments.Length
                    ? input.RawArguments.Substring( commandPosition + 2 )
                    : string.Empty;
                return result;
            }

            result.Node = node;
            result.Command = CommandType.Set;
            result.Argument = input.RawArguments;
            return result;
        }

        private static bool IsCommand( string arg ) {
            return arg == Command_Look
                || arg == Command_Describe
                || arg == Command_Set
                || arg == Command_Add 
                || arg == Command_Remove;
        }

        public static CommandType ParseCommand( string command ) {
            switch( command ) {
                case Command_Look:
                    return CommandType.Look;

                case Command_Describe:
                    return CommandType.Describe;

                case Command_Set:
                    return CommandType.Set;

                case Command_Add:
                    return CommandType.Add;

                case Command_Remove:
                    return CommandType.Remove;

                default:
                    return CommandType.InvalidCommand;
            }
        }

        public string Name => "editfield";

        public string Code {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public void Execute( ICharacterController pc, PlayerInput input ) {
//            if( ( ( CharacterController )pc ).Editor.Value == null )
//            {
//                pc.View.Write( "Nothing to edit. Use 'create' to create a new object.\n" );
//                return;
//            }
//
//            var parsed = ParseInput( input, ( ( CharacterController )pc ).Editor.Value );
//
//            switch( parsed.Command ) {
//                case CommandType.Look:
//                    LookCommand( parsed, pc.View );
//                    break;
//
//                case CommandType.Describe:
//                    DescribeCommand( parsed, pc.View );
//                    break;
//
//                case CommandType.Set:
//                    SetCommand( parsed, pc.View );
//                    break;
//
//                case CommandType.Add:
//                    AddCommand( parsed, pc.View );
//                    break;
//
//                case CommandType.Remove:
//                    RemoveCommand( parsed, pc.View );
//                    break;
//
//                case CommandType.InvalidCommand:
//                    InvalidCommand( parsed, pc.View );
//                    break;
//
//                default:
//                    throw new InvalidOperationException( "Unknown command type." );
//            }
        }

        private static void LookCommand( ParsedEditorInput input, ICharacterView view ) {
            input.Node.Look( view );
        }

        private static void DescribeCommand( ParsedEditorInput input, ICharacterView view ) {
            input.Node.Describe( view );
        }

        private static void SetCommand( ParsedEditorInput input, ICharacterView view ) {
            try {
                input.Node.Set( input.Argument, view );
            }
            catch( NotSupportedException ex ) {
                view.Write( ex.Message + "\n" );
            }
        }

        private static void AddCommand( ParsedEditorInput input, ICharacterView view ) {
            input.Node.Add( input.Argument, view );
        }

        private static void RemoveCommand( ParsedEditorInput input, ICharacterView view ) {
            input.Node.Remove( input.Argument, view );
        }

        private static void InvalidCommand( ParsedEditorInput input, ICharacterView view ) {
            if( input.Node == null )
                view.WriteFormat( "No such property: {0}.\n", input.Argument );
            else
                view.WriteFormat( "Invalid command.\n" );
        }

        public string ToString( string format, IFormatProvider formatProvider ) {
            return Name;
        }
    }
}
