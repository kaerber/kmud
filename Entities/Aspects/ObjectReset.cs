using System;
using System.Collections.Generic;
using System.Linq;

using Kaerber.MUD.Common;

namespace Kaerber.MUD.Entities.Aspects
{
    [MudComplexType]
    public class ObjectReset : ISerialized
    {
        [MudEdit( "Vnum" )]
        public string Vnum { get; set; }

        #region ISerializable members
        public IDictionary<string, object> Serialize()
        {
            return(
                new Dictionary<string, object>()
                .AddEx( "Vnum", Vnum )
            );
        }

        public ISerialized Deserialize( IDictionary<string, object> data )
        {
            Vnum = World.ConvertToTypeEx<string>( data, "Vnum" );
            return ( this );
        }
        #endregion


        public void Update( Room host, Action<Item> process = null )
        {
            if( !World.Instance.Items.ContainsKey( Vnum ) )
                return;

            if( host.Items.Count( obj => obj.Id == Vnum ) >= 1 )
                return;
            var item = host.Items.Load( Vnum );
            if( process != null )
                process( item );
        }
    }
}
