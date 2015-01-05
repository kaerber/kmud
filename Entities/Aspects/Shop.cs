using System;
using System.Collections.Generic;
using System.Diagnostics;

using Kaerber.MUD.Common;

namespace Kaerber.MUD.Entities.Aspects
{
    [MudComplexType]
    public class Shop : ISerialized
    {
        [MudEdit( "Goods in shop" )]
        public List<ObjectReset> Goods { get; set; }

        public Shop()
        {
            Debug.Assert( World.Instance.Aspects.ContainsKey( "shop" ) );
            //Info = World.Instance.Aspects["shop"];
        }

        #region ISerialized members
        public ISerialized Deserialize( IDictionary<string, object> data )
        {
            Goods = World.ConvertToTypeEx<List<ObjectReset>>( data, "Goods", null );
            return ( this );
        }

        public IDictionary<string, object> Serialize()
        {
            return (
                new Dictionary<string, object>()
                .AddIf( "Goods", Goods, Goods != null && Goods.Count > 0 )
            );
        }
        #endregion

        public void Update()
        {
            //if( Goods != null )
                //Goods.ForEach( g => g.Update( ( Room )Host, item => item.Affects.Cast( "shop_goods", -1 ) ) );
        }

        public void ReceiveEvent( Event e, Func<List<EventArg>> getArgs = null )
        {
            //Debug.Assert( Info != null );
            //base.ReceiveEvent( e, () => getArgs.NotNull()().AddEx( new EventArg( "shop", Host ) ) );
        }
    }
}
