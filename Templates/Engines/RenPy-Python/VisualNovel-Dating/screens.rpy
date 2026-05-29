## Screens for Dating Visual Novel
# Affection HUD, location picker, date choice, and ending screen

screen affection_display():
    vbox:
        xalign 1.0
        yalign 0.0
        text "Day [day]" size 24
        text "Alex: [alex_affection]" color "#ff6b6b"
        text "Sam: [sam_affection]" color "#4ecdc4"
        text "Jordan: [jordan_affection]" color "#45b7d1"
        text "Riley: [riley_affection]" color "#f9ca24"

screen location_choice():
    frame:
        xalign 0.5
        yalign 0.5
        vbox:
            text "Where will you go today?" size 28
            null height 20
            textbutton "Cafe" action [SetVariable("location", "cafe"), Return("cafe")]
            textbutton "Park" action [SetVariable("location", "park"), Return("park")]
            textbutton "Library" action [SetVariable("location", "library"), Return("library")]
            textbutton "Beach" action [SetVariable("location", "beach"), Return("beach")]

screen date_choice(a, b, c):
    frame:
        xalign 0.5
        yalign 0.5
        vbox:
            text "Who will you spend time with?" size 24
            null height 20
            textbutton "[a]" action Return("a")
            textbutton "[b]" action Return("b")
            textbutton "[c]" action Return("c")

screen ending_screen(ending_name):
    frame:
        xalign 0.5
        yalign 0.5
        vbox:
            text "Ending: [ending_name]" size 36
            null height 30
            text "Thank you for playing!"
            null height 20
            textbutton "Play Again" action MainMenu()
