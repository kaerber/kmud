using Kaerber.MUD.Controllers;

using Moq;
using NUnit.Framework;

namespace Kaerber.MUD.Tests.Controllers {
    [TestFixture]
    public class ConnectionControllerStateMachineTest {
        [SetUp]
        public void SetUp() {
            _controller = new Mock<IConnectionController>();
            _machine = new ConnectionControllerStateMachine( _controller.Object );
        }

        [Test]
        public void FromStart_OnLogin_Login() {
            _controller.Setup( c => c.Login() );
            _machine.State = _machine.Start;

            _machine.State( "login" );

            _controller.Verify( c => c.Login(), Times.Once() );
            Assert.IsTrue( _machine.State == _machine.LoginGotUsername );
        }

        [Test]
        public void FromLogin_OnAny_LoginGotUsername() {
            _controller.Setup( c => c.LoginGotUsername( "username" ) );
            _machine.State = _machine.LoginGotUsername;

            _machine.State( "username" );

            _controller.Verify( c => c.LoginGotUsername( "username" ), Times.Once() );
            Assert.IsTrue( _machine.State == _machine.LoginGotPassword );
        }

        [Test]
        public void FromLoginGotUsername_OnAny_LoginGotPassword() {
            _controller.Setup( c => c.LoginGotPassword( "password" ) );
            _machine.State = _machine.LoginGotPassword;

            _machine.State( "password" );

            _controller.Verify( c => c.LoginGotPassword( "password" ), Times.Once() );
        }

        [Test]
        public void FromStart_OnRegister_Register() {
            _controller.Setup( c => c.Register() );
            _machine.State = _machine.Start;

            _machine.State( "register" );

            _controller.Verify( c => c.Register(), Times.Once() );
            Assert.IsTrue( _machine.State == _machine.RegisterGotUsername );
        }

        [Test]
        public void FromRegister_OnAny_RegisterGotUsername() {
            _controller.Setup( c => c.RegisterGotUsername( "username" ) );
            _machine.State = _machine.RegisterGotUsername;

            _machine.State( "username" );

            _controller.Verify( c => c.RegisterGotUsername( "username" ) );
            Assert.IsTrue( _machine.State == _machine.RegisterGotPassword );
        }

        [Test]
        public void FromStart_OnExit_Exit() {
            _machine.State = _machine.Start;

            _machine.State( "exit" );

            Assert.IsTrue( _machine.State == null );
        }

        private ConnectionControllerStateMachine _machine;
        private Mock<IConnectionController> _controller;
    }
}
