namespace Kaerber.MUD.Entities.Abilities
{
    public interface ICombatAbility
    {
        Character SelectMainTarget( Character currentTarget );
        void Activate();
    }
}
