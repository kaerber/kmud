import System

from aspect import Aspect

def construct():
    return StatsAspect()

class StatsAspect( Aspect ):
    def __init__( self ):
        self.Health = 0
        self.Mana = 0

        self.Attack = 0
        self.Armor = 0

        self.MagicAttack = 0
        self.MagicArmor = 0

        self.Accuracy = 0
        self.Evasion = 0
        self.CriticalHitChance = 0

        self.Strength = 0
        self.Dexterety = 0
        self.Constitution = 0
        self.Intellect = 0
        self.Wisdom = 0


    def Clone( self ):
        clone = StatsAspect()

        clone.Health = self.Health
        clone.Mana = self.Mana

        clone.Attack = self.Attack
        clone.Armor = self.Armor

        clone.MagicAttack = self.MagicAttack
        clone.MagicArmor = self.MagicArmor

        clone.Accuracy = self.Accuracy
        clone.Evasion = self.Evasion
        clone.CriticalHitChance = self.CriticalHitChance

        clone.Strength = self.Strength
        clone.Dexterety = self.Dexterety
        clone.Constitution = self.Constitution
        clone.Intellect = self.Intellect
        clone.Wisdom = self.Wisdom

        return clone

    def Serialize( self ):
        return { "hp" : self.Health, "mana" : self.Mana,
            "attack" : self.Attack, "armor" : self.Armor,
            "mattack" : self.MagicAttack, "marmor" : self.MagicArmor,
            "accuracy" : self.Accuracy, "evasion" : self.Evasion, "critchance" : self.CriticalHitChance,
            "strength" : self.Strength, "dexterety" : self.Dexterety, "constitution" : self.Constitution,
            "intellect" : self.Intellect, "wisdom" : self.Wisdom }


    def Deserialize( self, data ):
        data = dict( data )
        self.Health = data.get( "hp", 0 )
        self.Mana = data.get( "mana", 0 )
        self.Attack = data.get( "attack", 0 )
        self.Armor = data.get( "armor", 0 )
        self.MagicAttack = data.get( "mattack", 0 )
        self.MagicArmor = data.get( "marmor", 0 )
        self.Accuracy = data.get( "accuracy", 0 )
        self.Evasion = data.get( "evasion", 0 )
        self.CriticalHitChance = data.get( "critchance", 0 )

        self.Strength = data.get( "strength", 0 )
        self.Dexterety = data.get( "dexterety", 0 )
        self.Constitution = data.get( "constitution", 0 )
        self.Intellect = data.get( "intellect", 0 )
        self.Wisdom = data.get( "wisdom", 0 )

        return self


    # events
    def ch_attacks_this( self, event ):
        attack = event['attack']
        attack.AddDefenderConstitution( self.Constitution )
        attack.AddDefenderWisdom( self.Wisdom )

        attack.AddDefenderArmor( self.Armor )
        attack.AddDefenderMagicArmor( self.MagicArmor )
        attack.AddDefenderEvasion( self.Evasion + self.evasionDexteretyBonus() )


    def this_attacks_ch1( self, event ):
        attack = event['attack']
        attack.AddAssaulterStrength( self.Strength )
        attack.AddAssaulterIntellect( self.Intellect )
        attack.AddAssaulterDexterety( self.Dexterety )

        attack.AddAssaulterAttack( self.Attack )
        attack.AddAssaulterMagicAttack( self.MagicAttack )
        attack.AddAssaulterAccuracy( self.Accuracy + self.accuracyDexteretyBonus() )
        attack.AddAssaulterCriticalChance( self.CriticalHitChance + self.criticalHitChanceDexteretyBonus() )


    def query_max_health( self, event ):
        event.ReturnValue = self.Health + self.healthConstitutionBonus()

    def query_max_mana( self, event ):
        event.ReturnValue = self.Mana + self.manaIntellectBonus()

    def query_stats( self, event ):
        event['stats'].Str = self.Strength
        event['stats'].Dex = self.Dexterety
        event['stats'].Con = self.Constitution
        event['stats'].Int = self.Intellect
        event['stats'].Wis = self.Wisdom

    #helpers
    def criticalHitChanceDexteretyBonus( self ):
        return self.Dexterety/2

    def accuracyDexteretyBonus( self ):
        return self.Dexterety/2

    def evasionDexteretyBonus( self ):
        return self.Dexterety

    def healthConstitutionBonus( self ):
        return self.Constitution*25

    def manaIntellectBonus( self ):
        return self.Intellect*25


    #editor support
    def DefaultValue( self ):
        return None

    def GetMember( self, key ):
        if key in self.Members():
            return getattr( self, key )

    def SetMember( self, key, value ):
        if key in self.Members():
            return setattr( self, key, value )
        

    def SetValue( self, value, view ):
        raise System.NotSupportedException( "Complex objects do not support setting directly." )

    def AddMember( self, key, view ):
        raise System.NotSupportedException( "No members to add found." )

    def RemoveMember( self, key, view ):
        raise System.NotSupportedException( "No removable member found." )


    def Editables( self ):
        return self.Members()


    def Members( self ):
        return [ 'Health', 'Mana',
            'Attack', 'Armor',
            'MagicAttack', 'MagicArmor',
            'Accuracy', 'Evasion', 'CriticalHitChance',
            'Strength', 'Dexterety', 'Constitution', 'Intellect', 'Wisdom' ]
