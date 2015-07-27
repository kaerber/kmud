from __future__ import division
import random

import math

from Kaerber.MUD.Entities import IAttack

class Attack( IAttack ):
    def __init__( self ):
        self.assaulterWeaponBaseDamage = 0
        self.assaulterStrength = 0
        self.assaulterDexterety = 0
        self.assaulterIntellect = 0

        self.assaulterAttack = 200
        self.assaulterMagicAttack = 200
        self.assaulterAccuracy = 100
        self.assaulterCriticalChance = 0

        self.defenderConstitution = 0
        self.defenderWisdom = 0

        self.defenderArmor = 200
        self.defenderMagicArmor = 200
        self.defenderEvasion = 0

        self.CriticalHit = False


    def SetAssaulterWeaponBaseDamage( self, value ):
        self.assaulterWeaponBaseDamage = value

    def AddAssaulterStrength( self, value ):
        self.assaulterStrength += value

    def AddAssaulterDexterety( self, value ):
        self.assaulterDexterety += value

    def AddAssaulterIntellect( self, value ):
        self.assaulterIntellect += value

    def AddAssaulterAttack( self, value ):
        self.assaulterAttack += value

    def AddAssaulterMagicAttack( self, value ):
        self.assaulterMagicAttack += value

    def AddAssaulterAccuracy( self, value ):
        self.assaulterAccuracy += value

    def AddAssaulterCriticalChance( self, value ):
        self.assaulterCriticalChance += value


    def AddDefenderConstitution( self, value ):
        self.defenderConstitution += value

    def AddDefenderWisdom( self, value ):
        self.defenderWisdom += value

    def AddDefenderArmor( self, value ):
        self.defenderArmor += value

    def AddDefenderMagicArmor( self, value ):
        self.defenderMagicArmor += value

    def AddDefenderEvasion( self, value ):
        self.defenderEvasion += value


    def CalculateHitChance( self ):
        return self.NormalizeChance( self.assaulterAccuracy - self.defenderEvasion )

    def CalculateCriticalChance( self ):
        return self.NormalizeChance( self.assaulterCriticalChance )

    def CalculateDamage( self, baseDamage ):
        return int( baseDamage*self.DamageModifier() )


    def Hit( self ):
        if getattr( self, 'hitResult', None ) == None:
            self.hitResult = self.CheckChance( self.CalculateHitChance() )
        return self.hitResult


    def DamageModifier( self ):
        return( self.assaulterAttack/self.defenderArmor + 
               math.erf( ( self.assaulterStrength - self.defenderConstitution )/15/math.sqrt( 2 ) ) )

    def CriticalDamageModifier( self ):
        return( self.damageModifier() )

    def NormalizeChance( self, chance, lowerBound = 5, higherBound = 95 ):
        return max( lowerBound, min( higherBound, chance ) )

    def CheckChance( self, chance ):
        return random.randint( 1, 100 ) <= chance

    def CheckCritical( self ):
        self.CriticalHit = random.randint( 1, 100 ) <= self.assaulterCriticalChance

    def Damage( self ):
        return self.CalculatePhysicalDamage()

    def CalculatePhysicalDamage( self ):
        WeaponDMG = self.assaulterWeaponBaseDamage
        Atk = self.assaulterAttack
        Def = self.defenderArmor
        str = self.assaulterStrength
        dex = self.assaulterDexterety
        vit = self.defenderConstitution

        if self.CriticalHit:
            return int( round( 
                WeaponDMG*( ( 200 + Atk )/( 200 + Def ) + erf( max( str, dex*2 ) - vit, 7.5 ) ) ) )

        return int( round( 
            WeaponDMG*( ( 200 + Atk )/( 200 + Def ) + erf( str - vit, 15 ) ) ) )

    def CalculateMagicalDamage( self ):
        WeaponMDMG = self.assaulterWeaponBaseDamage
        MAtk = self.assaulterMagicAttack
        MDef = self.defenderMagicArmor
        int = self.assaulterIntellect
        dex = self.assaulterDexterety
        wis = self.defenderWisdom

        if self.CriticalHit:
            return int( round( 
                WeaponDMG*( ( 200 + MAtk )/( 200 + MDef ) + erf( max( int, dex*2 ) - wis, 7.5 ) ) ) )

        return int( round( 
            WeaponDMG*( ( 200 + MAtk )/( 200 + MDef ) + erf( int - wis, 15 ) ) ) )


def erf( value, sigma ):
    return math.erf( value/math.sqrt( 2 )/sigma )
