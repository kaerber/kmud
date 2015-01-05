namespace Kaerber.MUD.Controllers {
    public interface IConnectionController : IController {
        IController Login();
        IController LoginGotUsername( string username );
        IController LoginGotPassword( string password );

        IController Register();
        IController RegisterGotUsername( string username );
        IController RegisterGotPassword( string password );
        IController RegisterGotPasswordConfirmed( string password );
        IController RegisterGotEmail( string email );
    }
}
