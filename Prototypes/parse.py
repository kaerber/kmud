import re

def parse( args ):
    return parse_lex( args,
        lambda l: parse_handle_lex_errors( l,
        lambda l: parse_item_in_room( l ) ) )

def parse_lex( args, cont ):
    return cont( list( lex( args ) ) )

def parse_handle_lex_errors( lx, cont ):
    error = [ l for l in lx if lx[0] == 'ERROR' ]
    if error:
        return ( ( 'SYNTAX_ERROR', error[0][1] ), )
    return cont( lx )

def parse_item_in_room( lx, cont=None ):
#    return parse_quantity( lx, lambda items: items,
#        lambda lx, prev: parse_order( lx, prev,
#        lambda lx, prev:
    return parse_names( lx, match_entity, select, #prev,
        lambda lx, prev: parse_finish( lx, prev, get_items_in_room ) ) #) )


def parse_quantity( lx, prev_compiled, compile_next) :
    compile = lambda quantity, lx: compile_next( lx,
                                                 lambda selector: prev_compiled(
                                                     lambda: take( quantity, selector() ) ) )
    quantifier = list( parse_token( lx, 'NUMBER',
        lambda lx: parse_token( lx, 'ASTERIX' ) ) )
    if quantifier:
        return compile( int( quantifier[0], lx[2:] ) )

    return compile( 1, lx )

def parse_order( lx, prev_compiled, compile_next ):
    compile = lambda order, lx: compile_next( lx,
                                          lambda selector: prev_compiled(
                                              lambda: skip( order, selector() ) ) )

    selector = list( parse_token( lx, 'NUMBER',
        lambda lx: parse_token( lx, 'DOT' ) ) )
    if selector:
        return compile( int( selector[0] ) - 1, lx[2:] )

    return compile( 0, lx )


def parse_names( lx, apply, prev_compiled, compile_next ):
    compile = lambda names: compile_next( lx[1:],
                                          lambda selector: prev_compiled(
                                              lambda: where(
                                                  lambda entity: apply( entity, names ),
                                                  selector() ) ) )

    ident = list( parse_token( lx, 'IDENTIFIER' ) )
    if ident:
        return compile( ident )

    quoted_string = list( parse_token( lx, 'QUOTED_STRING' ) )
    if quoted_string:
        return compile( [ n for n in quoted_string[0].strip( "'\"" ).split() if n != '' ] )

    return lambda lx, prev: ( 'ERROR', 'names', lx )


def parse_finish( lx, prev_compiled, final_step ):
    if lx:
        return ( 'ERROR', 'finish', lx )

    return lambda ch: prev_compiled( final_step( ch ) )

def select( selector ):
    return selector()

def parse_token( lx, token, cont=None ):
    if not lx: return
    if lx[0][0] != token: return

    r = []
    if cont != None:
        r = list( cont( lx[1:] ) )
        if not r: return

    yield lx[0][1]
    yield from r



def get_items_in_room( ch ):
    return ch.room.items

def match_entity( entity, keywords ):
    return keywords and all( [[name for name in entity.names
                                    if name.lower().startswith( key.lower() ) ]
                               for key in keywords ] )


def skip( number, get_sequence ):
    return get_sequence()[number:]

def take( number, get_sequence ):
    return get_sequence()[:number]

def where( predicate, get_sequence ):
    return [item for item in get_sequence() if predicate( item )]


def lex( text ):
    while text != '':
        lx, text = lexeme( text )
        yield lx

def lexeme( text ):
    rules = [
        ( 'DOT', r'^\.' ),
        ( 'ASTERIX', r'^\*' ),
        ( 'QUOTED_STRING', '^(["\']).*?\\1' ),
        ( 'NUMBER', r'^\d+' ),
        ( 'IDENTIFIER', r'^[a-zA-Z]+' ),
        ( 'WHITESPACE', r'^\s+' ),
        ( 'ERROR', r'^.+' ) ]
    for rule in rules:
        m = re.match( rule[1], text )
        if m:
            return ( ( rule[0], m.group(0) ), text[m.end():] )


def env():
    e = lambda: "env"

    ch = lambda: "ch"
    e.ch = ch

    room = lambda: "room"
    ch.room = room
    e.room = room

    sword = lambda: "shining sword"
    sword.names = ( 'shining', 'sword' )

    shield = lambda: "wooden shield"
    shield.names = ( 'wooden', 'shield' )

    sword2 = lambda: "bronze sword"
    sword2.names = ( 'bronze', 'sword' )

    room.items = list( ( sword, shield, sword2 ) )

    return e
