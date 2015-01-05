namespace Kaerber.MUD.Entities
{
    public interface IEventHandler
    {
        void ReceiveEvent( Event e );
        string Id { get; }
    }
}
