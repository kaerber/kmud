namespace Kaerber.MUD.Entities {
    public interface IAttack {
        void SetAssaulterWeaponBaseDamage( int baseDamage );

        void AddAssaulterStrength( int strength );
        void AddAssaulterIntellect( int intellect );
        void AddAssaulterDexterety( int dexterety );

        void AddAssaulterAttack( int attack );
        void AddAssaulterMagicAttack( int magicAttack );
        void AddAssaulterAccuracy( int accuracy );
        void AddAssaulterCriticalChance( int criticalChance );

        void AddDefenderArmor( int armor );
        void AddDefenderMagicArmor( int marmor );
        void AddDefenderEvasion( int evasion );

        void AddDefenderConstitution( int vitality );
        void AddDefenderWisdom( int wisdom );

        void CheckCritical();
    }
}
