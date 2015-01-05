namespace Kaerber.MUD.Entities.Aspects
{
    public class RaceFactory
    {
        static RaceFactory()
        {
            Default = new Race();
            Default.Stats.Strength = 20;
            Default.Stats.Dexterety = 20;
            Default.Stats.Constitution = 20;
            Default.Stats.Intellect = 20;
            Default.Stats.Wisdom = 20;
        }


        public static Race Default { get; private set; }
    }
}
