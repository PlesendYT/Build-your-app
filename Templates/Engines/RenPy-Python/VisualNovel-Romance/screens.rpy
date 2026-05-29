## Screens for Romance Visual Novel
# Affection tracker and date activity picker

screen relationship_status():
    vbox:
        xalign 1.0
        yalign 0.0
        text "Connections" size 20
        text "Elena: [elena_affection]" color "#e74c3c"
        text "Marco: [marco_affection]" color "#3498db"
        text "Lea: [lea_affection]" color "#9b59b6"
        text "Trust: [trust_level]"

screen date_activity():
    frame:
        xalign 0.5
        yalign 0.5
        vbox:
            text "What would you like to do?" size 24
            null height 20
            textbutton "Coffee date" action Return("coffee")
            textbutton "Walk in the park" action Return("walk")
            textbutton "Movie night" action Return("movie")
            textbutton "Cooking together" action Return("cook")
