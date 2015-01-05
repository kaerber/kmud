using NUnit.Framework;

using Kaerber.MUD.Entities;

namespace Kaerber.MUD.Tests.Entities
{
    [TestFixture]
    public class EntityExceptionTest
    {
        [Test]
        public void ConstructorTest()
        {
            Assert.Throws<EntityException>( () => { throw new EntityException(); } );
            Assert.Throws<EntityException>( () => { throw new EntityException( "message" ); } );
            Assert.Throws<EntityException>( () => { throw new EntityException( "message", null ); } );
            Assert.Throws<EntityException>( () => { throw new EntityException( null, null ); } );
        }
    }
}
