using System.Text;

namespace Kaerber.MUD.Telnet {
    public static class Extensions {
        public static string GetLine( this StringBuilder self ) {
            var index = self.ToString().IndexOf( '\n' );
            if( index < 0 )
                return null;
            var line = self.ToString().Substring( 0, index );
            self.Remove( 0, index + 1 );
            return line.TrimEnd( '\r' );
        }

        public static byte ToByte( this char self ) {
            return Encoding.ASCII.GetBytes( new string( self, 1 ) )[0];
        }
    }
}
