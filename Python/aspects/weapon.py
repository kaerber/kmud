import random

from aspect import Aspect

import System
from Kaerber.MUD.Entities import *


# See aspect.py for details
def construct():
    return WeaponAspect()


class WeaponAspect( Aspect ):
    def __init__( self ):
        self.BaseDamage = 0
    
    def Clone( self ):
        clone = WeaponAspect()
        clone.BaseDamage = self.BaseDamage
        return clone

    # Save/Load
    def Serialize( self ):
        return { 
            "BaseDamage" : self.BaseDamage }

    def Deserialize( self, data ):
        if "BaseDamage" in data:
            self.BaseDamage = data["BaseDamage"]
        elif "Average" in data:
            self.BaseDamage = data["Average"]

        return self

    # events
    def this_attacks_ch1( self, event ):
        event["attack"].SetAssaulterWeaponBaseDamage( self.BaseDamage )

    # editor support
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
        return ['BaseDamage']

