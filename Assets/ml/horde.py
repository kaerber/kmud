import random

def GetHost( room ):
    if( not room.Affects.Contains( 'horde_room' ) ):
        return( None )
    if( not room.Affects['horde_room'].Data.ContainsKey( 'host' ) ):
        return( None )
    return( room.Affects['horde_room'].Data['host'] )

class Host( object ):
    def __init__( self, rooms, stages ):
        self.Rooms = rooms
        self.Stages = stages
        self.Mobs = []
        self.Players = []


    def Start( self ):
        self.StageIndex = 0
        self.Stage = self.Stages[self.StageIndex]
        self.Mobs = []

        for room in self.Rooms:
            affect = room.Affects.Cast( 'horde_room', -1 )
            affect.Data['host'] = self

        for mobVnum in self.Stage:
            mob = random.choice( self.Rooms ).LoadMob( mobVnum )
            mob.Affects.Cast( 'horde_mob', -1 )
            self.Mobs.append( mob )


    def AddPlayer( self, player ):
        self.Players.append( player )
        player.Affects.Cast( 'horde_player', -1 )
        player.InRoom = self.Rooms[0];


    def PlayerIsDying( self, player ):
        player.InRoom = self.Rooms[0]
        player.Health.Value = 1
        self.StageLost()
        

    def MobIsDead( self, mob ):
        mob.Affects.Clear( 'horde_mob' )
        self.Mobs.remove( mob )
        for player in self.Players:
            player.Write( "You have killed " + mob.ShortDescr + "!\n" )
        
        if( len( self.Mobs ) == 0 ):
            self.StageWon()
        else:
            for player in self.Players:
                player.Write( "Only the following mobs left:\n" )
                for hordeMob in self.Mobs:
                    player.Write( "\t" + hordeMob.ShortDescr + "\n" )
            

    def StageWon( self ):
        for player in self.Players:
            player.Write( "You have survived the stage!\n" )
        self.StageOver()


    def StageLost( self ):
        for player in self.Players:
            player.Write( "You have lost the stage!\n" )
        self.StageOver()


    def StageOver( self ):
        for player in self.Players:
            player.Affects.Clear( 'horde_player' )
        self.Players = []
        
        for room in self.Rooms:
            room.Affects.Clear( 'horde_room' )
        self.Rooms = []

        for mob in self.Mobs:
            mob.Affects.Clear( 'horde_mob' )
        self.Mobs = []
