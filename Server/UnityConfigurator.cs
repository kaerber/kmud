using System;
using System.Configuration;
using Kaerber.MUD.Common;
using Kaerber.MUD.Controllers.Commands;
using Kaerber.MUD.Entities;
using Kaerber.MUD.Platform.Managers;

using Microsoft.Practices.Unity;

namespace Kaerber.MUD.Server {
    public class UnityConfigurator {
        public static UnityContainer Configure() {
            var container = new UnityContainer();

            var rootPath = ConfigurationManager.AppSettings.Get( "RootPath" );
            var assetsRootPath = ConfigurationManager.AppSettings.Get( "AssetsRootPath" );
            var commandsPath = ConfigurationManager.AppSettings.Get( "CommandsPath" );

            container.RegisterInstance( "RootPath", rootPath );
            container.RegisterInstance( "AssetsRootPath", assetsRootPath );
            container.RegisterInstance( "UsersRootPath", 
                                        ConfigurationManager.AppSettings.Get( "UsersRootPath" ) );
            container.RegisterInstance( "LibPath", 
                                        ConfigurationManager.AppSettings.Get( "LibPath" ) );
            container.RegisterInstance( "MlLibPath", 
                                        ConfigurationManager.AppSettings.Get( "MlLibPath" ) );
            container.RegisterInstance( "CommandsPath", commandsPath );
            container.RegisterInstance( "AffectsPath",
                                        ConfigurationManager.AppSettings.Get( "AffectsPath" ) );

            var characterManager = new CharacterManager( assetsRootPath );
            var itemManager = new ItemManager( assetsRootPath );
            var areaManager = new AreaManager( assetsRootPath, characterManager, itemManager );

            container.RegisterInstance<IManager<ICommand>>( new CommandManager( commandsPath ) );
            container.RegisterInstance<IManager<Character>>( characterManager );
            container.RegisterInstance<IManager<Item>>( itemManager );
            container.RegisterInstance<IManager<Area>>( areaManager );

            container.RegisterInstance( new Clock( DateTime.Now.Ticks ) );

            container.RegisterType<World>( new ContainerControlledLifetimeManager() );
            return container;
        }
    }
}
