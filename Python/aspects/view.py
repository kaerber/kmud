import clr
import System
clr.ImportExtensions( System.Linq )

from aspect import Aspect

from Kaerber.MUD.Entities import *


# See aspect.py for details
def construct():
    return ViewAspect()


class ViewAspect( Aspect ):
    def Saved( self ):
        return False

    def see_condition( self, e ):
        if self.model.IsInCombat:
            vch = self.model.Target
            self.WriteMformat( "$ch(1) $has(1) $str(2)% of health.\n", vch, vch.Health.Condition )


    def this_can_recall( self, e ):
        self.Write( "You pray to the Gods.\n" )

    def this_recalled( self, e ):
        self.view.RenderRoom( self.model.Room )
        self.Write( "\nSuccess!\n" )

    def this_saw_something( self, e ):
        self.Write( e["message"] )

    def this_stopped_fighting_ch1( self, e ):
        self.WriteMformat( "$ch(1) stopped fighting $ch(2).\n", self.model, e["ch1"] )

    def this_has_quit( self, e ):
        self.Write( "Alas, all good things must come to an end.\n" )


    def ch_missed_ch1( self, e ):
        self.WriteMformat( "$chs(1) attack missed $ch(2).\n", e["ch"], e["ch1"] )

    def ch_hit_ch1( self, e ):
        self.WriteMformat( "$ch(1) attack hit $ch(2).\n", e["ch"], e["ch1"] )

    def ch_took_damage_from_ch1( self, e ):
        self.Write( "ch_took_damage_from_ch1: outdated event.\n" )

    def ch_dealt_damage_to_ch1( self, e ):
        self.WriteMformat( "$chs(1) strike dealt $str(3) damage to $ch(2).\n",
            e["ch"], e["ch1"], e["damage"] ) 
            
    def ch_died( self, e ):
        self.WriteMformat( "$ch(1) $is(1) DEAD!\n", e["ch"] )


    def ch_went_from_room_to_room( self, e ):
        assert( self.model != None )

        exitPreposition, exitArticle, exitName = ( 
           ( "to", "the", e["exit"].Name ) if e["exit"] != None 
            else ( "in", "the", "strange direction" ) )

        entranceArticle, entranceName = ( 
           ( "the", e["entrance"].Name ) if e["entrance"] != None 
            else ( "a", "strange direction" ) )

        if self.model == e["ch"]:
            self.WriteFormat( "You walk {0} {1} {2}.\n", exitPreposition, exitArticle, exitName )
            return

        if self.model.Room == e["room_from"]:
            self.WriteMformat( "$ch(1) $has(1) left $str(2) $str(3) $str(4).\n",
                e["ch"],
                exitPreposition,
                exitArticle,
                exitName )

        if self.model.Room == e["room_to"]:
            self.WriteMformat( "$ch(1) $has(1) arrived from $str(2) $str(3).\n",
                e["ch"],
                entranceArticle,
                entranceName )


    def ch_got_item( self, e ):
        self.WriteMformat( "$ch(1) get$s(1) $item(2).\n", e["ch"], e["item"] )

    def ch_got_item_from_container( self, e ):
        self.WriteMformat( "$ch(1) get$s(1) $item(2) from $item(3).\n",
            e["ch"], e["item"], e["container"] )

    def ch_removed_item( self, e ):
        self.WriteMformat( "$ch(1) stopped using $item(2).\n", e["ch"], e["item"] )

    def ch_equipped_item( self, e):
        self.WriteMformat( "$ch(1) equipped $item(2).\n", e["ch"], e["item"] ) 

    def ch_dropped_item( self, e ):
        self.WriteMformat( "$ch(1) dropped $item(2).\n", e["ch"], e["item"] )
                
    def ch_put_item_into_container( self, e ):
        self.WriteMformat( "$ch(1) put $item(2) into $item(3).\n",
            e["ch"], e["item"], e["container"] )

    def ch_bought_item( self, e ):
        self.WriteMformat( "$ch(1) bought $item(2) for $str(3)$str(4).\n",
            e["ch"], e["item"],
            self.view.FormatMoney( e["money"] ),
            " and got {0} as a change".format( self.view.FormatMoney( e["change"] ) ) 
                if e["change"].Any()
                else string.Empty )

    def ch_said_text( self, e ):
        self.WriteMformat( "$ch(1) say$s(1) '$str(2)'.\n", e["ch"], e["text"] ) 

    def ch_recalled_out( self, e ):
        self.WriteMformat( "$ch(1) pray$xs(1) to the Gods and disapper$xs(1).\n", e["ch"] )

    def ch_recalled_in( self, e ):
        self.WriteMformat( "$ch(1) appear$xs(1) in a flash of light.\n", e["ch"] )

    def ch_has_quit( self, e ):
        if self.model != e["ch"]:
            self.WriteMformat( "$ch(1) $has(1) quit the game.\n", e["ch"] );


    def WriteFormat( self, format, *args ):
        self.view.WriteFormat( format, *args )

    def WriteMformat( self, format, *args ):
        self.view.WriteMformat( format, *args )

    def Write( self, message ):
        self.view.Write( message )
