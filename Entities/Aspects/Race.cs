using System.Diagnostics.Contracts;

namespace Kaerber.MUD.Entities.Aspects
{
    public class Race : IEventHandler
    {
        public string Id { get; set; }
        public dynamic Stats { get; set; }

        public Race()
        {
            object s = Stats;
            Contract.Ensures( s != null );

            Stats = AspectFactory.Stats();
        }

        public virtual void ReceiveEvent( Event e )
        {
            Stats.ReceiveEvent( e );
        }
    }
}
