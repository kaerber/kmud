using Kaerber.MUD.Entities;
using Kaerber.MUD.Entities.Aspects;

namespace Kaerber.MUD.Views {
    public interface ICharacterView : IView {
        void Write( string message );
        void WriteFormat( string message, params object[] args );

        bool Command { get; set; }

        void ReceiveEvent( Event e );
        void Quit();

        void RenderRoom( Room room );

        void RenderCharacter( Character character );
        void RenderInventory( Character character );
        void RenderEquipment( Equipment equipment );

        void RenderItem( Item item );
    }
}
