using System.Threading.Tasks;

using Kaerber.MUD.Common;
using Kaerber.MUD.Controllers;
using Kaerber.MUD.Server.Managers;
using Kaerber.MUD.Views;

using Microsoft.Practices.Unity;

namespace Kaerber.MUD.Server {
    public class TelnetSession : ISession {
        public TelnetSession( IUnityContainer container ) {
            _container = container;
        }

        public void Start() {
            var task = new Task( Run, TaskCreationOptions.LongRunning );
            task.Start();
        }

        public void Finish() {}

        public void Run() {
            IController controller = _container.Resolve<IConnectionController>();
            while( controller != null ) {
                controller.Start();
                var next = controller.Run();
                controller.Stop();

                controller = next;
            }
        }

        private readonly IUnityContainer _container;

        public static UnityContainer TelnetContainer() {
            var container = new UnityContainer();
            
            container.RegisterType<IConnectionController, ConnectionController>();
            container.RegisterType<IConnectionView, TelnetConnectionView>();
            
            container.RegisterType<IUserController, UserController>();
            container.RegisterType<IUserView, TelnetUserView>();

            container.RegisterType<ICharacterController, CharacterController>();
            container.RegisterType<ICharacterView, TelnetCharacterView>();
            container.RegisterType<ITelnetRenderer, TelnetRenderer>();

            container.RegisterType<TelnetSession, TelnetSession>();

            container.RegisterInstance<IUserManager>( UserManager.Instance );
            return container;
        }
   }
}
