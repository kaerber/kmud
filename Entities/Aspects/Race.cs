namespace Kaerber.MUD.Entities.Aspects {
    public class Race : EventTarget {
        public string Id { get; set; }
        public dynamic Stats { get; set; }

        public Race() {
            Stats = AspectFactory.Stats();
        }

        public override void ReceiveEvent( Event e ) {
            Stats.ReceiveEvent( e );
        }
    }
}
