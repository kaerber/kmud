using System;
using System.Collections.Generic;

using Kaerber.MUD.Entities;
using Kaerber.MUD.Views;

namespace Kaerber.MUD.Tests.Acceptance.Views {
    public class CommandAssertConstructor : IAssertConstructor<IPlayerCommand> {
        public CommandAssertConstructor( string commandName ) {
            CommandName = commandName;
        }

        public string CommandName { get; }
        public IList<Item> Parameter { get; private set; }

        public IAssertConstructor<IPlayerCommand> WithParameter( IList<Item> items ) {
            Parameter = items;
            return this;
        }

        public Action<IPlayerCommand> ConstructAssert() {
            return null;
        }
    }
}
