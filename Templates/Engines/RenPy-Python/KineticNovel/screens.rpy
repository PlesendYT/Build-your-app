## Screens for Kinetic Novel

screen kinetic_ui():
    textbutton "Auto" action Preference("auto-forward", "toggle")
    textbutton "Save" action ShowMenu("save")
    textbutton "Prefs" action ShowMenu("preferences")

## Enable auto-forward for reading experience
init python:
    config.auto_forward_time = 2000
