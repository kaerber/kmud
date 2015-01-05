using Kaerber.MUD.Telnet.Tokenizer;

namespace Kaerber.MUD.Telnet.Handlers
{
    public class EchoHandler : TelnetHandler {
        public EchoHandler() : base( Option.Echo,
            HandlerStateAutomaton.QMethodYesLocal(),
            HandlerStateAutomaton.QMethodYesRemote() ) {
            Echo = true;
        }

        public bool Echo { get; set; }

        public override TokenStream TokenStream {
            protected get { return base.TokenStream; }
            set {
                base.TokenStream = value;
                base.TokenStream.TokenReceived += TokenReceived;
            }
        }

        private void TokenReceived( Token token ) {
            var t = token as ContentToken;
            if( t == null || !Echo || !LocalState.Enabled )
                return;
            base.TokenStream.Write( token );
        }
    }
}
