using System;
using System.Collections.Generic;
using System.Text;

using Kaerber.MUD.Common;
using Kaerber.MUD.Telnet;

namespace Kaerber.MUD.Views {
    public class TelnetConnectionView : IConnectionView {
        public TelnetConnectionView( TelnetConnection connection ) {
            _connection = connection;
            _action = () => { };
        }

        /// <summary>
        /// A blocking sequence of commands received from Telnet connection and pre-parsed.
        /// </summary>
        public IEnumerable<string> Commands() {
            foreach( var line in _connection.ReadLines( Encoding.ASCII ) ) {
                if( _mapping.ContainsKey( "*" ) && line != "" )
                    yield return line;
                else if( _mapping.ContainsKey( line ) )
                    yield return _mapping[line];
                else
                    _action();
            }
        }

        public void Start() {
            ConfigureTerminal();

            ( _action = () =>
                     Write( "Welcome!\n" )
                    .Write( "\n" )
                    .Write( "Please select action:\n" )
                    .Write( "    1. Login user\n" )
                    .Write( "    2. Register new user\n" )
                    .Write( "    3. Exit\n" )
                    .Write( "\n" )
                    .Write( "Input you choice: " ) )();
            _mapping = new Dictionary<string, string> {
                { "1", "login" },
                { "2", "register" },
                { "3", "exit" }
            };
        }

        public void Login() {
            ( _action = () => 
                     Write( "\n" )
                    .Write( "Please enter your username: " ) )();
            _mapping = new Dictionary<string, string> {
                { "*", "login-got-username" }
            };
        }

        public void LoginGotUsername() {
            ( _action = () =>
                     Write( "Password: " ) )();
            _connection.SetOutputVisibility( false );
            _mapping = new Dictionary<string, string> {
                { "*", "login-got-password" }
            };
        }

        public void LoginGotPassword() {
            _connection.SetOutputVisibility( true );
        }

        public void LoginFailed() {
            Write( "Username or password is incorrect.\n" );
            Write( "\n" );
        }


        public void Register() {
            ( _action = () =>
                     Write( "\n" )
                    .Write( "Enter your username: " ) )();
            _mapping = new Dictionary<string, string> {
                { "*", "register-got-username" }
            };
        }

        public void RegisterGotUsername() {
            ( _action = () =>
                     Write( "\n" )
                    .Write( "Enter password: " ) )();
            _mapping = new Dictionary<string, string> {
                { "*", "register-got-password" }
            };
        }

        public void RegisterUsernameExists() {
            ( _action = () =>
                     Write( "Username already exists.\n" )
                    .Write( "\n" ) )();
        }

        public void RegisterGotPassword() {
            ( _action = () =>
                     Write( "Confirm password: " ) )();
            _mapping = new Dictionary<string, string> {
                { "*", "register-got-confirm-password" }
            };
        }

        public void RegisterGotPasswordConfirmed() {
            ( _action = () =>
                     Write( "\n" )
                    .Write( "Enter your email address: " ) )();
            _mapping = new Dictionary<string, string> {
                { "*", "register-got-email" }
            };
        }

        public void RegisterPasswordNotConfirmed() {
            ( _action = () =>
                     Write( "Passwords do not match.\n" ) )();
        }

        public void WelcomeUser( IUser user ) {
            Write( "Welcome, " ).Write( user.Username ).Write( "\n" );
        }

        public void Stop() {}

        public void SetMapping( Dictionary<string, string> mapping ) {
            _mapping = mapping;
        }

        private void ConfigureTerminal() {
            _connection.SetRemote( Option.SuppressLocalEcho, true );
            _connection.Set( Option.Echo, true );
        }

        private TelnetConnectionView Write( string message ) {
            _connection.Write( message );
            return this;
        }

        private readonly TelnetConnection _connection;
        private Dictionary<string, string> _mapping;
        private Action _action;

    }
}
