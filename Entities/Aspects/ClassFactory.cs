namespace Kaerber.MUD.Entities.Aspects {
    public class ClassFactory {
        private static readonly Class _warrior;

        static ClassFactory() {
            var stats = AspectFactory.Stats();
            stats.Strength = 20;
            stats.Dexterety = 20;
            stats.Constitution = 20;
            stats.Intellect = 20;
            stats.Wisdom = 20;

            _warrior = new Class( "warrior", stats );
        }

        public static dynamic Warrior => _warrior;
    }
}
