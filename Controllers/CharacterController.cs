using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Kaerber.MUD.Common;
using Kaerber.MUD.Controllers.Commands;
using Kaerber.MUD.Entities;
using Kaerber.MUD.Views;

namespace Kaerber.MUD.Controllers {
    public class CharacterController : ICharacterController {
        public delegate void CharacterCommandHandler( Character character, IEnumerable<string> args );

        public CharacterController( Character model, 
                                    ICharacterView view, 
                                    IManager<Command> commandManager ) {
            Model = model;
            View = view;
            _commandManager = commandManager;
        }

        public Character Model { get; private set; }

        public ICharacterView View { get; private set; }

        public CharacterController( Character model, ICharacterView view ) {
            View = view;
            Model = model;
        }

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
            var cmd = _commandManager.Get( input.Command );

            lock( View ) {
                View.Command = true;
                lock( Model.Room )
                    cmd.Execute( this, input );
            }
        }

        public void SaveCharacter()
        {
            File.WriteAllText( World.PlayersRootPath + Model.ShortDescr + ".data", World.Serializer.Serialize( Model ) );
        }

        public void Quit() {
            SaveCharacter();

            if( !Model.Room.Event( "ch_can_quit", EventReturnMethod.And, new EventArg( "ch", Model ) ) )
                return;

            Model.Room.Event( "ch_has_quit", EventReturnMethod.None, new EventArg( "ch", Model ) );

            Model.Room.RemoveCharacter( Model );

            View.Quit();
        }

        private readonly IManager<Command> _commandManager;
    }
}
