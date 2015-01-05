using System.Text;

namespace Kaerber.MUD.Telnet.Tokenizer {
    public abstract class Token {
        public abstract byte[] Encode();

        public virtual string Content( Encoding encoding ) {
            return string.Empty;
        }

        public virtual bool Handle( TelnetConnection connection ) {
            return false;
        }
    }
}
