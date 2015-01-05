using Kaerber.MUD.Entities.Aspects;

using log4net;

namespace Kaerber.MUD.Entities.Actions
{
    public class AutoAttackAction : CharacterAction
    {
        private static readonly ILog _logger = LogManager.GetLogger( typeof( AutoAttackAction ) );

        public override int SharedCooldown
        {
            get { return World.TimeRound; }
        }

        public override void Execute()
        {
            if( Character.Target == null )
                return;
            Character.MakeAttack( AspectFactory.Attack() );
            _logger.Debug( string.Format( "\nautoattack: {0} attacks {1}: executed",
                Character.ShortDescr, Character.Target.ShortDescr ) );
        }
    }
}
