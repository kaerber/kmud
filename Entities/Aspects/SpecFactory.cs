namespace Kaerber.MUD.Entities.Aspects
{
    public class SpecFactory
    {
        private static readonly Spec _warrior;

        static SpecFactory()
        {
            _warrior = new Spec( "warrior" );
        }

        public static dynamic Warrior
        {
            get { return _warrior; }
        }
    }
}
