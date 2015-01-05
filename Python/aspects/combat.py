import random

import attack

from aspect import Aspect

from Kaerber.MUD.Entities import *


# See aspect.py for details
def construct():
    return CombatAspect()


class CombatAspect( Aspect ):
    def __init__( self ):
        self._target = None

    def Clone( self ):
        return CombatAspect()


    def UseAbility( self, ability ):
        self.SetTarget( 
            ability.SelectMainTarget( self.Fighting ) )
        ability.Activate()


    def SetTarget( self, target ):
        self.Fighting = target
        if target == None: return

        # event triggers auto-attack ability
        self.Host.Did( 'targeted_ch1', { 'ch1': target } )

    
    def MakeAttack( self, attack ):
        if not attack.Hit():
            self.Host.Did( 'missed_ch1', { 'ch1' : self.Fighting } )
            return
        
        self.Host.Did( 'hit_ch1', { 'ch1' : self.Fighting } )

        damage = attack.Damage()
        self.Host.Did( 'dealt_damage_to_ch1', { 'damage': damage, 'ch1': self.Fighting } )

        
    @property
    def Fighting( self ):
        return self._target

    @Fighting.setter
    def Fighting( self, value ):
        if self._target != None and value == None:
            self.Host.Did( 'stopped_target_ch1', { 'ch1': self._target } )
        self._target = value
    
    @property
    def Target( self ):
        return self._target

    # Save/Load
    def Saved( self ):
        return False

    def Serialize( self ):
        return {}

    def Deserialize( self, data ):
        return self

    def InitiateAttack( self ):
        return attack.Attack()

    # event handlers
    def ch_hit_this( self, e ):
        if self.Fighting == None:
            self.Host.Kill( e['ch'] )
            
    def ch_missed_this( self, e ):
        if self.Fighting == None:
            self.Host.Kill( e['ch'] )
            
    def ch_died( self, e ):
        if self.Fighting == e['ch']:
            self.Fighting = None
    
    def this_died( self, e ):
        self.Fighting = None
