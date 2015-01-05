namespace Kaerber.MUD.Controllers {
    public interface IUserController : IController {
        IController GotCharacterName( string name );
    }
}
