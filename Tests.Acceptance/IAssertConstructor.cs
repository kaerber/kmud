using System;

namespace Kaerber.MUD.Tests.Acceptance {
    public interface IAssertConstructor<T> {
        Action<T> ConstructAssert();
    }
}
