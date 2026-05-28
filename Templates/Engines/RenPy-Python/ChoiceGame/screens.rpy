## Screens for Choice Game

screen stats_panel():
    frame:
        xalign 0.0
        yalign 0.0
        vbox:
            text "Stats" size 18
            text "Courage: [courage]"
            text "Kindness: [kindness]"
            text "Cunning: [cunning]"

screen multi_choice(title, a, b, c, d):
    frame:
        xalign 0.5
        yalign 0.5
        vbox:
            text "[title]" size 24
            null height 20
            textbutton "[a]" action Return("a")
            textbutton "[b]" action Return("b")
            textbutton "[c]" action Return("c")
            textbutton "[d]" action Return("d")
