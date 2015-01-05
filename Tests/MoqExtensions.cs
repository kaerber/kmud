using Moq;


namespace Kaerber.MUD.Tests
{
    public static class MoqExtensions
    {
        public static Mock<T> Name<T>( this Mock<T> self, string name ) where T : class
        {
            self.Setup( s => s.ToString() )
                .Returns( name );
            return ( self );
        }
    }
}
