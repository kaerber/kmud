using System;

using Kaerber.MUD.Common;
using Kaerber.MUD.Controllers.Commands;
using Kaerber.MUD.Entities;
using Kaerber.MUD.Server.Managers;

using Microsoft.Practices.Unity;

namespace Kaerber.MUD.Server {
    public class UnityConfigurator {
        public static UnityContainer Configure() {
            var container = new UnityContainer();
            container.RegisterInstance<IManager<ICommand>>( new CommandManager() );
            container.RegisterInstance<IManager<Character>>( new CharacterManager() );

            container.RegisterInstance( new Clock( DateTime.Now.Ticks ) );

            return container;
        }
    }
}
