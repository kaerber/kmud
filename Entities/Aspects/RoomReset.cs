using System.Collections.Generic;

using Kaerber.MUD.Common;

namespace Kaerber.MUD.Entities.Aspects
{
    [MudComplexType]
    public class RoomReset : ISerialized
    {
        public Room Host { get; set; }

        [MudEdit( "MobResets" )]
        public List<MobReset> MobResets { get; set; }
        [MudEdit( "ObjectResets" )]
        public List<ObjectReset> ObjectResets { get; set; }

        #region ISerialized members
        public ISerialized Deserialize( IDictionary<string, object> data )
        {
            MobResets = World.ConvertToTypeEx<List<MobReset>>( data, "MobResets", null );
            ObjectResets = World.ConvertToTypeEx<List<ObjectReset>>( data, "ObjectResets", null );

            return ( this );
        }

        public IDictionary<string, object> Serialize()
        {
            return (
                new Dictionary<string, object>()
                .AddIf( "MobResets", MobResets, MobResets != null && MobResets.Count > 0 )
                .AddIf( "ObjectResets", ObjectResets, ObjectResets != null && ObjectResets.Count > 0 )
            );
        }
        #endregion

        public void Update()
        {
            if( MobResets != null )
                MobResets.ForEach( mr => mr.Update( Host ) );

            if( ObjectResets != null )
                ObjectResets.ForEach( or => or.Update( Host ) );
        }
    }
}
