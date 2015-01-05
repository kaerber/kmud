using Kaerber.MUD.Entities;
using Kaerber.MUD.Views;

namespace Kaerber.MUD.Controllers {
    public interface ICharacterController : IController {
        Character Model { get; }
        ICharacterView View { get; }

        void Quit();
    }
}
