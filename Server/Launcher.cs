using System;
using System.Reflection;
using System.Threading;
using System.Web.Script.Serialization;

using Kaerber.MUD.Controllers;
using Kaerber.MUD.Entities;
using Kaerber.MUD.Entities.Aspects;
using Kaerber.MUD.Entities.Utilities;

namespace Kaerber.MUD.Server {
    public class Launcher {
        static void Main() {
            log4net.Config.XmlConfigurator.Configure();

            InitializeML();
            World.Serializer = InitializeSerializer();

            var server = new Server();
            server.Initialize();
            
            while( true ) {
                server.Pulse( DateTime.Now.Ticks );
                Thread.Sleep( Clock.TimeStep );
            }
        }

        public static void InitializeML() {
            MLFunction.LoadAssemblies( Assembly.GetAssembly( typeof( CharacterController ) ) );
        }

        public static JavaScriptSerializer InitializeSerializer() {
            var serializer = new JavaScriptSerializer();
            serializer.RegisterConverters( new JavaScriptConverter[] { 
                    new Converter<World>(),
                    new Converter<User>(),
                    new Converter<Character>(),
                    
                    new Converter<Area>(),
                    new Converter<Room>(),
                    new Converter<Exit>(),
                    
                    new Converter<Item>(),

                    new Converter<SkillSet>(),
                    new Converter<Skill>(),
                    
                    new Converter<AffectInfo>(),
                    new Converter<Affect>(),
                    
                    new Converter<HandlerSet>(),
                    new Converter<MLFunction>(),

                    new Converter<AspectInfo>(),

                    new Converter<RoomReset>(),
                    new Converter<ObjectReset>(),
                    new Converter<Equipment>(),
                    new Converter<Shop>(),
                    new Converter<Stack>(),
                } );

            return serializer;
        }

    }
}
