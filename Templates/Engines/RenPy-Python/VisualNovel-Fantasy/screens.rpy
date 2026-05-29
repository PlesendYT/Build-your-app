## Screens for Fantasy Visual Novel
# Player stat display and companion selection menu

screen stats_display():
    vbox:
        xalign 0.0
        yalign 0.0
        text "Stats" size 20
        text "Bravery: [bravery]"
        text "Wisdom: [wisdom]"
        text "Magic: [magic]"

screen companion_select():
    frame:
        xalign 0.5
        yalign 0.5
        vbox:
            text "Choose your companion:" size 24
            null height 20
            textbutton "Luna (Mage)" action Return("luna")
            textbutton "Thorn (Warrior)" action Return("thorn")
