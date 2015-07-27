def env():
    e = lambda x: x
    
    ch = lambda x: x
    e.ch = ch

    room = lambda x: x
    ch.room = room
    e.room = room

    sword = lambda x: x
    sword.names = ( 'shining', 'sword' )

    room.items = list( ( sword, ) )

    return e
