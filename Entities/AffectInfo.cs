using System;
using System.Collections.Generic;

using Kaerber.MUD.Common;

namespace Kaerber.MUD.Entities
{
    public enum AffectTarget
    {
        Room,
        Character,
        Item
    }

    [Flags]
    public enum AffectFlags
    {
        Multiple = 1,
        Hidden = 2,
        NoDeath = 4,
        NoSave = 8,
        System = 256
    }

    [MudComplexType]
    public class AffectInfo : ISerialized
    {
        [MudEdit( "Affect name" )]
        public string Name { get; set; }

        [MudEdit( "To what this affect can be applied - character, room or item" )]
        public AffectTarget Target { get; set; }

        [MudEdit( "Flags" )]
        public AffectFlags Flags { get; set; }

        [MudEdit( "Handlers" )]
        public HandlerSet Handlers { get; set; }

        #region Load & save
        public ISerialized Deserialize( IDictionary<string, object> data )
        {
            Name = World.ConvertToType<string>( data["Name"] );
            Target = World.ConvertToType<AffectTarget>( data["Target"] );
            Flags = World.ConvertToTypeExs<AffectFlags>( data, "Flags" );
            Handlers = World.ConvertToTypeEx( data, "Handlers", new HandlerSet() );

            return ( this );
        }

        public IDictionary<string, object> Serialize()
        {
            return (
                new Dictionary<string, object>()
                .AddEx( "Name", Name )
                .AddEx( "Target", Target )
                .AddEx( "Flags", Flags )
                .AddIf( "Handlers", Handlers, Handlers != null && Handlers.Count > 0 )
            );
        }
        #endregion

        public static AffectInfo Get( string name )
        {
            if( World.Instance.Affects.ContainsKey( name ) )
                return ( World.Instance.Affects[name] );

            throw new EntityException(
                string.Format( "No affect_data '{0}' exists.", name )
            );
        }
    }
}
