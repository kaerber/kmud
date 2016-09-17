namespace Kaerber.MUD.Entities {
    public interface IEventTarget {
        void ReceiveEvent( Event e );
        string Level { get; }
    }
}
