using System;
using System.Collections.Generic;

using Kaerber.MUD.Common;

namespace Kaerber.MUD.Entities.Aspects
{
    [MudComplexType]
    public class AspectInfo : ISerialized
    {
        [MudEdit( "Aspect name" )]
        public string Name { get; set; }

        [MudEdit( "Common handlers for all isntances of Aspect" )]
        public HandlerSet Handlers { get; set; }


        #region ISerialized members
        public ISerialized Deserialize( IDictionary<string, object> data )
        {
            Name = World.ConvertToType<string>( data["Name"] );
            Handlers = World.ConvertToTypeEx( data, "Handlers", new HandlerSet() );

            return ( this );
        }

        public IDictionary<string, object> Serialize()
        {
            return (
                new Dictionary<string, object>()
                .AddEx( "Name", Name )
                .AddIf( "Handlers", Handlers, Handlers != null && Handlers.Count > 0 )
            );
        }
        #endregion

        public void ReceiveEvent( Event e, Func<IList<EventArg>> getArgs = null )
        {
            Handlers.Execute( e, getArgs );
        }
    }
}
