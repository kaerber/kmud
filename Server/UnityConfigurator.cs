using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
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
            var usersRootPath = ConfigurationManager.AppSettings.Get( "UsersRootPath" );
            var assetsRootPath = ConfigurationManager.AppSettings.Get( "AssetsRootPath" );
            var commandsPath = ConfigurationManager.AppSettings.Get( "CommandsPath" );
            var libPath = ConfigurationManager.AppSettings.Get( "LibPath" );
            var mlLibPath = ConfigurationManager.AppSettings.Get( "MlLibPath" );
            var abilitiesPath = Path.Combine( mlLibPath, "abilities" );
            var affectsPath = ConfigurationManager.AppSettings.Get( "AffectsPath" );

            container.RegisterInstance( "RootPath", rootPath );
            container.RegisterInstance( "AssetsRootPath", assetsRootPath );
            container.RegisterInstance( "UsersRootPath", usersRootPath );
            container.RegisterInstance( "LibPath", libPath );
            container.RegisterInstance( "MlLibPath", mlLibPath );
            container.RegisterInstance( "CommandsPath", commandsPath );
            container.RegisterInstance( "AffectsPath", affectsPath );

            MLFunction.AddSearchPaths( libPath, mlLibPath );
            MLFunction.LoadAssemblies(
                Assembly.GetAssembly( typeof( Character ) ),
                Assembly.GetAssembly( typeof( IEnumerable<string> ) ),
                Assembly.GetAssembly( typeof( Enumerable ) ) );

            var commandManager = new CommandManager( commandsPath );
            var pythonManager = new PythonManager();
            var abilityManager = new AbilityManager( pythonManager, abilitiesPath );
            var characterManager = new CharacterManager( assetsRootPath, abilityManager );

            var itemManager = new ItemManager( assetsRootPath );
            var areaManager = new AreaManager( assetsRootPath, characterManager, itemManager );

            container.RegisterInstance<IManager<ICommand>>( commandManager );
            container.RegisterInstance<IManager<Character>>( characterManager );
            container.RegisterInstance<IManager<Item>>( itemManager );
            container.RegisterInstance<IManager<Area>>( areaManager );

            container.RegisterInstance( new Clock( DateTime.Now.Ticks ) );

            container.RegisterType<World>( new ContainerControlledLifetimeManager() );
            return container;
        }
    }
}
