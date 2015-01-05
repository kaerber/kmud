namespace Kaerber.MUD.Controllers {
    public interface IController {
        IController Start();
        IController Run();
        IController Stop();
    }
}
