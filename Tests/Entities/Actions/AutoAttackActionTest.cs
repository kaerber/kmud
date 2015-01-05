using Kaerber.MUD.Entities;
using Kaerber.MUD.Entities.Actions;

using Moq;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Entities.Actions {
    [TestFixture]
    public class AutoAttackActionTest : BaseEntityTest {
        [Test]
        public void MakeAttackOnExecute() {
            var mockTarget = new Mock<Character>();

            var mockSelf = new Mock<Character>();
            mockSelf.Setup( self => self.MakeAttack( It.IsAny<IAttack>() ) );
            mockSelf.Setup( self => self.Target )
                .Returns( mockTarget.Object );

            var action = new AutoAttackAction();
            action.Setup( mockSelf.Object );

            action.Execute();

            mockSelf.VerifyAll();
        }
    }
}
