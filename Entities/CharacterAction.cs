namespace Kaerber.MUD.Entities
{
    public abstract class CharacterAction
    {
        public virtual Character Character { get; private set; }

        public abstract int SharedCooldown { get; }

        public void Setup( Character character )
        {
            Character = character;
        }

        public abstract void Execute();
    }
}
