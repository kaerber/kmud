using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

using Kaerber.MUD.Common;

namespace Kaerber.MUD.Entities.Utilities
{
    public class Converter<T> : JavaScriptConverter where T : class, ISerialized, new() {
        private readonly Type[] _supportedTypes = { typeof( T ) };
        public override IEnumerable<Type> SupportedTypes { get { return _supportedTypes; } }

        public override object Deserialize( IDictionary<string, object> dict,
            Type type,
            JavaScriptSerializer serializer )
        {
            return type == typeof( T ) ? ( new T() ).Deserialize( dict ) : null;
        }

        public override IDictionary<string, object> Serialize(
            object entity,
            JavaScriptSerializer serializer )
        {
            var typedEntity = entity as T;
            return typedEntity != null ? typedEntity.Serialize() : new Dictionary<string, object>();
        }
    }
}
