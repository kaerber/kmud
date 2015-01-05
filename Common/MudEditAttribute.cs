using System;

namespace Kaerber.MUD.Common {
    [AttributeUsage( AttributeTargets.Property )]
    public class MudEditAttribute : Attribute {
        public MudEditAttribute( string description ) {
            Description = description;
        }

        public MudEditAttribute( string description, string customType ) : this( description ) {
            CustomType = customType;
        }

        public MudEditAttribute( string description, string customType, string customTypeKey ) 
            : this( description, customType ) {
            CustomTypeKey = customTypeKey;
        }

        public string Description;
        public string CustomType;
        public string CustomTypeKey;
    }
}
