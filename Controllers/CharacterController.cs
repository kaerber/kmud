﻿using System.IO;
using System.Linq;

using Kaerber.MUD.Common;
using Kaerber.MUD.Controllers.Commands;
using Kaerber.MUD.Entities;
using Kaerber.MUD.Views;

namespace Kaerber.MUD.Controllers {
    public class CharacterController : ICharacterController {
        public CharacterController( Character model, 
                                    ICharacterView view, 
                                    IManager<ICommand> commandManager,
                                    IManager<Character> characterManager,
                                    IUser user ) {
            Model = model;
            View = view;
            _user = user;
            _commandManager = commandManager;
            _characterManager = characterManager;

        }

        public Character Model { get; }

        public ICharacterView View { get; }

        public IController Start() {
            View.Start();
            return this;
        }

        public IController Run() {
            return View.Commands()
                .Select( InputReceived )
                .FirstOrDefault( controller => controller != this );
        }

        public IController Stop() {
            View.Stop();
            return null;
        }

        public IController InputReceived( string line ) {
            OnCommand( PlayerInput.Parse( line ) );
            return this;
        }

        public void OnCommand( PlayerInput input ) {
            var cmd = _commandManager.Load( string.Empty, input.Command );

            lock( View ) {
                View.Command = true;
                lock( Model.Sync )
                    cmd.Execute( this, input );
            }
        }

        public void SaveCharacter() {
            _characterManager.Save( Path.Combine( "players", _user.Username ), Model );
        }

        public void Quit() {
            SaveCharacter();

            if( !Model.Can( "quit", new EventArg( "ch", Model ) ) )
                return;

            Model.Has( "has_quit", new EventArg( "ch", Model ) );   // todo fix double has

            Model.Room.RemoveCharacter( Model );    // todo remove direct access to ch.room.characters

            View.Quit();
        }

        private readonly IManager<ICommand> _commandManager;
        private readonly IManager<Character> _characterManager;
        private readonly IUser _user;
    }
}
