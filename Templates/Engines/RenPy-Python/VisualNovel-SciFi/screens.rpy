## Screens for Sci-Fi Visual Novel

screen ship_status():
    vbox:
        xalign 1.0
        yalign 0.0
        text "Ship Status" size 20
        text "Hull: [ship_hull]%"
        text "Fuel: [fuel]%"
        text "Morale: [crew_morale]"
        text "Tech Lv: [tech_level]"

screen exploration_choice():
    frame:
        xalign 0.5
        yalign 0.5
        vbox:
            text "Where to explore?" size 24
            null height 20
            textbutton "Abandoned station" action Return("station")
            textbutton "Alien planet" action Return("planet")
            textbutton "Asteroid field" action Return("asteroids")
