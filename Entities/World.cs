using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Script.Serialization;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;

using Kaerber.MUD.Common;
using Kaerber.MUD.Entities.Aspects;

namespace Kaerber.MUD.Entities {
    public class World : Area {
        public static readonly string RootPath;
        public static readonly string AssetsRootPath;
        public static readonly string UsersRootPath;
        public static readonly string LibPath;
        public static readonly string MlLibPath;
        public static readonly string CommandsPath;
        public static readonly string AffectsPath;

        public static World Instance;
        public static JavaScriptSerializer Serializer;

        public static Random RndGen = new Random();

        static World() {
            RootPath = ConfigurationManager.AppSettings.Get( "RootPath" );
            AssetsRootPath = ConfigurationManager.AppSettings.Get( "AssetsRootPath" );
            UsersRootPath = ConfigurationManager.AppSettings.Get( "UsersRootPath" );
            LibPath = ConfigurationManager.AppSettings.Get( "LibPath" );
            MlLibPath = ConfigurationManager.AppSettings.Get( "MlLibPath" );
            CommandsPath = ConfigurationManager.AppSettings.Get( "CommandsPath" );
            AffectsPath = ConfigurationManager.AppSettings.Get( "AffectsPath" ); 
        }

        public long Time { get { return _clock.Time; } }

        public List<Area> Areas;
        public new Dictionary<string, Room> Rooms;
        public new Dictionary<string, Character> Mobs;
        public new Dictionary<string, Item> Items;
        public Dictionary<string, SkillData> Skills;
        public new Dictionary<string, AffectInfo> Affects;
        public new Dictionary<string, AspectInfo> Aspects;

        public World() {
            Areas = new List<Area>();
            Rooms = new Dictionary<string, Room>();
            Mobs = new Dictionary<string, Character>();
            Items = new Dictionary<string, Item>();
            Skills = new Dictionary<string, SkillData>();
            Affects = new Dictionary<string, AffectInfo>();

            UpdateQueue = new TimedEventQueue( null );
        }

        public void Initialize( IUnityContainer container ) {
            _container = container;
            _clock = container.Resolve<Clock>();
            _clock.Update += UpdateQueue.Run;
            _clock.Round += () => Rooms.Values.ForEach( room => room.Round() );
            _clock.Hour += () => Rooms.Values.ForEach( room => room.Tick() );
        }

        public override ISerialized Deserialize( IDictionary<string, object> data ) {
            Areas = ConvertToType<List<string>>( data["Areas"] )
                .ConvertAll( vnum => new Area { Id = vnum } );
            
            Skills = ConvertToTypeEx( data, "Skills", new Dictionary<string, SkillData>() );
            Affects = ConvertToTypeEx( data, "AffectData", new Dictionary<string, AffectInfo>() );
            Aspects = ConvertToTypeEx( data, "AspectInfo", new Dictionary<string, AspectInfo>() );

            return base.Deserialize( data );
        }

        public override IDictionary<string, object> Serialize() {
            return base.Serialize()
                .AddEx( "Areas", Areas.ConvertAll( area => area.Id ) )
                .AddEx( "Skills", Skills )
                .AddEx( "AffectData", Affects ) 
                .AddIf( "AspectInfo", Aspects, Aspects != null && Aspects.Count > 0 );
        }

        public virtual void Pulse( long ticks ) {
            _clock.Pulse( ticks );
        }

        public void LoadAreas() {
            Areas = Areas.ConvertAll( area => Load( area.Id ) );
            Areas.ForEach( area => area.Initialize() );
        }


        public override Room GetRoom( string id ) {
            if( id == null )
                return null;

            return Rooms.ContainsKey( id ) ? Rooms[id] : null;
        }

        private IUnityContainer _container;
        private Clock _clock;

        #region Serialization
        // TODO: this responsibility does not belong to World
        public static T ConvertToType<T>( object data ) {
            return ( Serializer.ConvertToType<T>( data ) );
        }

        public static T ConvertToTypeEx<T>( IDictionary<string, object> dict,
            string key ) where T : class {
            return ( dict.ContainsKey( key )
                ? ConvertToType<T>( dict[key] )
                : default( T ) );
        }

        public static T ConvertToTypeEx<T>( IDictionary<string, object> dict,
            string key,
            T defaultValue ) where T : class {
            return ( ConvertToTypeEx<T>( dict, key ) ?? defaultValue );
        }

        public static T ConvertToTypeExs<T>( IDictionary<string, object> dict,
            string key ) where T : struct {
            return ( dict.ContainsKey( key )
                ? ConvertToType<T>( dict[key] )
                : default( T ) );
        }
        #endregion
    }
}
