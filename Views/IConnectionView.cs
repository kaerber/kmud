
using Kaerber.MUD.Common;

namespace Kaerber.MUD.Views {
    public interface IConnectionView : IView {
        void Login();
        void LoginGotUsername();
        void LoginGotPassword();
        void LoginFailed();

        void Register();
        void RegisterGotUsername();
        void RegisterUsernameExists();
        void RegisterGotPassword();
        void RegisterGotPasswordConfirmed();
        void RegisterPasswordNotConfirmed();
        void WelcomeUser( IUser user );
    }
}
