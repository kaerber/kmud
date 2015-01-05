using System;
using System.Runtime.Serialization;

namespace Kaerber.MUD.Entities {
    [Serializable]
    public class EntityException : Exception
    {
        public EntityException() {}

        public EntityException( string message ) : base( message ) {}

        public EntityException( string message, Exception innerException )
            : base( message, innerException ) {}

        public EntityException( SerializationInfo info, StreamingContext context )
            : base( info, context ) {}

    }
}
