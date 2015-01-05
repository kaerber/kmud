using NUnit.Framework;

using Kaerber.MUD.Entities;

namespace Kaerber.MUD.Tests.Entities {
    [TestFixture]
    public class MLFunctionTest : BaseEntityTest {
        [Test]
        public void DeserializeTest() {
            var function = World.Serializer.Deserialize<MLFunction>( "{'Code':'import clr'}" );
            Assert.AreEqual( "import clr", function.Code );
        }

        [Test]
        public void SerializeTest() {
            var function = new MLFunction {
                Code = "import clr"
            };

            var data = World.Serializer.Serialize( function );
            Assert.AreEqual( "{\"Code\":\"import clr\"}", data );
        }

        [Test]
        public void ReturnValueTest() {
            var function = new MLFunction {
                Code = "import clr\nret_val = 1"
            };

            Assert.AreEqual( 1, function.Execute() );
        }
    }
}
