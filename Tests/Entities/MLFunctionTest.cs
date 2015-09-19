using NUnit.Framework;

using Kaerber.MUD.Entities;
using Newtonsoft.Json;

namespace Kaerber.MUD.Tests.Entities {
    [TestFixture]
    public class MLFunctionTest : BaseEntityTest {
        [Test]
        public void DeserializeTest() {
            var function = MLFunction.Deserialize( JsonConvert.DeserializeObject( "{'Code':'import clr'}" ) );
            Assert.AreEqual( "import clr", function.Code );
        }

        [Test]
        public void SerializeTest() {
            var function = new MLFunction {
                Code = "import clr"
            };

            var json = JsonConvert.SerializeObject( MLFunction.Serialize( function ) );
            Assert.AreEqual( "{\"Code\":\"import clr\"}", json );
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
