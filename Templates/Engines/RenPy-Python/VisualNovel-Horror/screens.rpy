## Screens for Horror Visual Novel

screen survival_status():
    vbox:
        xalign 0.0
        yalign 0.0
        text "Sanity: [sanity]"
        text "Battery: [flashlight_battery]%"

screen safe_room_choices():
    frame:
        xalign 0.5
        yalign 0.8
        vbox:
            textbutton "Search the room" action Return("search")
            textbutton "Check the door" action Return("door")
            textbutton "Rest and recover" action Return("rest")
            textbutton "Listen carefully" action Return("listen")
