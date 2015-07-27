import sys
import math

import clr

from mud import *


from System import Func
from System.Linq import Enumerable

from Kaerber.MUD.Entities import *


def debugEntityPrint( ch, ccol ):
    map( lambda c: ch.Write( str( ItemCount( c ) ) + " " + c.ShortDescr + "\n" ), ccol )


def IsCoin( item ):
    return( item.Flags.HasFlag( ItemFlags.Money ) )


def CharCoins( ch ):
    return( ch.Inventory.FindAll( IsCoin ) )


def ItemCount( coin ):
    if( coin.Stack != None ):
        return( coin.Stack.Count )
    return( 1 )


def CoinValue( coin ):
    return( coin.Cost*ItemCount( coin ) )


def MoneyValue( ch ):
    return( reduce( lambda value, coin: value + CoinValue( coin ), CharCoins( ch ), 0 ) )


def ItemSetCount( set, vnum = "" ):
    return(
        reduce(
            lambda count, itemCount: count + itemCount,
            map( ItemCount, filter( lambda item: vnum == "" or item.Vnum == vnum,  set ) ),
            0
        )
    )


def ItemPrice( shop, item ):
    return( int( item.Cost*1.2 ) )


def GetCurrencies():
    currencies = ( 'heaven_gold', 'heaven_silver' )
    return( sorted( map( Item.Create, currencies ), key = lambda item : -item.Cost ) )


def CharPaysMoneyToShop( ch, money, shop ):
    ( coinsPaid, change ) = DetermineMoneyToPay( ch, money )
    coinsChange = DetermineChangeToReturn( change, coinsPaid )
    
    PayMoney( ch, coinsPaid )
    ReturnChange( ch, coinsChange )
    
    return( coinsPaid, coinsChange )


def DetermineMoneyToPay( ch, money ):
    coins = sorted( CharCoins( ch ) , key = lambda item : item.Cost )
    coinsPaid = Aspects.ItemSet()
    for coin in coins:
        coinPaid = Item( coin )

        if( coinPaid.Stack != None ):
            if( CoinValue( coin ) <= money ):
                coinPaid.Stack.Count = coin.Stack.Count
            else:
                coinPaid.Stack.Count = math.ceil( float( money )/coinPaid.Cost )

        coinsPaid.Add( coinPaid )
        money -= CoinValue( coinPaid )
        
        if( money <= 0 ): break
    
    return( coinsPaid, -money )


def DetermineChangeToReturn( change, coinsPaid ):
    coinsChange = Aspects.ItemSet()
    if( change <= 0 ):
        return( coinsChange )

    for coinChange in GetCurrencies():
        if( coinChange.Cost > change ): continue
        
        ( count, change ) = divmod( change, coinChange.Cost )
        countPaidDiff = min( count, ItemSetCount( coinsPaid, coinChange.Vnum ) )
        if( countPaidDiff > 0 ):
            count -= countPaidDiff
            coinsPaid.Remove( coinChange.Vnum, countPaidDiff )
        
        if( coinChange.Stack.MaxCount > 0 ):
            while( count >= coinChange.Stack.MaxCount ):
                fullStack = Item( coinChange )
                fullStack.Stack.Count = fullStack.Stack.MaxCount
                coinsChange.Add( fullStack )
                count -= fullStack.Stack.Count
            
        if( count > 0 ):
            coinChange.Stack.Count = count
            coinsChange.Add( coinChange )
    
    return( coinsChange )


def PayMoney( ch, coinsToPay ):
    for coin in coinsToPay:
        ch.Inventory.Remove( coin.Vnum, ItemCount( coin ) )
   
    ch.InRoom.Event( 'ch_paid_money', EventReturnMethod.None, { 'ch': ch, 'money': coinsToPay } )


def ReturnChange( ch, coinsToReturn ):
    for coin in coinsToReturn:
        ch.Inventory.Add( coin )
   
    ch.InRoom.Event( 'ch_got_change', EventReturnMethod.None, { 'ch': ch, 'money': coinsToReturn } )


def SellToChar( shop, ch, args ):
    if( args.Count == 0 ):
        ch.Write( "Buy what?\n" )
        return;
    
    item = ch.InRoom.Items.Find( args[0], lambda item : item.Affects.Contains( 'shop_goods' ) )
    if( item is None ):
        ch.Write( "There's nothing like that on sale here!\n" )
        return;
    
    if( not ch.InRoom.Event( 'ch_can_buy_item', EventReturnMethod.And, { 'ch': ch, 'item': item } ) ):
        return;
    
    ( money, change ) = CharPaysMoneyToShop( ch, ItemPrice( shop, item ), shop )
    ch.InRoom.Items.Remove( item )
    ch.Inventory.Add( item )
    item.Affects.Clear( 'shop_goods' )

    ch.InRoom.Event( 'ch_bought_item', EventReturnMethod.None, { 'ch': ch, 'item': item, 'money': money, 'change': change } )

    
def ChCanBuyItem( shop, ch, item ):
    if( MoneyValue( ch ) < ItemPrice( shop, item ) ):
        ch.Write( "You can't afford it.\n" )
        return( False )
    return( True )
    
    
def ChBoughtItem( shop, ch, item ):
    pass
