using System;

using Kaerber.MUD.Common;
using Kaerber.MUD.Controllers.Commands;
using Kaerber.MUD.Entities;
using Kaerber.MUD.Platform.Managers;

using Microsoft.Practices.Unity;

namespace Kaerber.MUD.Server {
    public class UnityConfigurator {
        public static UnityContainer Configure() {
            var container = new UnityContainer();
            container.RegisterInstance<IManager<ICommand>>( new CommandManager( World.CommandsPath ) );
            container.RegisterInstance<IManager<Character>>( new CharacterManager( World.AssetsRootPath ) );

            container.RegisterInstance( new Clock( DateTime.Now.Ticks ) );

            return container;
        }
    }
}
