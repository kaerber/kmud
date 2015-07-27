import sys
import math

import clr

from mud import *
from aspect import Aspect

from System import Func
from System.Collections.Generic import List
from System.Linq import Enumerable

from Kaerber.MUD.Entities import *

# See aspect.py for details
def construct():
    return ShopAspect()

# Char buys Item event flow:
# ch_will_pay_money [ch, amount]  or
#     a query determining whether char has the ability to pay money (has a handler)
# ch_can_pay_money [ch, amount]  and
#     a query determining whether char can afford the payment
# ch_can_buy_item [ch, item]  and
#     a query determining whether character can buy an item (restrictions, weight, etc )
# ch_pays_money [ch, amount, coins, change]  none
#     an action requesting payment, payer handler receives amount and sends back coins in event args
# ch_paid_money [ch, amount, coins, change]  none
#     a notification that the money were paid
# ch_bought_item [ch, item]  none
#     an action/notification, sending bought item to payer in event args

class ShopAspect( Aspect ):
    def __init__( self ):
        self.Goods = List[ItemVnum]()

    def Serialize( self ):
        return dict( [ ( "Goods", [ vnum.ToString() for vnum in self.Goods if not vnum.IsEmpty ] ) ] )


    def Deserialize( self, data ):
        if "Goods" in data:
            self.Goods = List[ItemVnum]( [ ItemVnum.FromString( vnum, True ) for vnum in data["Goods"] ] )
        return self


    def debugEntityPrint( ch, ccol ):
        map( lambda c: ch.Write( str( ItemCount( c ) ) + " " + c.ShortDescr + "\n" ), ccol )


    def ItemPrice( self, item ):
        return( int( item.Cost*1.2 ) )



    def SellToChar( self, ch, args ):
        if( args.Count == 0 ):
            ch.Write( "Buy what?\n" )
            return;
    
        item = ch.InRoom.Items.Find( args[0], lambda item : item.Affects.Contains( 'shop_goods' ) )
        if( item is None ):
            ch.Write( "There's nothing like that on sale here!\n" )
            return;
    
        price = self.ItemPrice( item )
        if( not ch.Event( 'ch_will_pay_money', EventReturnMethod.Or, { 'ch': ch, 'amount': price } ) ):
            return

        if( not ch.Event( 'ch_can_pay_money', EventReturnMethod.And, { 'ch': ch, 'amount': price } ) ):
            return

        if( not ch.InRoom.Event( 'ch_can_buy_item', EventReturnMethod.And, { 'ch': ch, 'item': item } ) ):
            return
    
        ch_pays_money = Event.Create( 'ch_pays_money', EventReturnMethod.None,
            { 'ch': ch, 'amount': price, 'coins': None, 'change': None } )
        ch.InRoom.Event( ch_pays_money )
        # todo take and store coins from event

        ch.InRoom.Event( 'ch_paid_money', EventReturnMethod.None, 
            { 'ch': ch,
              'amount': price,
              'coins': ch_pays_money.Parameters['coins'],
              'change': ch_pays_money.Parameters['change'] }
        )

        ( money, change ) = CharPaysMoneyToShop( ch, ItemPrice( shop, item ), shop )
        ch.InRoom.Items.Remove( item )
        item.Affects.Clear( 'shop_goods' )

        ch.InRoom.Event( 'ch_bought_item', EventReturnMethod.None, { 'ch': ch, 'item': item } )

    # editor support
    def DefaultValue( self ):
        return None

    def GetMember( self, key ):
        return getattr( self, key )

    def SetMember( self, key, value ):
        raise System.InvalidOperationException( "Shop aspects do not support setting members directly." )
        

    def SetValue( self, value, view ):
        raise System.NotSupportedException( "Shop aspects do not support setting directly." )

    def AddMember( self, key, view ):
        raise System.NotSupportedException( "Shop aspects do not support adding members." )

    def RemoveMember( self, key, view ):
        raise System.NotSupportedException( "Shop aspects do not support removing members." )

    def Editables( self ):
        return [ "Goods" ]

    def Members( self ):
        return [key for key in self.Editables() if self.GetMember( key ) != None]