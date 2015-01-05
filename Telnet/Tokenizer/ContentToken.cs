using System.Text;

namespace Kaerber.MUD.Telnet.Tokenizer {
    public class ContentToken : Token {
        public ContentToken( byte value ) {
            _value = value;
        }

        public override byte[] Encode() {
            if( _value == Command.Iac )
                return new byte[] { Command.Iac, Command.Iac };
            if( _value == '\n'.ToByte() )
                return new byte[] { 13, 10 };
            return new[] { _value };
        }

        public override string Content( Encoding encoding ) {
            return new string( encoding.GetChars( new[] { _value } ) );
        }

        private readonly byte _value;
    }
}
