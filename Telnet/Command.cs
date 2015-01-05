using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Kaerber.MUD.Telnet
{
    public class Command {
        public static implicit operator Byte( Command command ) {
            return command._value;
        }

        public static readonly Command Se = new Command( "SE", 240 );
        public static readonly Command Nop = new Command( "NOP", 241 );
        public static readonly Command Dm = new Command( "DM", 242 );
        public static readonly Command Brk = new Command( "BRK", 243 );
        public static readonly Command Ip = new Command( "IP", 244 );
        public static readonly Command Ao = new Command( "AO", 245 );
        public static readonly Command Ayt = new Command( "AYT", 246 );
        public static readonly Command Ec = new Command( "EC", 247 );
        public static readonly Command El = new Command( "EL", 248 );
        public static readonly Command Ga = new Command( "GA", 249 );
        public static readonly Command Sb = new Command( "SB", 250 );
        public static readonly Command Will = new Command( "WILL", 251 );
        public static readonly Command Wont = new Command( "WONT", 252 );
        public static readonly Command Do = new Command( "DO", 253 );
        public static readonly Command Dont = new Command( "DONT", 254 );
        public static readonly Command Iac = new Command( "IAC", 255 );

        public bool Negotiation {
            get { return this == Do || this == Dont || this == Will || this == Wont; }
        }

        public bool Remote {
            get { return this == Do || this == Dont; }
        }

        public bool Local {
            get { return this == Will || this == Wont; }
        }

        public Command Peer() {
            return _peer[this];
        }

        public Command Negate() {
            return _negate[this];
        }

        private Command( string name, byte value ) {
            _name = name;
            _value = value;
        }

        public override string ToString() {
            return _name;
        }

        public byte[] Encode() {
            Contract.Requires( this != Iac );
            return new[] { Iac, _value };
        }


        public static bool IsNegotiation( byte data ) {
            return data == Do || data == Dont || data == Will || data == Wont;
        }

        public static Command Parse( byte value ) {
            return _commands.SingleOrDefault( command => command._value == value ) ?? Nop;
        }

        public static Command LocalRequest( bool request ) {
            return request ? Will : Wont;
        }

        public static Command RemoteRequest( bool request ) {
            return request ? Do : Dont;
        }


        private readonly byte _value;
        private readonly string _name;


        private static readonly List<Command> _commands = new List<Command>
            { Se, Nop, Dm, Brk, Ip, Ao, Ayt, Ec, El, Ga, Sb, Will, Wont, Do, Dont, Iac };

        private static readonly Dictionary<Command, Command> _peer =
            new Dictionary<Command, Command>
            { { Do, Will }, { Dont, Wont }, { Will, Do }, { Wont, Dont } };

        private static readonly Dictionary<Command, Command> _negate =
            new Dictionary<Command, Command>
            { { Do, Dont }, { Dont, Do }, { Will, Wont }, { Wont, Will } };
    }
}
