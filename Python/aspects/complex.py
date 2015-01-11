import sys

from aspect import Aspect

from Kaerber.MUD.Entities import Room, Item, Character
from Kaerber.MUD.Entities.Aspects import IAspect, AspectFactory

import System

# See aspect.py for details
def construct():
    return ComplexAspect()


class ComplexAspect( Aspect ):
    def isAspect( self, aspect ):
        return isinstance( aspect, IAspect )


    def __init__( self ):
        self.Host = None

    def Clone( self ):
        clone = ComplexAspect()
        for aspect in self.Aspects():
            clone[aspect] = self[aspect].Clone()

        return clone


    def __getitem__( self, key ):
        return getattr( self, key, None )

    def __setitem__( self, key, value ):
        setattr( self, key, value )

    def __setattr__( self, key, value ):
        if( self.isAspect( value ) ):
            value.Host = self.Host
        Aspect.__setattr__( self, key, value )


    @Aspect.Host.setter
    def Host( self, value ):
        Aspect.Host.__set__( self, value )

        for aspect in self.Aspects():
            self[aspect].Host = value


    def Serialize( self ):
        return dict( [ ( key, self[key].Serialize() ) 
            for key in filter( lambda k: self[k].Saved(), self.Aspects() ) ] )


    def Deserialize( self, data ):
        for item in data:
            try:
                self[item.Key] = self.Construct( item.Key ).Deserialize( item.Value ) 
            except ImportError:
                self.log.Debug( "Aspect " + item.Key + " not found." )

        return self


    def ReceiveEvent( self, event ):
        for aspect in self.Aspects():
            self[aspect].ReceiveEvent( event )

    # C# interface
    def Add( self, key, value ):
        self[key] = value

    def Remove( self, key ):
        delattr( self, key )

    # editor support
    def DefaultValue( self ):
        return None

    def GetMember( self, key ):
        return self[key]

    def SetMember( self, key, value ):
        raise System.InvalidOperationException( "Can't set member Aspect, create it with '+' command instead." )
        

    def SetValue( self, value, view ):
        raise System.NotSupportedException( "Complex objects do not support setting directly." )

    def AddMember( self, key, view ):
        if key in self.Editables() and self[key] == None:
            self[key] = self.Construct( key )
            view.Write( "Aspect " + key + " is created.\n" )


    def RemoveMember( self, key, view ):
        if self[key] != None:
            delattr( self, key )
            view.Write( "Aspect " + key + " removed.\n" )


    def Editables( self ):
        if isinstance( self.Host, Character ):
            return []
        if isinstance( self.Host, Room ):
            return [ 'shop' ]
        if isinstance( self.Host, Item ):
            return [ 'weapon' ]

        return []


    def Members( self ):
        return [key for key in self.Editables() if self[key] != None]

    def Aspects( self ):
        return filter( lambda key: self.isAspect( self[key] ), self.__dict__ )

    def Construct( self, key ):
        name = "aspects." + key
        __import__( name )
        aspect = sys.modules[name].construct()
        return aspect
