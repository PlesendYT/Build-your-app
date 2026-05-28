## Screens for Comedy Visual Novel

screen chaos_meter():
    vbox:
        xalign 0.5
        yalign 0.0
        text "Chaos Level: [chaos_level]/100" size 20

screen comedy_choice():
    frame:
        xalign 0.5
        yalign 0.5
        vbox:
            text "What do you do?" size 24
            null height 20
            textbutton "Say something hilarious" action Return("joke")
            textbutton "Do something absurd" action Return("absurd")
            textbutton "Play it straight" action Return("straight")
