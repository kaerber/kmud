using System;
using System.Collections.Generic;

using Kaerber.MUD.Common;

namespace Kaerber.MUD.Entities {
    public class Skill : ISerialized {
        public string Name { get { return ( _data.Name ); } }
        public int Value { get; set; }

        public Skill() {}

        public Skill( SkillData data ) {
            _data = data;
        }


        public void Improve() {
            Value++;
            _pointsToImprove = Value*_data.Rating;
            if( SkillImproved != null )
                SkillImproved( this, Value );
        }

        public void CheckImprove() {
            _pointsToImprove--;
            if( _pointsToImprove <= 0 )
                Improve();
        }

        public ISerialized Deserialize( IDictionary<string, object> data ) {
            _data = World.Instance.Skills[ World.ConvertToType<string>( data["Name"] ) ];
            Value = World.ConvertToType<int>( data["Value"] );
            _pointsToImprove = World.ConvertToType<int>( data["PointsToImprove"] );

            return( this );
        }

        public IDictionary<string, object> Serialize() {
            return ( new Dictionary<string, object>()
                .AddEx( "Name", _data.Name )
                .AddEx( "Value", Value )
                .AddEx( "PointsToImprove", _pointsToImprove ) );
        }

        public event Action<Skill, int> SkillImproved;

        private SkillData _data;
        private int _pointsToImprove;
    }
}
