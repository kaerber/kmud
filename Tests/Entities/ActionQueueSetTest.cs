using Kaerber.MUD.Entities;
using Kaerber.MUD.Server;
using Moq;

using NUnit.Framework;

namespace Kaerber.MUD.Tests.Entities
{
    [TestFixture]
    public class ActionQueueSetTest {
        [SetUp]
        public void SetUp() {
            UnityConfigurator.Configure();
        }

        public class ActionQueueSetWhiteBox: ActionQueueSet
        {
            public ActionQueueSetWhiteBox( Character host ) : base( host ) {}

            public void SetActionQueue( string name, ActionQueue queue )
            {
                Set.Add( name, queue );
            }
        }

        [Test]
        public void ActionIsForwardedToQueue()
        {
            var mockHost = new Mock<Character>();

            var mockAction = new Mock<CharacterAction>();

            var mockActionQueue = new Mock<ActionQueue>( "mock", mockHost.Object );
            mockActionQueue.Setup( actionQueue => actionQueue.Add( mockAction.Object ) );

            var queueSet = new ActionQueueSetWhiteBox( mockHost.Object );
            queueSet.SetActionQueue( "mock", mockActionQueue.Object );

            queueSet.EnqueueAction( "mock", mockAction.Object );

            mockActionQueue.VerifyAll();
        }

    }
}
