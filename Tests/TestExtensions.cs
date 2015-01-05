using System.Text;

namespace Kaerber.MUD.Tests
{
    public static class TestExtensions
    {
        public static byte[] ToBytes( this string self ) {
            return Encoding.ASCII.GetBytes( self.ToCharArray() );
        }
    }
}
