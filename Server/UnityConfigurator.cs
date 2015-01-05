using Kaerber.MUD.Common;
using Kaerber.MUD.Controllers.Commands;
using Kaerber.MUD.Server.Managers;

using Microsoft.Practices.Unity;

namespace Kaerber.MUD.Server {
    public class UnityConfigurator {
        public static UnityContainer Configure() {
            var container = new UnityContainer();
            container.RegisterInstance<IManager<ICommand>>( new CommandManager() );
            return container;
        }
    }
}
